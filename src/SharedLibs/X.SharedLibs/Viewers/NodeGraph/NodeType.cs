using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public enum NodeType
    {
        Empty,
       
        AlphaMaskEffect = 101, // >=100 & <=1000 are effects
        ArithmeticEffect,
        BlendEffect,
        ColorSourceEffect,
        ContrastEffect,
        ExposureEffect,
        GammaTransferEffect,
        GrayscaleEffect,
        HueRotationEffect,
        InvertEffect,
        SaturationEffect,
        SepiaEffect,
        TemperatureAndTintEffect,
        Transform2DEffect,
        TextureAsset,

        TextboxValue = 1001, // >=1000 are controls
        SliderValue,
        ToggleValue,
        BlendEffectModeValue,
        ColorSliderValue,
        ExposureSliderValue

    }
}

