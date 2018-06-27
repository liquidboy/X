using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace X.Viewer.SketchFlow.Controls.Pickers
{
  public sealed partial class EntityLinePicker : UserControl
  {
    public event EventHandler LineChanged;

    public ObservableCollection<String> AllowedListOfEntities { get; set; } = new ObservableCollection<string>();

    public EntityLinePicker()
    {
      this.InitializeComponent();
    }


    private void rcb1_ValueChanged(object sender, RoutedEventArgs e)
    {
      //TextChanged?.Invoke(sender, new TextPickerEventArgs() { Text = (string)tbMain.Text, FontFamily = (string)rcb.Value2 });
    }

    private void rcb2_ValueChanged(object sender, RoutedEventArgs e)
    {
      //TextChanged?.Invoke(sender, new TextPickerEventArgs() { Text = (string)tbMain.Text, FontFamily = (string)rcb.Value2 });
    }
  }

  public class EntityLinePickerEventArgs : EventArgs
  {
    public string Line1;
    public string Line2;
  }
}
