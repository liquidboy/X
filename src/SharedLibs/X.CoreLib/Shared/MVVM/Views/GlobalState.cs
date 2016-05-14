
using X.CoreLib.Shared.Services;
namespace FavouriteMX.Shared.Views
{
    public class GlobalState
    {
        public Solution SelectedSolution { get; set; }
        public Project SelectedProject { get; set; }
        public Scene SelectedScene { get; set; }
        public UIElementState SelectedUIElement { get; set; }
        public double DrawingSurfaceWidth { get; set; }
        public double DrawingSurfaceHeight { get; set; }
        public bool IsSharpDxRendering { get; set; }

        public string DefaultBackgroundUri { get; set; }
        public string DefaultBackgroundFolder { get; set; }

    }
}
