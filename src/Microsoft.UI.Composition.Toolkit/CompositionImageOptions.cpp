#include "pch.h"
#include "CompositionImageOptions.h"

namespace winrt::Microsoft::UI::Composition::Toolkit::implementation
{
    int32_t CompositionImageOptions::DecodeWidth()
    {
        return m_decodeWidth;
    }

    void CompositionImageOptions::DecodeWidth(int32_t value)
    {
        if (value < 0)
        {
            throw hresult_invalid_argument(L"The DecodeWidth property of CompositionImageOptions cannot be less than 0.");
        }

        m_decodeWidth = value;
    }

    int32_t CompositionImageOptions::DecodeHeight()
    {
        return m_decodeHeight;
    }

    void CompositionImageOptions::DecodeHeight(int32_t value)
    {
        if (value < 0)
        {
            throw hresult_invalid_argument(L"The DecodeHeight property of CompositionImageOptions cannot be less than 0.");
        }

        m_decodeHeight = value;
    }
}
