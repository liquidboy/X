﻿// WARNING: Please don't edit this file. It was generated by C++/WinRT v1.0.190111.3

#pragma once

#include "winrt/Windows.Storage.Streams.h"
#include "winrt/Windows.UI.Composition.h"
#include "winrt/Windows.UI.Composition.Scenes.h"
#include "winrt/SceneLoaderComponent.h"

namespace winrt::SceneLoaderComponent::implementation {

template <typename D, typename... I>
struct WINRT_EBO SceneLoader_base : implements<D, SceneLoaderComponent::ISceneLoader, I...>
{
    using base_type = SceneLoader_base;
    using class_type = SceneLoaderComponent::SceneLoader;
    using implements_type = typename SceneLoader_base::implements_type;
    using implements_type::implements_type;
    
#if _MSC_VER < 1914
    operator class_type() const noexcept
    {
        static_assert(std::is_same_v<typename impl::implements_default_interface<D>::type, default_interface<class_type>>);
        class_type result{ nullptr };
        attach_abi(result, detach_abi(static_cast<default_interface<class_type>>(*this)));
        return result;
    }
#else
    operator impl::producer_ref<class_type> const() const noexcept
    {
        return { to_abi<default_interface<class_type>>(this) };
    }
#endif

    hstring GetRuntimeClassName() const
    {
        return L"SceneLoaderComponent.SceneLoader";
    }
};

}

namespace winrt::SceneLoaderComponent::factory_implementation {

template <typename D, typename T, typename... I>
struct WINRT_EBO SceneLoaderT : implements<D, Windows::Foundation::IActivationFactory, I...>
{
    using instance_type = SceneLoaderComponent::SceneLoader;

    hstring GetRuntimeClassName() const
    {
        return L"SceneLoaderComponent.SceneLoader";
    }

    Windows::Foundation::IInspectable ActivateInstance() const
    {
        return make<T>();
    }
};

}

#if defined(WINRT_FORCE_INCLUDE_SCENELOADER_XAML_G_H) || __has_include("SceneLoader.xaml.g.h")

#include "SceneLoader.xaml.g.h"

#else

namespace winrt::SceneLoaderComponent::implementation
{
    template <typename D, typename... I>
    using SceneLoaderT = SceneLoader_base<D, I...>;
}

#endif
