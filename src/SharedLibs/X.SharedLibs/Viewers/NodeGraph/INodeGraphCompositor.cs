using Windows.UI.Xaml;

namespace X.Viewer.NodeGraph
{
    public enum EffectType
    {
        NoEffect,
        AlphaMask,
        Arithmetic,
        Blend,
        ColorSource,
        Contrast,
        Exposure,
        GammaTransfer,
        Grayscale,
        HueRotation,
        Invert,
        Saturation,
        Sepia,
        TemperatureAndTint,
        Transform2D,

        NumEffectTypes
    }

    public interface INodeGraphCompositor
    {
        void InitializeCompositor(UIElement rootVisualElement);
        void CreateNodeVisual(string nodeKey, UIElement parentRootOfVisual, EffectType effectType);
    }
}
