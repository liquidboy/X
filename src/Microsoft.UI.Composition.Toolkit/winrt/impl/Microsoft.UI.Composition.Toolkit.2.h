// C++/WinRT v1.0.170906.1
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.

#pragma once
#include "winrt/impl/Windows.Foundation.1.h"
#include "winrt/impl/Windows.Graphics.DirectX.1.h"
#include "winrt/impl/Windows.Storage.1.h"
#include "winrt/impl/Windows.UI.Composition.1.h"
#include "winrt/impl/Microsoft.UI.Composition.Toolkit.1.h"

WINRT_EXPORT namespace winrt::Microsoft::UI::Composition::Toolkit {

struct CompositionGraphicsDeviceLostEventHandler : Windows::Foundation::IUnknown
{
    CompositionGraphicsDeviceLostEventHandler(std::nullptr_t = nullptr) noexcept {}
    template <typename L> CompositionGraphicsDeviceLostEventHandler(L lambda);
    template <typename F> CompositionGraphicsDeviceLostEventHandler(F* function);
    template <typename O, typename M> CompositionGraphicsDeviceLostEventHandler(O* object, M method);
    void operator()(Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice const& sender) const;
};

struct CompositionImageLoadedEventHandler : Windows::Foundation::IUnknown
{
    CompositionImageLoadedEventHandler(std::nullptr_t = nullptr) noexcept {}
    template <typename L> CompositionImageLoadedEventHandler(L lambda);
    template <typename F> CompositionImageLoadedEventHandler(F* function);
    template <typename O, typename M> CompositionImageLoadedEventHandler(O* object, M method);
    void operator()(Microsoft::UI::Composition::Toolkit::CompositionImage const& sender, Microsoft::UI::Composition::Toolkit::CompositionImageLoadStatus const& status) const;
};

}

namespace winrt::impl {

}

WINRT_EXPORT namespace winrt::Microsoft::UI::Composition::Toolkit {

struct WINRT_EBO CompositionGraphicsDevice :
    Microsoft::UI::Composition::Toolkit::ICompositionGraphicsDevice,
    impl::require<CompositionGraphicsDevice, Windows::Foundation::IClosable>
{
    CompositionGraphicsDevice(std::nullptr_t) noexcept {}
    static Microsoft::UI::Composition::Toolkit::CompositionGraphicsDevice CreateCompositionGraphicsDevice(Windows::UI::Composition::Compositor const& compositor);
};

struct WINRT_EBO CompositionImage :
    Microsoft::UI::Composition::Toolkit::ICompositionImage
{
    CompositionImage(std::nullptr_t) noexcept {}
};

struct WINRT_EBO CompositionImageFactory :
    Microsoft::UI::Composition::Toolkit::ICompositionImageFactory
{
    CompositionImageFactory(std::nullptr_t) noexcept {}
    static Microsoft::UI::Composition::Toolkit::CompositionImageFactory CreateCompositionImageFactory(Windows::UI::Composition::Compositor const& compositor);
};

struct WINRT_EBO CompositionImageOptions :
    Microsoft::UI::Composition::Toolkit::ICompositionImageOptions
{
    CompositionImageOptions(std::nullptr_t) noexcept {}
    CompositionImageOptions();
};

}
