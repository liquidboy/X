namespace X.UI.NodeGraph
{
    public enum NodeType
    {
        Empty,
       
        AlphaMaskEffect = 101, // >=100 & <=1000 are effects
        ArithmeticEffect = 102,
        BlendEffect = 103,
        ColorSourceEffect = 104,
        ContrastEffect = 105,
        ExposureEffect = 106,
        GammaTransferEffect = 107,
        GrayscaleEffect = 108,
        HueRotationEffect = 109,
        InvertEffect = 110,
        SaturationEffect = 111,
        SepiaEffect = 112,
        TemperatureAndTintEffect = 113,
        Transform2DEffect = 114,
        TextureAsset = 115,

        TextboxValue = 1001, // >=1000 are controls
        SliderValue = 1002,
        ToggleValue = 1003,
        BlendEffectModeValue = 1004,
        ColorSliderValue = 1005,
        ExposureSliderValue = 1006,
        GammaTransferValue = 1007,
        CanvasAlphaModeValue = 1008,
        TemperatureValue = 1009,
        TintValue = 1010,
        BorderModeValue = 1011,
        TransformMatrixValue = 1012,
        PathScene = 1013,
        XamlFragment = 1014,

        CloudNodeType = 5000
    }
}

