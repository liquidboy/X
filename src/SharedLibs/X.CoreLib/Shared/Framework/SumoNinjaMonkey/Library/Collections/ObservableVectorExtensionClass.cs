using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Framework.Collections
{
    public static class ObservableVectorExtensionClass
    {
        public static ObservableVector<T> ToObservableVector<T>(this IEnumerable<T> s)
        {
            return new ObservableVector<T>(s);
        }
    }
}
