using CoreLib.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace X.UI.RichImage
{
    public class IdToEffectDataTemplateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var id = (int)value;

            //DataTemplate dt = null;

            //switch (id) {
            //    case 1: dt = (DataTemplate)App.Current.Resources.MergedDictionaries[2]["dtGS"]; break;
            //    case 2: dt = (DataTemplate)App.Current.Resources.MergedDictionaries[2]["dtSat"]; break;
            //    default: dt = (DataTemplate)App.Current.Resources.MergedDictionaries[2]["dtGS"]; break;
            //}

            //return dt;


            var effect = new SaturationEffect();
            effect.Level = 150;
            effect.Source = (string)parameter;

            var grd = new Grid();
            grd.SetValue(Composition.EffectProperty, effect);

            return grd;


        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
