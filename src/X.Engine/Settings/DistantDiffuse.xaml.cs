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
using X.UI.Composition;

namespace X.Engine.Settings
{
    public sealed partial class DistantDiffuse : UserControl
    {
        private LightPanel _lp;
        public DistantDiffuse()
        {
            this.InitializeComponent();
        }

        public void InitUI(float AmbientAmount, float DiffuseAmount, float SpecularAmount) {
            sldAmbientAmount.Value = AmbientAmount;
            sldDiffuseAmount.Value = DiffuseAmount;
            sldSpecularAmount.Value = SpecularAmount;
        }

        public void SetLightPanel(ref LightPanel lp) {
            _lp = lp;
        }

        private void updateData() {
            _lp?.UpdateLightingEffect_DistantDiffuse((float)sldAmbientAmount.Value, (float)sldDiffuseAmount.Value, (float)sldSpecularAmount.Value, forceUpdate:true);
        }

        private void sldDiffuseAmount_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldAmbientAmount_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldSpecularAmount_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }
    }
}
