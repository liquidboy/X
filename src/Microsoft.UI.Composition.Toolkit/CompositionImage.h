#pragma once

#include "CompositionImage.g.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    struct CompositionImage : CompositionImageT<CompositionImage>
    {
        CompositionImage() = default;

        void Initialize(
            Windows::UI::Composition::Compositor const& compositor,
            CompositionGraphicsDevice const& graphicsDevice,
            Windows::Foundation::Uri const& uri,
            Windows::Storage::StorageFile const& file,
            Toolkit::CompositionImageOptions const& options);

        void Initialize(
            Windows::UI::Composition::Compositor const& compositor,
            CompositionGraphicsDevice const& graphicsDevice,
            array_view<byte const> pixels,
            int pixelWidth,
            int pixelHeight);

        event_token ImageLoaded(CompositionImageLoadedEventHandler const& handler);
        void ImageLoaded(event_token const& token);

        Windows::Foundation::Size Size();
        Windows::UI::Composition::ICompositionSurface Surface();

    private:

        Windows::Foundation::IAsyncAction LoadImageAsync();
        Windows::Foundation::IAsyncOperation<Windows::Storage::Streams::IBuffer> LoadImageFromFileAsync(Windows::Storage::StorageFile file);
        com_ptr<IWICBitmapSource> DecodeBufferIntoBitmap(Windows::Storage::Streams::IBuffer const& imageRawBuffer, SIZE sizeDecode);

        void DrawBitmapOnSurface(IWICBitmapSource* bitmapSource);
        void DrawBitmapOnSurface(array_view<byte const> buffer, int pixelWidth, int pixelHeight);


        agile_event<CompositionImageLoadedEventHandler> _imageLoaded;
        Windows::UI::Composition::Compositor _compositor{ nullptr };
        Toolkit::CompositionImageOptions _imageOptions{nullptr};
        CompositionGraphicsDevice _graphicsDevice{ nullptr };
        Windows::Foundation::Uri _uri{ nullptr };
        Windows::Storage::StorageFile _file{ nullptr };

        Windows::UI::Composition::ICompositionSurface _compositionSurface{ nullptr };
        com_ptr<ABI::Windows::UI::Composition::ICompositionDrawingSurfaceInterop> _compositionSurfaceInterop;

        Windows::Foundation::IAsyncAction _loadAsyncAction;

        SIZE _sizeBitmapSource{};
        SIZE _sizeCompositionSurface{};
        SIZE _sizeDecode{};
    };
}
