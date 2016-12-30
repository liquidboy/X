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
    public sealed partial class PointSpecular : UserControl
    {
        private LightPanel _lp;
        public PointSpecular()
        {
            this.InitializeComponent();
        }

        public void InitUI(float AmbientAmount1, float DiffuseAmount1, float SpecularAmount1, float AmbientAmount2, float DiffuseAmount2, float SpecularAmount2, float SpecularShine2) {
            sldAmbientAmount.Value = AmbientAmount1;
            sldDiffuseAmount.Value = DiffuseAmount1;
            sldSpecularAmount.Value = SpecularAmount1;

            sldAmbientAmount2.Value = AmbientAmount2;
            sldDiffuseAmount2.Value = DiffuseAmount2;
            sldSpecularAmount2.Value = SpecularAmount2;
            sldSpecularShine2.Value = SpecularShine2;
        }

        public void SetLightPanel(ref LightPanel lp) {
            _lp = lp;
        }

        private void updateData() {
            _lp?.UpdateLightingEffect_PointSpecular(
                AmbientAmount1:(float)sldAmbientAmount.Value, DiffuseAmount1:(float)sldDiffuseAmount.Value, SpecularAmount1: (float)sldSpecularAmount.Value,
                AmbientAmount2:(float)sldAmbientAmount2.Value, DiffuseAmount2: (float)sldDiffuseAmount2.Value, SpecularAmount2: (float)sldSpecularAmount2.Value, SpecularShine2: (float)sldSpecularShine2.Value,
                forceUpdate:true);
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

        private void sldDiffuseAmount2_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldAmbientAmount2_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldSpecularAmount2_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldSpecularShine2_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }
    }
}
