using System;
using Windows.UI.Xaml.Controls;


namespace X.ModernDesktop
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
