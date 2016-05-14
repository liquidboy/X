using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using WeakEvent;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
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

                    var ef = Extensions.ExtensionsFullService.Instance.Extensions[0];
                    var result = await ef.MakeCommandCall("UI", "Call");


                    var newEl = new StackPanel() { Orientation = Orientation.Vertical};
                    foreach (var val in result)
                    {
                        newEl.Children.Add(new TextBlock() { Text = $"{val.Key}  - {val.Value}" });
                    }
                    ctlContent.Children.Add(newEl);
                }
                    
      
                    


                
               
            }

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
