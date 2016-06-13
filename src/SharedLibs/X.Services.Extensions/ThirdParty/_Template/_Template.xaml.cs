using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using WeakEvent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.Extensions;

namespace X.Services.ThirdParty
{
    public sealed partial class _Template : UserControl, IExtension
    {
        private readonly WeakEventSource<EventArgs> _SendMessageSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }

        public IExtensionManifest ExtensionManifest { get; set; }
        public ExtensionType ExtensionType { get; set; } = ExtensionType.UIComponent;
        public string Path { get; set; }
        private IExtensionContent _content;


        public _Template(IExtensionManifest extensionManifest)
        {
            this.InitializeComponent();

            ExtensionManifest = extensionManifest;

            layoutRoot.DataContext = this;
        }
        

        public void RecieveMessage(object message)
        {
            _content?.RecieveMessage(message);
        }

        public async void OnPaneLoad()
        {
            if (!string.IsNullOrEmpty(ExtensionManifest.ContentControl)) {

                if (!ExtensionManifest.IsUWPExtension) {
                    Type type = null;
                    if (!string.IsNullOrEmpty(ExtensionManifest.AssemblyName))
                    {
                        var an = new System.Reflection.AssemblyName(ExtensionManifest.AssemblyName);
                        var ass = System.Reflection.Assembly.Load(an);
                        type = ass.GetType(ExtensionManifest.ContentControl);
                    }
                    else type = Type.GetType(ExtensionManifest.ContentControl);


                    var newEl = (UserControl)Activator.CreateInstance(type);

                    if (newEl is IExtensionContent)
                    {
                        _content = (IExtensionContent)newEl;
                        _content.SendMessage += _content_SendMessage;
                    }

                    ctlContent.Children.Add(newEl);
                }
                else {

                    var ef = Extensions.ExtensionsService.Instance.GetExtensionByAppExtensionUniqueId(ExtensionManifest.AppExtensionUniqueID);
                    var result = await ef.MakeUWPCommandCall("UI", "Call");

                    if (result != null) {
                        //var newEl = new StackPanel() { Orientation = Orientation.Vertical };
                        var newEl = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
                        //foreach (var val in result)
                        //{
                        //    newEl.Children.Add(new TextBlock() { Text = $"{val.Key}  - {val.Value}" });
                        //}
                        if (result.ContainsKey("default"))
                        {
                            
                            var keyValueForDefault = result.Where(x => x.Key == "default").First();
                            var defaultPage = result.Where(x => x.Key == (string)keyValueForDefault.Value).First();

                            var packageDirectory = ef.AppExtension.Package.InstalledLocation;
                            var publicDirectory = await packageDirectory.GetFolderAsync("public");

                            var defaultPageXaml = await publicDirectory.GetFileAsync(defaultPage.Value.ToString());
                            using (var stream = await defaultPageXaml.OpenStreamForReadAsync())
                            {
                                var tr = new StreamReader(stream);
                                var xaml = await tr.ReadToEndAsync();

                                if (xaml.Length > 0)
                                {
                                    var xamlFe = (FrameworkElement)XamlReader.Load(UnescapeString(xaml));
                                    newEl.Children.Add(xamlFe);
                                }
                            }

                            
                        }else
                        {

                        }
                        ctlContent.Children.Add(newEl);
                    }
                    
                }
                
                switch (ExtensionManifest.LaunchInDockPositions)
                {
                    case ExtensionInToolbarPositions.Left: brMain.BorderThickness = new Thickness(0,0,0.5,0); brMain.Margin = new Thickness(0, 3, 0, 3); break;
                    case ExtensionInToolbarPositions.Right: brMain.BorderThickness = new Thickness(0.5,0,0,0); brMain.Margin = new Thickness(0, 3, 0, 3); break;
                    case ExtensionInToolbarPositions.Bottom: brMain.BorderThickness = new Thickness(0, 0.5, 0, 0); brMain.Margin = new Thickness(3, 0, 3, 0); break;
                    case ExtensionInToolbarPositions.BottomFull: brMain.BorderThickness = new Thickness(0, 0.5, 0, 0); brMain.Margin = new Thickness(3, 0, 3, 0); break;
                    case ExtensionInToolbarPositions.Top: brMain.BorderThickness = new Thickness(0, 0, 0, 0.5); brMain.Margin = new Thickness(3, 0, 3, 0); break;
                    default: brMain.BorderThickness = new Thickness(0); brMain.Margin = new Thickness(3, 0, 3, 0); break;
                }
                
            }

        }

        private string UnescapeString(string escapedString)
        {
            var output = Regex.Replace(escapedString, @"\\[rnt]", m =>
            {
                switch (m.Value)
                {
                    case @"\r": return "\r";
                    case @"\n": return "\n";
                    case @"\t": return "\t";
                    default: return m.Value;
                }
            });
            return output;
        }

        public void OnPaneUnload()
        {
            if (_content != null) {
                _content.SendMessage -= _content_SendMessage;
                _content.Unload();
                ctlContent.Children.RemoveAt(0);
                _content = null;
            }
        }

        private void _content_SendMessage(object sender, EventArgs e)
        {
            _SendMessageSource?.Raise(this, e);
        }

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void butClose_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            _SendMessageSource?.Raise(this, new CloseExtensionEventArgs() { ReceiverType = ExtensionType.UIComponent, ExtensionUniqueGuid = ExtensionManifest.UniqueID });
        }
    }
}
