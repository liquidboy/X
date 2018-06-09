using System;
using System.Collections.Generic;
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

namespace X.Desktop
{

  public sealed partial class HomePage : Page
  {
    public HomePage()
    {
      this.InitializeComponent();

      layoutRoot.Children.Add(new WebView(executionMode: WebViewExecutionMode.SeparateProcess) { Source = new Uri("http://www.microsoft.com") });
    }
  }
}
