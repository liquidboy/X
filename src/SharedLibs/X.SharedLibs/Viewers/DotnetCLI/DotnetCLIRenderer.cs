﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace X.Viewer.DotnetCLI
{
    public class DotnetCLIRenderer : IContentRenderer
    {
        FrameworkElement _renderElement;

        public event EventHandler<ContentViewEventArgs> SendMessage;
        
        public FrameworkElement RenderElement
        {
            get
            {
                return _renderElement;
            }

            set
            {
                _renderElement = value;
            }
        }

        public string Uri { get; set; }

        //public event EventHandler<ContentViewEventArgs> SendMessage;

        public async Task CaptureThumbnail(InMemoryRandomAccessStream ms)
        {

        }

        public void Load()
        {
            _renderElement = new CommandLine();
        }

        public void Unload()
        {
            throw new NotImplementedException();
        }

        public void UpdateSource(string uri)
        {
            
        }

        public void SendMessageThru(object source, ContentViewEventArgs ea)
        {
            throw new NotImplementedException();
        }
    }
}
