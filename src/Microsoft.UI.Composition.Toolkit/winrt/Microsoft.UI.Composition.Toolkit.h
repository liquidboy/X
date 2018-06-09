// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once
#include "winrt/base.h"

WINRT_WARNING_PUSH

static_assert(winrt::impl::make_constexpr_string(CPPWINRT_VERSION) == "1.0.170906.1", "Mismatched component and base headers.");
#include "winrt/Windows.Foundation.h"
#include "winrt/Windows.Foundation.Collections.h"
#include "winrt/impl/Windows.Foundation.2.h"
#include "winrt/impl/Windows.Graphics.DirectX.2.h"
#include "winrt/impl/Windows.Storage.2.h"
#include "winrt/impl/Windows.UI.Composition.2.h"
#include "winrt/impl/Microsoft.UI.Composition.Toolkit.2.h"

namespace winrt::impl {

template <typename D> event_token consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::DeviceLost(Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice)->add_DeviceLost(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice> consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::DeviceLost(auto_revoke_t, Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler const& handler) const
{
    return impl::make_event_revoker<D, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>(this, &abi_t<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>::remove_DeviceLost, DeviceLost(handler));
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::DeviceLost(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice)->remove_DeviceLost(get_abi(token)));
}

template <typename D> Windows::UI::Composition::ICompositionSurface consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::CreateDrawingSurface(Windows::Foundation::Size const& sizePixels, Windows::Graphics::DirectX::DirectXPixelFormat const& pixelFormat, Windows::Graphics::DirectX::DirectXAlphaMode const& alphaMode) const
{
    Windows::UI::Composition::ICompositionSurface surface{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice)->CreateDrawingSurface(get_abi(sizePixels), get_abi(pixelFormat), get_abi(alphaMode), put_abi(surface)));
    return surface;
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::AcquireDrawingLock() const
{
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice)->AcquireDrawingLock());
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>::ReleaseDrawingLock() const
{
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice)->ReleaseDrawingLock());
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDeviceStatics<D>::CreateCompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor) const
{
    Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice device{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics)->CreateCompositionGraphicsDevice(get_abi(compositor), put_abi(device)));
    return device;
}

template <typename D> event_token consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>::ImageLoaded(Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler const& handler) const
{
    event_token token{};
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImage)->add_ImageLoaded(get_abi(handler), put_abi(token)));
    return token;
}

template <typename D> event_revoker<Microsoft::UI::Composition::Toolkit::ICompositionImage> consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>::ImageLoaded(auto_revoke_t, Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler const& handler) const
{
    return impl::make_event_revoker<D, Microsoft::UI::Composition::Toolkit::ICompositionImage>(this, &abi_t<Microsoft::UI::Composition::Toolkit::ICompositionImage>::remove_ImageLoaded, ImageLoaded(handler));
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>::ImageLoaded(event_token const& token) const
{
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImage)->remove_ImageLoaded(get_abi(token)));
}

template <typename D> Windows::Foundation::Size consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>::Size() const noexcept
{
    Windows::Foundation::Size value{};
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImage)->get_Size(put_abi(value)));
    return value;
}

template <typename D> Windows::UI::Composition::ICompositionSurface consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>::Surface() const noexcept
{
    Windows::UI::Composition::ICompositionSurface value{ nullptr };
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImage)->get_Surface(put_abi(value)));
    return value;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImage consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>::CreateImageFromUri(Windows::Foundation::Uri const& uri) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImage image{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactory)->CreateImageFromUri2(get_abi(uri), put_abi(image)));
    return image;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImage consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>::CreateImageFromUri(Windows::Foundation::Uri const& uri, Microsoft::UI::Composition::Toolkit::CompositionImageOptions const& options) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImage image{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactory)->CreateImageFromUri1(get_abi(uri), get_abi(options), put_abi(image)));
    return image;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImage consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>::CreateImageFromFile(Windows::Storage::StorageFile const& file) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImage image{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactory)->CreateImageFromFile2(get_abi(file), put_abi(image)));
    return image;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImage consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>::CreateImageFromFile(Windows::Storage::StorageFile const& file, Microsoft::UI::Composition::Toolkit::CompositionImageOptions const& options) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImage image{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactory)->CreateImageFromFile1(get_abi(file), get_abi(options), put_abi(image)));
    return image;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImage consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>::CreateImageFromPixels(array_view<uint8_t const> pixels, int32_t pixelWidth, int32_t pixelHeight) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImage image{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactory)->CreateImageFromPixels(pixels.size(), get_abi(pixels), pixelWidth, pixelHeight, put_abi(image)));
    return image;
}

template <typename D> Microsoft::UI::Composition::Toolkit::CompositionImageFactory consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactoryStatics<D>::CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor) const
{
    Microsoft::UI::Composition::Toolkit::CompositionImageFactory factory{ nullptr };
    check_hresult(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics)->CreateCompositionImageFactory(get_abi(compositor), put_abi(factory)));
    return factory;
}

template <typename D> int32_t consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions<D>::DecodeWidth() const noexcept
{
    int32_t value{};
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageOptions)->get_DecodeWidth(&value));
    return value;
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions<D>::DecodeWidth(int32_t value) const noexcept
{
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageOptions)->put_DecodeWidth(value));
}

template <typename D> int32_t consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions<D>::DecodeHeight() const noexcept
{
    int32_t value{};
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageOptions)->get_DecodeHeight(&value));
    return value;
}

template <typename D> void consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions<D>::DecodeHeight(int32_t value) const noexcept
{
    check_terminate(WINRT_SHIM(Microsoft::UI::Composition::Toolkit::ICompositionImageOptions)->put_DecodeHeight(value));
}

template <> struct delegate<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler>
{
    template <typename H>
    struct type : implements_delegate<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler, H>
    {
        type(H&& handler) : implements_delegate<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler, H>(std::forward<H>(handler)) {}

        HRESULT __stdcall Invoke(::IUnknown* sender) noexcept final
        {
            try
            {
                (*this)(*reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice const*>(&sender));
                return S_OK;
            }
            catch (...)
            {
                return to_hresult();
            }
        }
    };
};

template <> struct delegate<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler>
{
    template <typename H>
    struct type : implements_delegate<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler, H>
    {
        type(H&& handler) : implements_delegate<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler, H>(std::forward<H>(handler)) {}

        HRESULT __stdcall Invoke(::IUnknown* sender, Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus status) noexcept final
        {
            try
            {
                (*this)(*reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionImage const*>(&sender), *reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus const*>(&status));
                return S_OK;
            }
            catch (...)
            {
                return to_hresult();
            }
        }
    };
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>
{
    HRESULT __stdcall add_DeviceLost(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().DeviceLost(*reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_DeviceLost(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().DeviceLost(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CreateDrawingSurface(Windows::Foundation::Size sizePixels, Windows::Graphics::DirectX::DirectXPixelFormat pixelFormat, Windows::Graphics::DirectX::DirectXAlphaMode alphaMode, ::IUnknown** surface) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *surface = detach_abi(this->shim().CreateDrawingSurface(*reinterpret_cast<Windows::Foundation::Size const*>(&sizePixels), *reinterpret_cast<Windows::Graphics::DirectX::DirectXPixelFormat const*>(&pixelFormat), *reinterpret_cast<Windows::Graphics::DirectX::DirectXAlphaMode const*>(&alphaMode)));
            return S_OK;
        }
        catch (...)
        {
            *surface = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall AcquireDrawingLock() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().AcquireDrawingLock();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall ReleaseDrawingLock() noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().ReleaseDrawingLock();
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>
{
    HRESULT __stdcall CreateCompositionGraphicsDevice(::IUnknown* compositor, ::IUnknown** device) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *device = detach_abi(this->shim().CreateCompositionGraphicsDevice(*reinterpret_cast<Windows::UI::Composition::Compositor const*>(&compositor)));
            return S_OK;
        }
        catch (...)
        {
            *device = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionImage> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionImage>
{
    HRESULT __stdcall add_ImageLoaded(::IUnknown* handler, event_token* token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *token = detach_abi(this->shim().ImageLoaded(*reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler const*>(&handler)));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall remove_ImageLoaded(event_token token) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            this->shim().ImageLoaded(*reinterpret_cast<event_token const*>(&token));
            return S_OK;
        }
        catch (...)
        {
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall get_Size(Windows::Foundation::Size* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Size());
        return S_OK;
    }

    HRESULT __stdcall get_Surface(::IUnknown** value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().Surface());
        return S_OK;
    }
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionImageFactory> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionImageFactory>
{
    HRESULT __stdcall CreateImageFromUri2(::IUnknown* uri, ::IUnknown** image) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *image = detach_abi(this->shim().CreateImageFromUri(*reinterpret_cast<Windows::Foundation::Uri const*>(&uri)));
            return S_OK;
        }
        catch (...)
        {
            *image = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CreateImageFromUri1(::IUnknown* uri, ::IUnknown* options, ::IUnknown** image) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *image = detach_abi(this->shim().CreateImageFromUri(*reinterpret_cast<Windows::Foundation::Uri const*>(&uri), *reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionImageOptions const*>(&options)));
            return S_OK;
        }
        catch (...)
        {
            *image = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CreateImageFromFile2(::IUnknown* file, ::IUnknown** image) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *image = detach_abi(this->shim().CreateImageFromFile(*reinterpret_cast<Windows::Storage::StorageFile const*>(&file)));
            return S_OK;
        }
        catch (...)
        {
            *image = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CreateImageFromFile1(::IUnknown* file, ::IUnknown* options, ::IUnknown** image) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *image = detach_abi(this->shim().CreateImageFromFile(*reinterpret_cast<Windows::Storage::StorageFile const*>(&file), *reinterpret_cast<Microsoft::UI::Composition::Toolkit::CompositionImageOptions const*>(&options)));
            return S_OK;
        }
        catch (...)
        {
            *image = nullptr;
            return impl::to_hresult();
        }
    }

    HRESULT __stdcall CreateImageFromPixels(uint32_t __pixelsSize, uint8_t* pixels, int32_t pixelWidth, int32_t pixelHeight, ::IUnknown** image) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *image = detach_abi(this->shim().CreateImageFromPixels(array_view<uint8_t const>(reinterpret_cast<uint8_t const *>(pixels), reinterpret_cast<uint8_t const *>(pixels) + __pixelsSize), pixelWidth, pixelHeight));
            return S_OK;
        }
        catch (...)
        {
            *image = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>
{
    HRESULT __stdcall CreateCompositionImageFactory(::IUnknown* compositor, ::IUnknown** factory) noexcept final
    {
        try
        {
            typename D::abi_guard guard(this->shim());
            *factory = detach_abi(this->shim().CreateCompositionImageFactory(*reinterpret_cast<Windows::UI::Composition::Compositor const*>(&compositor)));
            return S_OK;
        }
        catch (...)
        {
            *factory = nullptr;
            return impl::to_hresult();
        }
    }
};

template <typename D>
struct produce<D, Microsoft::UI::Composition::Toolkit::ICompositionImageOptions> : produce_base<D, Microsoft::UI::Composition::Toolkit::ICompositionImageOptions>
{
    HRESULT __stdcall get_DecodeWidth(int32_t* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().DecodeWidth());
        return S_OK;
    }

    HRESULT __stdcall put_DecodeWidth(int32_t value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().DecodeWidth(value);
        return S_OK;
    }

    HRESULT __stdcall get_DecodeHeight(int32_t* value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        *value = detach_abi(this->shim().DecodeHeight());
        return S_OK;
    }

    HRESULT __stdcall put_DecodeHeight(int32_t value) noexcept final
    {
        typename D::abi_guard guard(this->shim());
        this->shim().DecodeHeight(value);
        return S_OK;
    }
};

}

WINRT_EXPORT namespace winrt::Microsoft::UI::Composition::Toolkit {

inline Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice CompositionGraphicsDevice::CreateCompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor)
{
    return get_activation_factory<CompositionGraphicsDevice, Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>().CreateCompositionGraphicsDevice(compositor);
}

inline Microsoft::UI::Composition::Toolkit::CompositionImageFactory CompositionImageFactory::CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor)
{
    return get_activation_factory<CompositionImageFactory, Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>().CreateCompositionImageFactory(compositor);
}

inline CompositionImageOptions::CompositionImageOptions() :
    CompositionImageOptions(activate_instance<CompositionImageOptions>())
{}

template <typename L> CompositionGraphicsDeviceLostEventHandler::CompositionGraphicsDeviceLostEventHandler(L handler) :
    CompositionGraphicsDeviceLostEventHandler(impl::make_delegate<CompositionGraphicsDeviceLostEventHandler>(std::forward<L>(handler)))
{}

template <typename F> CompositionGraphicsDeviceLostEventHandler::CompositionGraphicsDeviceLostEventHandler(F* handler) :
    CompositionGraphicsDeviceLostEventHandler([=](auto&& ... args) { handler(args ...); })
{}

template <typename O, typename M> CompositionGraphicsDeviceLostEventHandler::CompositionGraphicsDeviceLostEventHandler(O* object, M method) :
    CompositionGraphicsDeviceLostEventHandler([=](auto&& ... args) { ((*object).*(method))(args ...); })
{}

inline void CompositionGraphicsDeviceLostEventHandler::operator()(Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice const& sender) const
{
    check_hresult((*(abi_t<CompositionGraphicsDeviceLostEventHandler>**)this)->Invoke(get_abi(sender)));
}

template <typename L> CompositionImageLoadedEventHandler::CompositionImageLoadedEventHandler(L handler) :
    CompositionImageLoadedEventHandler(impl::make_delegate<CompositionImageLoadedEventHandler>(std::forward<L>(handler)))
{}

template <typename F> CompositionImageLoadedEventHandler::CompositionImageLoadedEventHandler(F* handler) :
    CompositionImageLoadedEventHandler([=](auto&& ... args) { handler(args ...); })
{}

template <typename O, typename M> CompositionImageLoadedEventHandler::CompositionImageLoadedEventHandler(O* object, M method) :
    CompositionImageLoadedEventHandler([=](auto&& ... args) { ((*object).*(method))(args ...); })
{}

inline void CompositionImageLoadedEventHandler::operator()(Microsoft::UI::Composition::Toolkit::CompositionImage const& sender, Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus const& status) const
{
    check_hresult((*(abi_t<CompositionImageLoadedEventHandler>**)this)->Invoke(get_abi(sender), get_abi(status)));
}

}

WINRT_EXPORT namespace std {

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImage> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImage> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageFactory> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageFactory> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageOptions> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::ICompositionImageOptions> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::CompositionImage> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::CompositionImage> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::CompositionImageFactory> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::CompositionImageFactory> {};

template<> struct hash<winrt::Microsoft::UI::Composition::Toolkit::CompositionImageOptions> : 
    winrt::impl::impl_hash_unknown<winrt::Microsoft::UI::Composition::Toolkit::CompositionImageOptions> {};

}

WINRT_WARNING_POP
