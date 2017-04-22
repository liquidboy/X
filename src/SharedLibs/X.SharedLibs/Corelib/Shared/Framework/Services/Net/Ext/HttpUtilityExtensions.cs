using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public static class HttpUtilityExtensions
    {
        public static Dictionary<string, string> ParseQueryString(string queryString)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string[] array = queryString.Split("&".ToCharArray()); 
            for (int i = 0; i < array.Length; i++)
            {
                string text = array[i];
                int num = text.IndexOf('=');
                if (num != -1 && num + 1 < text.Length)
                {
                    string key = text.Substring(0, num);
                    string value = Uri.UnescapeDataString(text.Substring(num + 1));
                    dictionary.Add(key, value);
                }
            }
            return dictionary;
        }
    }
}
