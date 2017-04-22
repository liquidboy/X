
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
namespace SumoNinjaMonkey.Framework.Controls
{
    public class SeeMoreItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate VirtualItemTemplate
        {
            get;
            set;
        }
        public DataTemplate ContentItemTemplate
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            FrameworkElement frameworkElement = container as FrameworkElement;
            if (frameworkElement == null || item == null || !(item is INavigableItem))
            {
                return null;
            }
            INavigableItem navigableItem = item as INavigableItem;
            if (navigableItem.IsVirtualItem)
            {
                return this.VirtualItemTemplate;
            }
            return this.ContentItemTemplate;
        }

    }
}
