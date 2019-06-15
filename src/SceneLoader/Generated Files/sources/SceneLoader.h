#pragma once
#include "SceneLoader.g.h"

namespace winrt::SceneLoaderComponent::implementation
{
    struct SceneLoader : SceneLoaderT<SceneLoader>
    {
        SceneLoader() = default;

        Windows::UI::Composition::Scenes::SceneNode Load(Windows::Storage::Streams::IBuffer const& buffer, Windows::UI::Composition::Compositor const& compositor);
    };
}
namespace winrt::SceneLoaderComponent::factory_implementation
{
    struct SceneLoader : SceneLoaderT<SceneLoader, implementation::SceneLoader>
    {
    };
}
