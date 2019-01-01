using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public enum NodeType
    {
        Empty,
       
        AlphaMaskEffect = 100, // >=100 & <=1000 are effects
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
        ImageEffect,

        TextboxValue = 1000, // >=1000 are controls
    }

    public interface INodeGraphCompositor
    {
        void InitializeCompositor(UIElement rootVisualElement);
        void CreateNodeVisual(string nodeKey, UIElement parentRootOfVisual, NodeType effectType);
        void ClearCompositor();
    }
}
