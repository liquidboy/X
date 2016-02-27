

using System.Collections.ObjectModel;

using Windows.UI.Xaml.Shapes;
namespace SumoNinjaMonkey.Framework.Controls
{
    public class IconItem 
    {
        public string DisplayTitle { get; set; }
        public Path IconVector { get; set; }
        public ObservableCollection<IconItem> IconItems { get; set; }

        
    }
}
