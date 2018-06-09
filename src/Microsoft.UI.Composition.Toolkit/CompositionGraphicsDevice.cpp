#include "pch.h"
#include "CompositionGraphicsDevice.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    using namespace Windows::Foundation;
    using namespace Windows::Graphics::DirectX;
    using namespace Windows::UI::Composition;
    namespace ABI = ABI::Windows::UI::Composition;

    CompositionGraphicsDevice::CompositionGraphicsDevice(Compositor const& compositor) :
        m_compositor(compositor)
    {
        std::lock_guard<std::mutex> lock(_stateLock);
        InitializeDX();

        auto interop = compositor.as<ABI::ICompositorInterop>();
        check_hresult(interop->CreateGraphicsDevice(_graphicsFactoryBackingDXDevice.get(), reinterpret_cast<ABI::ICompositionGraphicsDevice**>(put_abi(_igraphicsDevice))));
    }

    CompositionGraphicsDevice::~CompositionGraphicsDevice()
    {
        UninitializeDX();
    }

    event_token CompositionGraphicsDevice::DeviceLost(CompositionGraphicsDeviceLostEventHandler const& handler)
    {
        return m_deviceLost.add(handler);
    }

    void CompositionGraphicsDevice::DeviceLost(event_token const& token)
    {
        m_deviceLost.remove(token);
    }

    Windows::UI::Composition::ICompositionSurface CompositionGraphicsDevice::CreateDrawingSurface(
        Size const& sizePixels,
        DirectXPixelFormat const& pixelFormat,
        DirectXAlphaMode const& alphaMode)
    {
        std::lock_guard<std::mutex> lock(_stateLock);

        return _igraphicsDevice.CreateDrawingSurface(sizePixels, pixelFormat, alphaMode);
    }

    void CompositionGraphicsDevice::AcquireDrawingLock()
    {
        _drawingLock.lock();
    }

    void CompositionGraphicsDevice::ReleaseDrawingLock()
    {
        _drawingLock.unlock();
    }

    void CompositionGraphicsDevice::Close()
    {
        UninitializeDX();
    }

    Toolkit::CompositionGraphicsDevice CompositionGraphicsDevice::CreateCompositionGraphicsDevice(Compositor const& compositor)
    {
        return make<CompositionGraphicsDevice>(compositor);
    }

    void CompositionGraphicsDevice::InitializeDX()
    {
        UninitializeDX();

        UINT creationFlags = D3D11_CREATE_DEVICE_BGRA_SUPPORT;

        D3D_FEATURE_LEVEL featureLevels[] =
        {
            D3D_FEATURE_LEVEL_11_1,
            D3D_FEATURE_LEVEL_11_0,
            D3D_FEATURE_LEVEL_10_1,
            D3D_FEATURE_LEVEL_10_0,
            D3D_FEATURE_LEVEL_9_3,
            D3D_FEATURE_LEVEL_9_2,
            D3D_FEATURE_LEVEL_9_1
        };

        com_ptr<ID3D11Device> d3dDevice;
        com_ptr<ID3D11DeviceContext> d3dContext;
        D3D_FEATURE_LEVEL usedFeatureLevel{};

        check_hresult(D3D11CreateDevice(
            nullptr,
            D3D_DRIVER_TYPE_HARDWARE,
            nullptr,
            creationFlags,
            featureLevels,
            ARRAYSIZE(featureLevels),
            D3D11_SDK_VERSION,
            d3dDevice.put(),
            &usedFeatureLevel,
            d3dContext.put()));

        com_ptr<ID2D1Factory1> d2dFactory;

        check_hresult(D2D1CreateFactory(D2D1_FACTORY_TYPE_MULTI_THREADED, __uuidof(ID2D1Factory1), d2dFactory.put_void()));
        auto dxgiDevice = d3dDevice.as<IDXGIDevice>();

        com_ptr<ID2D1Device> d2d1Device;
        check_hresult(d2dFactory->CreateDevice(dxgiDevice.get(), d2d1Device.put()));

        _graphicsFactoryBackingDXDevice = d2d1Device;

        d3dDevice.as(_d3dDevice4);

        _deviceLostEvent = CreateEvent(
            NULL,  // Attributes
            TRUE,  // Manual reset
            FALSE, // Initial state
            NULL); // Name

        if (_deviceLostEvent == nullptr)
        {
            throw_last_error();
        }

        _threadPoolWait = CreateThreadpoolWait(CompositionGraphicsDevice::OnDeviceLostCallback, (PVOID)this, NULL);

        if (_threadPoolWait == nullptr)
        {
            throw_last_error();
        }

        SetThreadpoolWait(_threadPoolWait, _deviceLostEvent, NULL);
        check_hresult(_d3dDevice4->RegisterDeviceRemovedEvent(_deviceLostEvent, &_deviceLostRegistrationCookie));
    }

    void CompositionGraphicsDevice::UninitializeDX()
    {
        if ((_deviceLostRegistrationCookie != 0) && (_d3dDevice4 != nullptr))
        {
            _d3dDevice4->UnregisterDeviceRemoved(_deviceLostRegistrationCookie);
            _deviceLostRegistrationCookie = 0;
        }

        if (_deviceLostEvent != NULL)
        {
            CloseHandle(_deviceLostEvent);
            _deviceLostEvent = NULL;
        }

        if (_threadPoolWait != NULL)
        {
            CloseThreadpoolWait(_threadPoolWait);
            _threadPoolWait = NULL;
        }

        _d3dDevice4 = nullptr;
    }

    void CALLBACK CompositionGraphicsDevice::OnDeviceLostCallback(PTP_CALLBACK_INSTANCE, PVOID context, PTP_WAIT, TP_WAIT_RESULT)
    {
        CompositionGraphicsDevice* pThis = reinterpret_cast<CompositionGraphicsDevice*>(context);

        pThis->InitializeDX();

        auto graphicsDeviceInterop = pThis->_igraphicsDevice.as<ABI::ICompositionGraphicsDeviceInterop>();

        graphicsDeviceInterop->SetRenderingDevice(pThis->_graphicsFactoryBackingDXDevice.get());

        pThis->m_deviceLost(*pThis);
    }

}
