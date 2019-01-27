using Popcorn.Models.Movie;
using Popcorn.Models.Shows;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace X.Store.Converters
{
    public class PosterFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return null;

            if (value is ShowJson)
            {
                var show = (ShowJson)value;
                return show.Images.Poster;
            }
            else if (value is MovieJson)
            {
                var movie = (MovieJson)value;
                return movie.PosterImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
