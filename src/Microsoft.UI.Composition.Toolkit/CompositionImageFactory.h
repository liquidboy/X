#pragma once

#include "CompositionImageFactory.g.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    struct CompositionImageFactory : CompositionImageFactoryT<CompositionImageFactory>
    {
        CompositionImageFactory(Windows::UI::Composition::Compositor const& compositor, Toolkit::CompositionGraphicsDevice const& device);

        Toolkit::CompositionImage CreateImageFromUri(Windows::Foundation::Uri const& uri);

        Toolkit::CompositionImage CreateImageFromUri(Windows::Foundation::Uri const& uri, Toolkit::CompositionImageOptions const& options);

        Toolkit::CompositionImage CreateImageFromFile(Windows::Storage::StorageFile const& file);

        Toolkit::CompositionImage CreateImageFromFile(Windows::Storage::StorageFile const& file, Toolkit::CompositionImageOptions const& options);

        Toolkit::CompositionImage CreateImageFromPixels(array_view<uint8_t const> pixels, int32_t pixelWidth, int32_t pixelHeight);

        static Toolkit::CompositionImageFactory CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor);

    private:

        Windows::UI::Composition::Compositor m_compositor{ nullptr };
        Toolkit::CompositionGraphicsDevice m_device{ nullptr };
    };
}

namespace winrt::Microsoft::UI::Composition::Toolkit::factory_implementation
{
    struct CompositionImageFactory : CompositionImageFactoryT<CompositionImageFactory, implementation::CompositionImageFactory>
    {
    };
}
