using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SumoNinjaMonkey.Services.Controls
{
  public static class UserControlExtensions
  {
    public static void FindChildren<T>(this UserControl userControl,  List<T> results, DependencyObject startNode) where T : DependencyObject
    {
      int count = VisualTreeHelper.GetChildrenCount(startNode);
      for (int i = 0; i < count; i++)
      {
        DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
        if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
        {
          T asType = (T)current;
          results.Add(asType);
        }
        userControl.FindChildren<T>(results, current);
      }
    }
  }
}
