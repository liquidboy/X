#pragma once

#include "CompositionImageOptions.g.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    struct CompositionImageOptions : CompositionImageOptionsT<CompositionImageOptions>
    {
        CompositionImageOptions() = default;

        int32_t DecodeWidth();
        void DecodeWidth(int32_t value);
        int32_t DecodeHeight();
        void DecodeHeight(int32_t value);

    private:

        int m_decodeWidth{};
        int m_decodeHeight{};
    };
}

namespace winrt::Microsoft::UI::Composition::Toolkit::factory_implementation
{
    struct CompositionImageOptions : CompositionImageOptionsT<CompositionImageOptions, implementation::CompositionImageOptions>
    {
    };
}
