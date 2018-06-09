#pragma once

#include "CompositionGraphicsDevice.g.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    struct CompositionGraphicsDevice : CompositionGraphicsDeviceT<CompositionGraphicsDevice>
    {
        CompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor);
        ~CompositionGraphicsDevice();

        event_token DeviceLost(CompositionGraphicsDeviceLostEventHandler const& handler);
        void DeviceLost(event_token const& token);

        Windows::UI::Composition::ICompositionSurface CreateDrawingSurface(
            Windows::Foundation::Size const& sizePixels,
            Windows::Graphics::DirectX::DirectXPixelFormat const& pixelFormat,
            Windows::Graphics::DirectX::DirectXAlphaMode const& alphaMode);

        void AcquireDrawingLock();
        void ReleaseDrawingLock();
        void Close();

        static Toolkit::CompositionGraphicsDevice CreateCompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor);

    private:

        void InitializeDX();
        void UninitializeDX();
        static void __stdcall OnDeviceLostCallback(PTP_CALLBACK_INSTANCE, PVOID context, PTP_WAIT, TP_WAIT_RESULT);

        Windows::UI::Composition::Compositor m_compositor;
        agile_event<CompositionGraphicsDeviceLostEventHandler> m_deviceLost;

        com_ptr<::IUnknown> _graphicsFactoryBackingDXDevice;
        Windows::UI::Composition::CompositionGraphicsDevice _igraphicsDevice{ nullptr };
        com_ptr<ID3D11Device4> _d3dDevice4;
        HANDLE _deviceLostEvent{ nullptr };
        DWORD  _deviceLostRegistrationCookie{};
        PTP_WAIT _threadPoolWait{ nullptr };

        std::mutex _stateLock;
        std::mutex _drawingLock;
    };
}

namespace winrt::Microsoft::UI::Composition::Toolkit::factory_implementation
{
    struct CompositionGraphicsDevice : CompositionGraphicsDeviceT<CompositionGraphicsDevice, implementation::CompositionGraphicsDevice>
    {
    };
}
