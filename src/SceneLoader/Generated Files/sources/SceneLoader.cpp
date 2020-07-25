#include "pch.h"
#include "SceneLoader.h"

namespace winrt::SceneLoaderComponent::implementation
{
    Windows::UI::Composition::Scenes::SceneNode SceneLoader::Load(Windows::Storage::Streams::IBuffer const& buffer, Windows::UI::Composition::Compositor const& compositor)
    {
        throw hresult_not_implemented();
    }
}
