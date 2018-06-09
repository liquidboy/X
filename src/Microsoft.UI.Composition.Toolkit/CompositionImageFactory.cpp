#include "pch.h"
#include "CompositionImageFactory.h"
#include "CompositionImageOptions.h"
#include "CompositionImage.h"
#include "CompositionGraphicsDevice.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    using namespace Windows::Foundation;
    using namespace Windows::UI::Composition;

    CompositionImageFactory::CompositionImageFactory(
        Compositor const& compositor,
        Toolkit::CompositionGraphicsDevice const& device) :
        m_compositor(compositor),
        m_device(device)
    {
    }

    Toolkit::CompositionImage CompositionImageFactory::CreateImageFromUri(Windows::Foundation::Uri const& uri)
    {
        return CreateImageFromUri(uri, make<CompositionImageOptions>());
    }

    Toolkit::CompositionImage CompositionImageFactory::CreateImageFromUri(Windows::Foundation::Uri const& uri, Toolkit::CompositionImageOptions const& options)
    {
        auto image = make<CompositionImage>();

        from_abi<CompositionImage, Toolkit::ICompositionImage>(image)->Initialize(m_compositor, m_device, uri, nullptr, options);

        return image;
    }

    Toolkit::CompositionImage CompositionImageFactory::CreateImageFromFile(Windows::Storage::StorageFile const& file)
    {
        return CreateImageFromFile(file, make<CompositionImageOptions>());
    }

    Toolkit::CompositionImage CompositionImageFactory::CreateImageFromFile(Windows::Storage::StorageFile const& file, Toolkit::CompositionImageOptions const& options)
    {
        auto image = make<CompositionImage>();

        from_abi<CompositionImage, Toolkit::ICompositionImage>(image)->Initialize(m_compositor, m_device, nullptr, file, options);

        return image;
    }

    Toolkit::CompositionImage CompositionImageFactory::CreateImageFromPixels(array_view<uint8_t const> pixels, int32_t pixelWidth, int32_t pixelHeight)
    {
        auto image = make<CompositionImage>();

        from_abi<CompositionImage, Toolkit::ICompositionImage>(image)->Initialize(m_compositor, m_device, pixels, pixelWidth, pixelHeight);

        return image;
    }

    Toolkit::CompositionImageFactory CompositionImageFactory::CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor)
    {
        if (compositor == nullptr)
        {
            throw hresult_invalid_argument();
        }

        Toolkit::CompositionGraphicsDevice device = make<CompositionGraphicsDevice>(compositor);
        return make<CompositionImageFactory>(compositor, device);
    }
}
