using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using X.UI.Composition;

namespace X.Engine
{

    public sealed partial class MainPage : Page
    {
        bool running = true;
        LightPanel _selectedLightPanel;

        D3D12Pipeline _pipeline;

        public MainPage()
        {
            this.InitializeComponent();

            layoutRoot.Unloaded += LayoutRoot_Unloaded;
        }

        private void LayoutRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            _pipeline = null;
        }

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //_pipeline = new D3D12Pipeline();

            //_pipeline.InitPipeline(Window.Current.CoreWindow, (int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height);

            //DoWorkAsyncInfiniteLoop(_pipeline);

            _selectedLightPanel = lpImage;
            settingsPointDiffuse.InitUI(0, 0.75f, 0);
            settingsPointDiffuse.SetLightPanel(ref _selectedLightPanel);
        }



        private async Task DoWorkAsyncInfiniteLoop(D3D12Pipeline pipeline)
        {
            while (true)
            {
                // do the work in the loop
                pipeline.Update();

                // update the UI
                pipeline.Render();
                
                // don't run again for at least 200 milliseconds
                await Task.Delay(30);
            }
        }

        private void butChangeLight_Click(Object sender, RoutedEventArgs e)
        {
            var lt = UI.Composition.LightPanel.LightingTypes.DistantDiffuse;
            var ltn = string.Empty;

            hideAllSettings();

            switch (_selectedLightPanel.SelectedLight)
            {
                case UI.Composition.LightPanel.LightingTypes.DistantDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.DistantSpecular;
                    ltn = UI.Composition.LightPanel.LightingTypes.DistantSpecular.ToString();
                    break;
                case UI.Composition.LightPanel.LightingTypes.DistantSpecular:
                    settingsPointDiffuse.Visibility = Visibility.Visible;
                    lt = UI.Composition.LightPanel.LightingTypes.PointDiffuse;
                    ltn = UI.Composition.LightPanel.LightingTypes.PointDiffuse.ToString();
                    break;
                case UI.Composition.LightPanel.LightingTypes.PointDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.PointSpecular;
                    ltn = UI.Composition.LightPanel.LightingTypes.PointSpecular.ToString();
                    break;
                case UI.Composition.LightPanel.LightingTypes.PointSpecular:
                    lt = UI.Composition.LightPanel.LightingTypes.SpotLightDiffuse;
                    ltn = UI.Composition.LightPanel.LightingTypes.SpotLightDiffuse.ToString();
                    break;
                case UI.Composition.LightPanel.LightingTypes.SpotLightDiffuse:
                    lt = UI.Composition.LightPanel.LightingTypes.SpotLightSpecular;
                    ltn = UI.Composition.LightPanel.LightingTypes.SpotLightSpecular.ToString();
                    break;
                case UI.Composition.LightPanel.LightingTypes.SpotLightSpecular:
                    lt = UI.Composition.LightPanel.LightingTypes.DistantDiffuse;
                    ltn = UI.Composition.LightPanel.LightingTypes.DistantDiffuse.ToString();
                    break;
            }

            butChangeLight.Content = ltn;
            _selectedLightPanel.SelectedLight = lt;
            _selectedLightPanel.Redraw();
        }

        private void hideAllSettings() {
            settingsPointDiffuse.Visibility = Visibility.Collapsed;
        }

        private void cbLightElements_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            var selected = (string)((ComboBoxItem)e.AddedItems[0]).Tag;

            switch (selected.ToString()) {
                case "lpImage":
                    _selectedLightPanel = lpImage;
                    break;
            }

            settingsPointDiffuse?.SetLightPanel(ref _selectedLightPanel);
        }
    }
}
