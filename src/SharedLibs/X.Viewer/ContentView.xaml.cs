using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using X.Viewer.Tiles;

namespace X.Viewer
{
    
    public sealed partial class ContentView : UserControl
    {
        
        private readonly WeakEventSource<ContentViewEventArgs> _SendMessageSource = new WeakEventSource<ContentViewEventArgs>();
        public event EventHandler<ContentViewEventArgs> SendMessage
        {
            add { _SendMessageSource.Subscribe(value); }
            remove { _SendMessageSource.Unsubscribe(value); }
        }




        public IContentRenderer Renderer;

        public ContentView()
        {
            this.InitializeComponent();

            layoutRoot.DataContext = this;

         
            

        }

   

        public string Uri
        {
            get { return (string)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(string), typeof(ContentView), new PropertyMetadata(string.Empty, UriChanged));

        private static void UriChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (!e.NewValue.Equals(e.OldValue)) {
                ContentView cv = (ContentView)d;

                if (cv.Renderer != null ) {
               
                    cv.Renderer.Unload();
                    cv.layoutRoot.Children.Remove(cv.Renderer.RenderElement);
                    cv.layoutRoot.Children.Clear();
                    cv.Renderer.RenderElement = null;
                    cv.Renderer = null;
                }

                if (string.IsNullOrEmpty((string)e.NewValue)) return;

                var uriNew = (string)e.NewValue;


                if (uriNew.Contains(".mp4") || uriNew.Contains(".mpeg") || uriNew.Contains(".avi") || uriNew.Contains(".webm") || uriNew.Contains(".ogv") || uriNew.Contains(".3gp") || uriNew.Contains(".mkv"))
                {
                    cv.Renderer = new FFmpegRenderer();

                    //todo : need to find a way to light bind this so as to unbind when unloaded
                    cv.Renderer.SendMessage += (s, ea) => { cv._SendMessageSource.Raise(s, ea); };
                    cv.Renderer.Load();
                    cv.layoutRoot.Children.Add(cv.Renderer.RenderElement);

                    //cv.Renderer.UpdateSource(new Uri("ms-appx:///Assets/Videos/sample01.mp4"));
                    cv.Renderer.UpdateSource(uriNew);
                }
                else if (uriNew.Contains(".tile")){
                    cv.Renderer = new TileRenderer();
                    cv.Renderer.SendMessage += (s, ea) => { cv._SendMessageSource.Raise(s, ea); };
                    cv.Renderer.Load();
                    cv.layoutRoot.Children.Add(cv.Renderer.RenderElement);

                    cv.Renderer.UpdateSource((string)e.NewValue);
                }
                else
                {
                    cv.Renderer = new WebViewRenderer();

                    //todo : need to find a way to light bind this so as to unbind when unloaded
                    cv.Renderer.SendMessage += (s, ea) => { cv._SendMessageSource.Raise(s, ea); };
                    cv.Renderer.Load();
                    cv.layoutRoot.Children.Add(cv.Renderer.RenderElement);

                    cv.Renderer.UpdateSource((string)e.NewValue);

                }
                    


            }
            
        }

    }



}
