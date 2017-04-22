

using Windows.UI.Xaml.Media;
namespace SumoNinjaMonkey.Framework.Controls
{
    public class RibbonTabItem
    {
        public string DisplayTitle { get; set; }
        public int ID { get; set; }
        public SolidColorBrush SelectedBackgroundColor { get; set; }
        public SolidColorBrush SelectedForegroundColor { get; set; }
        public SolidColorBrush NormalForegroundColor { get; set; }
    }
}
