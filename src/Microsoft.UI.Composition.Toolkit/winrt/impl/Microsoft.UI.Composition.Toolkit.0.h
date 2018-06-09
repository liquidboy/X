// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once

WINRT_EXPORT namespace winrt::Windows::Foundation {

struct Uri;

}

WINRT_EXPORT namespace winrt::Windows::Graphics::DirectX {

enum class DirectXAlphaMode;
enum class DirectXPixelFormat;

}

WINRT_EXPORT namespace winrt::Windows::Storage {

struct StorageFile;

}

WINRT_EXPORT namespace winrt::Windows::UI::Composition {

struct Compositor;
struct ICompositionSurface;

}

WINRT_EXPORT namespace winrt::Microsoft::UI::Composition::Toolkit {

enum class CompositionImageLoadStatus : int32_t
{
    Success = 0,
    FileAccessError = 1,
    DecodeError = 2,
    NotEnoughResources = 3,
    Other = 4,
};

struct ICompositionGraphicsDevice;
struct ICompositionGraphicsDeviceStatics;
struct ICompositionImage;
struct ICompositionImageFactory;
struct ICompositionImageFactoryStatics;
struct ICompositionImageOptions;
struct CompositionGraphicsDevice;
struct CompositionImage;
struct CompositionImageFactory;
struct CompositionImageOptions;
struct CompositionGraphicsDeviceLostEventHandler;
struct CompositionImageLoadedEventHandler;

}

namespace winrt::impl {

template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionImage>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionImageFactory>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::ICompositionImageOptions>{ using type = interface_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice>{ using type = class_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionImage>{ using type = class_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionImageFactory>{ using type = class_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionImageOptions>{ using type = class_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus>{ using type = enum_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler>{ using type = delegate_category; };
template <> struct category<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler>{ using type = delegate_category; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDevice" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionGraphicsDeviceStatics" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionImage>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionImage" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionImageFactory>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactory" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionImageFactoryStatics" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::ICompositionImageOptions>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.ICompositionImageOptions" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionGraphicsDevice" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionImage>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionImage" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionImageFactory>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionImageFactory" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionImageOptions>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionImageOptions" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionImageLoadStatus" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionGraphicsDeviceLostEventHandler" }; };
template <> struct name<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler>{ static constexpr auto & value{ L"Microsoft.UI.Composition.Toolkit.CompositionImageLoadedEventHandler" }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>{ static constexpr GUID value{ 0x4A0E578E,0x68FA,0x3394,{ 0x90,0x5A,0xD4,0xD9,0xD8,0xAE,0x30,0xCA } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>{ static constexpr GUID value{ 0xBC22813B,0xC736,0x3176,{ 0x9D,0x45,0x75,0xF8,0x6B,0xEA,0xE8,0xB6 } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionImage>{ static constexpr GUID value{ 0xD5E4B2E9,0x82F7,0x3A11,{ 0x8D,0x7A,0xE5,0x8B,0x89,0x2E,0x05,0x4C } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionImageFactory>{ static constexpr GUID value{ 0x25372FEC,0x3FA1,0x3C03,{ 0x98,0x0F,0xFC,0x48,0xBA,0x44,0x68,0x0F } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>{ static constexpr GUID value{ 0xEB167AED,0x6BF0,0x39AE,{ 0x90,0xB1,0x7C,0x05,0xFF,0x0F,0x30,0x8F } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::ICompositionImageOptions>{ static constexpr GUID value{ 0x8554C61C,0xF17B,0x3602,{ 0x86,0x8B,0xC1,0x7C,0x9F,0x7B,0xC4,0x49 } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler>{ static constexpr GUID value{ 0x0BE20CB6,0xFE18,0x3023,{ 0x96,0x80,0xF4,0xAA,0x21,0x08,0xBC,0xEE } }; };
template <> struct guid<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler>{ static constexpr GUID value{ 0x5B3AA813,0xCEBF,0x38B9,{ 0x8F,0x31,0xD2,0x3E,0xB8,0xC2,0x4F,0x86 } }; };
template <> struct default_interface<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice>{ using type = Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice; };
template <> struct default_interface<Microsoft::UI::Composition::Toolkit::CompositionImage>{ using type = Microsoft::UI::Composition::Toolkit::ICompositionImage; };
template <> struct default_interface<Microsoft::UI::Composition::Toolkit::CompositionImageFactory>{ using type = Microsoft::UI::Composition::Toolkit::ICompositionImageFactory; };
template <> struct default_interface<Microsoft::UI::Composition::Toolkit::CompositionImageOptions>{ using type = Microsoft::UI::Composition::Toolkit::ICompositionImageOptions; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice
{
    event_token DeviceLost(Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler const& handler) const;
    using DeviceLost_revoker = event_revoker<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>;
    DeviceLost_revoker DeviceLost(auto_revoke_t, Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler const& handler) const;
    void DeviceLost(event_token const& token) const;
    Windows::UI::Composition::ICompositionSurface CreateDrawingSurface(Windows::Foundation::Size const& sizePixels, Windows::Graphics::DirectX::DirectXPixelFormat const& pixelFormat, Windows::Graphics::DirectX::DirectXAlphaMode const& alphaMode) const;
    void AcquireDrawingLock() const;
    void ReleaseDrawingLock() const;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDevice<D>; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDeviceStatics
{
    Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice CreateCompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor) const;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionGraphicsDeviceStatics<D>; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionImage
{
    event_token ImageLoaded(Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler const& handler) const;
    using ImageLoaded_revoker = event_revoker<Microsoft::UI::Composition::Toolkit::ICompositionImage>;
    ImageLoaded_revoker ImageLoaded(auto_revoke_t, Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler const& handler) const;
    void ImageLoaded(event_token const& token) const;
    Windows::Foundation::Size Size() const noexcept;
    Windows::UI::Composition::ICompositionSurface Surface() const noexcept;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionImage> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionImage<D>; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory
{
    Microsoft::UI::Composition::Toolkit::CompositionImage CreateImageFromUri(Windows::Foundation::Uri const& uri) const;
    Microsoft::UI::Composition::Toolkit::CompositionImage CreateImageFromUri(Windows::Foundation::Uri const& uri, Microsoft::UI::Composition::Toolkit::CompositionImageOptions const& options) const;
    Microsoft::UI::Composition::Toolkit::CompositionImage CreateImageFromFile(Windows::Storage::StorageFile const& file) const;
    Microsoft::UI::Composition::Toolkit::CompositionImage CreateImageFromFile(Windows::Storage::StorageFile const& file, Microsoft::UI::Composition::Toolkit::CompositionImageOptions const& options) const;
    Microsoft::UI::Composition::Toolkit::CompositionImage CreateImageFromPixels(array_view<uint8_t const> pixels, int32_t pixelWidth, int32_t pixelHeight) const;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionImageFactory> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactory<D>; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactoryStatics
{
    Microsoft::UI::Composition::Toolkit::CompositionImageFactory CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor) const;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionImageFactoryStatics<D>; };

template <typename D>
struct consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions
{
    int32_t DecodeWidth() const noexcept;
    void DecodeWidth(int32_t value) const noexcept;
    int32_t DecodeHeight() const noexcept;
    void DecodeHeight(int32_t value) const noexcept;
};
template <> struct consume<Microsoft::UI::Composition::Toolkit::ICompositionImageOptions> { template <typename D> using type = consume_Microsoft_UI_Composition_Toolkit_ICompositionImageOptions<D>; };

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall add_DeviceLost(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_DeviceLost(event_token token) = 0;
    virtual HRESULT __stdcall CreateDrawingSurface(Windows::Foundation::Size sizePixels, Windows::Graphics::DirectX::DirectXPixelFormat pixelFormat, Windows::Graphics::DirectX::DirectXAlphaMode alphaMode, ::IUnknown** surface) = 0;
    virtual HRESULT __stdcall AcquireDrawingLock() = 0;
    virtual HRESULT __stdcall ReleaseDrawingLock() = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDeviceStatics>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall CreateCompositionGraphicsDevice(::IUnknown* compositor, ::IUnknown** device) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionImage>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall add_ImageLoaded(::IUnknown* handler, event_token* token) = 0;
    virtual HRESULT __stdcall remove_ImageLoaded(event_token token) = 0;
    virtual HRESULT __stdcall get_Size(Windows::Foundation::Size* value) = 0;
    virtual HRESULT __stdcall get_Surface(::IUnknown** value) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionImageFactory>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall CreateImageFromUri2(::IUnknown* uri, ::IUnknown** image) = 0;
    virtual HRESULT __stdcall CreateImageFromUri1(::IUnknown* uri, ::IUnknown* options, ::IUnknown** image) = 0;
    virtual HRESULT __stdcall CreateImageFromFile2(::IUnknown* file, ::IUnknown** image) = 0;
    virtual HRESULT __stdcall CreateImageFromFile1(::IUnknown* file, ::IUnknown* options, ::IUnknown** image) = 0;
    virtual HRESULT __stdcall CreateImageFromPixels(uint32_t __pixelsSize, uint8_t* pixels, int32_t pixelWidth, int32_t pixelHeight, ::IUnknown** image) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionImageFactoryStatics>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall CreateCompositionImageFactory(::IUnknown* compositor, ::IUnknown** factory) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::ICompositionImageOptions>{ struct type : ::IInspectable
{
    virtual HRESULT __stdcall get_DecodeWidth(int32_t* value) = 0;
    virtual HRESULT __stdcall put_DecodeWidth(int32_t value) = 0;
    virtual HRESULT __stdcall get_DecodeHeight(int32_t* value) = 0;
    virtual HRESULT __stdcall put_DecodeHeight(int32_t value) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::CompositionGraphicsDeviceLostEventHandler>{ struct type : ::IUnknown
{
    virtual HRESULT __stdcall Invoke(::IUnknown* sender) = 0;
};};

template <> struct abi<Microsoft::UI::Composition::Toolkit::CompositionImageLoadedEventHandler>{ struct type : ::IUnknown
{
    virtual HRESULT __stdcall Invoke(::IUnknown* sender, Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus status) = 0;
};};

}
