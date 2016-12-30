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
    public sealed partial class SpotlightDiffuse : UserControl
    {
        private LightPanel _lp;
        public SpotlightDiffuse()
        {
            this.InitializeComponent();
        }

        public void InitUI(float AmbientAmount, float DiffuseAmount, float SpecularAmount, float InnerConeAngle, float OuterConeAngle) {
            sldAmbientAmount.Value = AmbientAmount;
            sldDiffuseAmount.Value = DiffuseAmount;
            sldSpecularAmount.Value = SpecularAmount;
            sldInnerConeAngle.Value = InnerConeAngle;
            sldOuterConeAngle.Value = OuterConeAngle;
        }

        public void SetLightPanel(ref LightPanel lp) {
            _lp = lp;
        }

        private void updateData() {
            _lp?.UpdateLightingEffect_SpotLightDiffuse((float)sldAmbientAmount.Value, (float)sldDiffuseAmount.Value, (float)sldSpecularAmount.Value, InnerConeAngle: (float)sldInnerConeAngle.Value, OuterConeAngle: (float)sldOuterConeAngle.Value, forceUpdate:true);
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

        private void sldInnerConeAngle_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }

        private void sldOuterConeAngle_ValueChanged(Object sender, RangeBaseValueChangedEventArgs e)
        {
            updateData();
        }
    }
}
