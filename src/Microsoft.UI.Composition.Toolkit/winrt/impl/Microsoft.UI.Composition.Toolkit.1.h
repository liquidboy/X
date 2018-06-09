// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once
#include "winrt/impl/Windows.Foundation.0.h"
#include "winrt/impl/Windows.Graphics.DirectX.0.h"
#include "winrt/impl/Windows.Storage.0.h"
#include "winrt/impl/Windows.UI.Composition.0.h"
#include "winrt/impl/Microsoft.UI.Composition.Toolkit.0.h"

WINRT_EXPORT namespace winrt::Microsoft::UI::Composition::Toolkit {

struct WINRT_EBO ICompositionGraphicsDevice :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionGraphicsDevice>
{
    ICompositionGraphicsDevice(std::nullptr_t = nullptr) noexcept {}
};

struct WINRT_EBO ICompositionGraphicsDeviceStatics :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionGraphicsDeviceStatics>
{
    ICompositionGraphicsDeviceStatics(std::nullptr_t = nullptr) noexcept {}
};

struct WINRT_EBO ICompositionImage :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionImage>
{
    ICompositionImage(std::nullptr_t = nullptr) noexcept {}
};

struct WINRT_EBO ICompositionImageFactory :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionImageFactory>
{
    ICompositionImageFactory(std::nullptr_t = nullptr) noexcept {}
};

struct WINRT_EBO ICompositionImageFactoryStatics :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionImageFactoryStatics>
{
    ICompositionImageFactoryStatics(std::nullptr_t = nullptr) noexcept {}
};

struct WINRT_EBO ICompositionImageOptions :
    Windows::Foundation::IInspectable,
    impl::consume_t<ICompositionImageOptions>
{
    ICompositionImageOptions(std::nullptr_t = nullptr) noexcept {}
};

}
