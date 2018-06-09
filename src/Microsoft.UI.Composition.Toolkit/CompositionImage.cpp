#include "pch.h"
#include "CompositionImage.h"

struct __declspec(uuid("905a0fef-bc53-11df-8c49-001e4fc686da")) IBufferByteAccess : ::IUnknown
{
    virtual HRESULT __stdcall Buffer(uint8_t** value) = 0;
};

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    using namespace Windows::Foundation;
    using namespace Windows::Storage;
    using namespace Windows::Storage::Streams;
    using namespace Windows::UI::Composition;
    using namespace Windows::Graphics::DirectX;

    void CompositionImage::Initialize(
        Compositor const& compositor,
        CompositionGraphicsDevice const& graphicsDevice,
        Uri const& uri,
        StorageFile const& file,
        Toolkit::CompositionImageOptions const& options)
    {
        WINRT_ASSERT(_compositor == nullptr);

        if (uri != nullptr && file != nullptr)
        {
            throw hresult_invalid_argument(L"CompositionImage cannot be created with both a file and uri.");
        }

        if (uri == nullptr && file == nullptr)
        {
            throw hresult_invalid_argument(L"CompositionImage requires either a file or uri to be created.");
        }

        _compositor = compositor;
        _graphicsDevice = graphicsDevice;
        _uri = uri;
        _file = file;
        _imageOptions = options;

        if (options != nullptr)
        {
            _sizeDecode.cx = options.DecodeWidth();
            _sizeDecode.cy = options.DecodeHeight();
        }

        Windows::Foundation::Size initialPixelSize{ static_cast<float>(_sizeDecode.cx),static_cast<float>(_sizeDecode.cy) };

        _compositionSurface = graphicsDevice.CreateDrawingSurface(
            initialPixelSize,
            DirectXPixelFormat::B8G8R8A8UIntNormalized,
            DirectXAlphaMode::Premultiplied);

        _sizeCompositionSurface = _sizeDecode;
        _compositionSurface.as(_compositionSurfaceInterop);

        graphicsDevice.DeviceLost([&, ref = static_cast<Toolkit::CompositionImage>(*this)](auto&&)
        {
            if (_loadAsyncAction != nullptr)
            {
                _loadAsyncAction.Cancel();
            }

            _loadAsyncAction = LoadImageAsync();
        });

        _loadAsyncAction = LoadImageAsync();
    }

    void CompositionImage::Initialize(
        Compositor const& compositor,
        CompositionGraphicsDevice const& graphicsDevice,
        array_view<byte const> pixels,
        int pixelWidth,
        int pixelHeight)
    {
        if (pixels.empty())
        {
            throw hresult_invalid_argument(L"CompositionImage requires a buffer to be created.");
        }

        _compositor = compositor;
        _graphicsDevice = graphicsDevice;
        _sizeDecode.cx = pixelWidth;
        _sizeDecode.cy = pixelHeight;

        Windows::Foundation::Size initialPixelSize;
        initialPixelSize.Width = static_cast<float>(pixelWidth);
        initialPixelSize.Height = static_cast<float>(pixelHeight);

        _compositionSurface = graphicsDevice.CreateDrawingSurface(
            initialPixelSize,
            DirectXPixelFormat::B8G8R8A8UIntNormalized,
            DirectXAlphaMode::Premultiplied);

        _compositionSurface.as(_compositionSurfaceInterop);

        graphicsDevice.DeviceLost([&, ref = static_cast<Toolkit::CompositionImage>(*this)](auto&&)
        {
            if (_loadAsyncAction != nullptr)
            {
                _loadAsyncAction.Cancel();
            }

            _loadAsyncAction = LoadImageAsync();
        });

        DrawBitmapOnSurface(pixels, pixelWidth, pixelHeight);
    }

    IAsyncAction CompositionImage::LoadImageAsync()
    {
        try
        {
            IBuffer imageRawBuffer;

            if (_uri != nullptr)
            {
                StorageFile file{ nullptr };
                hstring const schemeName = _uri.SchemeName();

                if (schemeName == L"http" || schemeName == L"https")
                {
                    file = co_await StorageFile::CreateStreamedFileFromUriAsync(_uri.Extension(), _uri, nullptr);
                }
                else if (schemeName == L"file")
                {
                    throw hresult_invalid_argument(L"Uri cannot have the file scheme, use CreateCompositionImageFromFile instead.");
                }
                else
                {
                    file = co_await StorageFile::GetFileFromApplicationUriAsync(_uri);
                }

                imageRawBuffer = co_await LoadImageFromFileAsync(file);
            }
            else if (_file != nullptr)
            {
                imageRawBuffer = co_await LoadImageFromFileAsync(_file);
            }

            com_ptr<IWICBitmapSource> bitmapSource = DecodeBufferIntoBitmap(imageRawBuffer, _sizeDecode);

            UINT cxSizeBitmap;
            UINT cySizeBitmap;
            check_hresult(bitmapSource->GetSize(&cxSizeBitmap, &cySizeBitmap));

            _sizeBitmapSource.cx = cxSizeBitmap;
            _sizeBitmapSource.cy = cySizeBitmap;

            DrawBitmapOnSurface(bitmapSource.get());

            _imageLoaded(*this, CompositionImageLoadStatus::Success);
        }
        catch (hresult_error const&)
        {
            _imageLoaded(*this, CompositionImageLoadStatus::Other);

        }
    }

    IAsyncOperation<IBuffer> CompositionImage::LoadImageFromFileAsync(StorageFile const file)
    {
        IRandomAccessStream stream = co_await file.OpenAsync(FileAccessMode::Read);

        DataReader reader(stream);

        uint32_t const numBytesLoaded = co_await reader.LoadAsync(static_cast<uint32_t>(stream.Size()));

        return reader.ReadBuffer(numBytesLoaded);
    }

    com_ptr<IWICBitmapSource> CompositionImage::DecodeBufferIntoBitmap(IBuffer const& imageRawBuffer, SIZE sizeDecode)
    {
        com_ptr<IWICStream> wicStream;
        com_ptr<IWICBitmapDecoder> bitmapDecoder;
        com_ptr<IWICBitmapFrameDecode> bitmapFrame;
        com_ptr<IWICBitmapSource> bitmapSource;
        com_ptr<IWICBitmapSourceTransform> sourceTransform;

        GUID pixelFormat = GUID_NULL;

        UINT cxImage = 0;
        UINT cyImage = 0;
        UINT cxScaled = 0;
        UINT cyScaled = 0;

        com_ptr<IWICImagingFactory> imagingFactory;

        check_hresult(CoCreateInstance(
            CLSID_WICImagingFactory,
            NULL,
            CLSCTX_INPROC_SERVER,
            IID_IWICImagingFactory,
            imagingFactory.put_void()
        ));

        unsigned int bufferLength = imageRawBuffer.Length();
        com_ptr<IBufferByteAccess> byteAccess = imageRawBuffer.as<IBufferByteAccess>();

        BYTE* bytes = nullptr;
        check_hresult(byteAccess->Buffer(&bytes));

        check_hresult(imagingFactory->CreateStream(wicStream.put()));
        check_hresult(wicStream->InitializeFromMemory(bytes, bufferLength));

        check_hresult(imagingFactory->CreateDecoderFromStream(
            wicStream.get(),
            NULL,
            WICDecodeMetadataCacheOnDemand,
            bitmapDecoder.put()));

        check_hresult(bitmapDecoder->GetFrame(0, bitmapFrame.put()));
        bitmapSource = bitmapFrame;

        check_hresult(bitmapSource->GetPixelFormat(&pixelFormat));

        REFWICPixelFormatGUID desiredPixelFormat = GUID_WICPixelFormat32bppPBGRA;

        check_hresult(bitmapSource->GetSize(&cxImage, &cyImage));

        if (sizeDecode.cx == 0)
        {
            sizeDecode.cx = cxImage;
        }

        if (sizeDecode.cy == 0)
        {
            sizeDecode.cy = cyImage;
        }

        if ((cxImage > (UINT)sizeDecode.cx) ||
            (cyImage > (UINT)sizeDecode.cy))
        {
            if ((sizeDecode.cx * cyImage) < (sizeDecode.cy * cxImage))
            {
                cxScaled = sizeDecode.cx;
                cyScaled = cxScaled * cyImage / cxImage;
            }
            else
            {
                cyScaled = sizeDecode.cy;
                cxScaled = cyScaled * cxImage / cyImage;
            }
        }
        else
        {
            cxScaled = cxImage;
            cyScaled = cyImage;
        }

        cxScaled = max(1u, cxScaled);
        cyScaled = max(1u, cyScaled);

        if (cxImage >= INT_MAX || cyImage >= INT_MAX)
        {
            throw hresult_invalid_argument(L"Image size is too large to decode.");
        }

        if ((pixelFormat != GUID_NULL) && (pixelFormat != desiredPixelFormat))
        {
            com_ptr<IWICFormatConverter> formatConverter;

            check_hresult(imagingFactory->CreateFormatConverter(formatConverter.put()));

            check_hresult(formatConverter->Initialize(
                bitmapSource.get(),               // Input bitmap to convert
                desiredPixelFormat,               // Destination pixel format
                WICBitmapDitherTypeNone,          // Specified dither pattern
                NULL,                             // Specify a particular palette
                0.0f,                             // Alpha threshold
                WICBitmapPaletteTypeCustom        // Palette translation type
            ));

            bitmapSource = formatConverter;
        }

        if ((cxScaled != cxImage) || (cyScaled != cyImage))
        {
            com_ptr<IWICBitmapScaler> scaler;

            check_hresult(imagingFactory->CreateBitmapScaler(scaler.put()));

            WICBitmapInterpolationMode interpolationMode = WICBitmapInterpolationModeFant;

            if ((cxScaled * 2 >= cxImage) && (cyScaled * 2 >= cyImage))
            {
                interpolationMode = WICBitmapInterpolationModeLinear;
            }

            check_hresult(scaler->Initialize(
                bitmapSource.get(),
                cxScaled,
                cyScaled,
                interpolationMode));

            bitmapSource = scaler;
        }

        return bitmapSource;
    }

    void CompositionImage::DrawBitmapOnSurface(IWICBitmapSource* bitmapSource)
    {
        com_ptr<ID2D1DeviceContext> d2d1DeviceContext;
        com_ptr<ID2D1Bitmap> d2d1BitmapSource;
        com_ptr<ID2D1BitmapRenderTarget> compatibleRenderTarget;

        bool drawingBegun = false;

        _graphicsDevice.AcquireDrawingLock();

        if ((_sizeCompositionSurface.cx != _sizeBitmapSource.cx) ||
            (_sizeCompositionSurface.cx != _sizeBitmapSource.cy))
        {
            HRESULT hr = _compositionSurfaceInterop->Resize(_sizeBitmapSource);
            if (FAILED(hr))
            {
                throw hresult_error(hr, L"Failed to resize composition image surface.");
            }

            _sizeCompositionSurface = _sizeBitmapSource;
        }

        RECT rect;
        rect.left = 0;
        rect.top = 0;
        rect.right = _sizeBitmapSource.cx;
        rect.bottom = _sizeBitmapSource.cy;
        POINT offset;

        check_hresult(_compositionSurfaceInterop->BeginDraw(
            &rect,
            __uuidof(d2d1DeviceContext),
            d2d1DeviceContext.put_void(),
            &offset));
        drawingBegun = true;

        check_hresult(d2d1DeviceContext->CreateCompatibleRenderTarget(compatibleRenderTarget.put()));

        check_hresult(compatibleRenderTarget->CreateBitmapFromWicBitmap(
            bitmapSource,
            nullptr,
            d2d1BitmapSource.put()));

        D2D1_RECT_F destRect;
        destRect.left = static_cast<float>(offset.x);
        destRect.top = static_cast<float>(offset.y);
        destRect.right = static_cast<float>(destRect.left + _sizeBitmapSource.cx);
        destRect.bottom = static_cast<float>(destRect.top + _sizeBitmapSource.cy);

        D2D1_RECT_F sourceRect;
        sourceRect.left = 0;
        sourceRect.top = 0;
        sourceRect.right = static_cast<float>(_sizeBitmapSource.cx);
        sourceRect.bottom = static_cast<float>(_sizeBitmapSource.cy);

        d2d1DeviceContext->SetPrimitiveBlend(D2D1_PRIMITIVE_BLEND_COPY);
        d2d1DeviceContext->DrawBitmap(
            d2d1BitmapSource.get(),
            &destRect,
            1.0f,
            D2D1_BITMAP_INTERPOLATION_MODE_LINEAR,
            &sourceRect);

        WINRT_VERIFY_(S_OK, _compositionSurfaceInterop->EndDraw());

        _graphicsDevice.ReleaseDrawingLock();
    }

    void CompositionImage::DrawBitmapOnSurface(array_view<byte const> buffer, int pixelWidth, int pixelHeight)
    {
        com_ptr<ID2D1DeviceContext> d2d1DeviceContext;
        com_ptr<ID2D1Bitmap> d2d1BitmapSource;
        com_ptr<ID2D1BitmapRenderTarget> compatibleRenderTarget;

        bool drawingBegun = false;

        _graphicsDevice.AcquireDrawingLock();

        RECT rect;
        rect.left = 0;
        rect.top = 0;
        rect.right = pixelWidth;
        rect.bottom = pixelHeight;
        POINT offset;

        check_hresult(_compositionSurfaceInterop->BeginDraw(
            &rect,
            __uuidof(d2d1DeviceContext),
            d2d1DeviceContext.put_void(),
            &offset));
        drawingBegun = true;

        check_hresult(d2d1DeviceContext->CreateCompatibleRenderTarget(compatibleRenderTarget.put()));

        D2D1_SIZE_U d2d1Size = D2D1::SizeU(pixelWidth, pixelHeight);

        D2D1_BITMAP_PROPERTIES bitmapProperties =
            D2D1::BitmapProperties(
                D2D1::PixelFormat(DXGI_FORMAT_B8G8R8A8_UNORM, D2D1_ALPHA_MODE_PREMULTIPLIED));

        HRESULT hr = compatibleRenderTarget->CreateBitmap(d2d1Size, buffer.data(), d2d1Size.width * 4, &bitmapProperties, d2d1BitmapSource.put());
        if (FAILED(hr))
        {
            throw hresult_error(hr, L"Failed to create bitmap form pixels.");
        }

        D2D1_RECT_F d2d1Rect;
        d2d1Rect.left = static_cast<float>(offset.x);
        d2d1Rect.top = static_cast<float>(offset.y);
        d2d1Rect.right = static_cast<float>(d2d1Rect.left + pixelWidth);
        d2d1Rect.bottom = static_cast<float>(d2d1Rect.top + pixelHeight);

        d2d1DeviceContext->SetPrimitiveBlend(D2D1_PRIMITIVE_BLEND_COPY);

        d2d1DeviceContext->DrawBitmap(
            d2d1BitmapSource.get(),
            &d2d1Rect,
            1.0f,
            D2D1_BITMAP_INTERPOLATION_MODE_LINEAR,
            &d2d1Rect);

        WINRT_VERIFY_(S_OK, _compositionSurfaceInterop->EndDraw());

        _graphicsDevice.ReleaseDrawingLock();

    }

    event_token CompositionImage::ImageLoaded(CompositionImageLoadedEventHandler const& handler)
    {
        return _imageLoaded.add(handler);
    }

    void CompositionImage::ImageLoaded(event_token const& token)
    {
        _imageLoaded.remove(token);
    }

    Windows::Foundation::Size CompositionImage::Size()
    {
        return
        {
            static_cast<float>(_sizeBitmapSource.cx),
            static_cast<float>(_sizeBitmapSource.cy)
        };
    }

    Windows::UI::Composition::ICompositionSurface CompositionImage::Surface()
    {
        return _compositionSurface;
    }
}
