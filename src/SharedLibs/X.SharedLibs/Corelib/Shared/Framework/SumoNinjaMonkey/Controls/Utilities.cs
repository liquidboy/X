

using System.IO;
using System.Text;
//using FlickrNet;
namespace SumoNinjaMonkey.Framework
{
    public class Utilities
    {

        ////SERIALIZATION
        //public static string SerializeFlickrParsableObject<T>(IFlickrParsable o)
        //{
        //    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
        //    StringBuilder sb = new StringBuilder();
        //    StringWriter sw = new StringWriter(sb);
        //    xs.Serialize(sw, o);
        //    return sb.ToString();
        //}

        //public static T DeSerializeFlickrParsableObject<T>(string o)
        //{
        //    System.IO.StringReader sr = new StringReader(o);

        //    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
        //    return (T)xs.Deserialize(sr);
        //}



        //APPDATA
        public static void AppDataAdd(string key, string data)
        {
            AppDataRemove(key);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Add(key, data);
        }

        public static void AppDataRemove(string key)
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove(key);
            }
        }

        public static string AppDataGet(string key)
        {
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                return (string)Windows.Storage.ApplicationData.Current.LocalSettings.Values[key];
            }
            else return string.Empty;

        }
    }
}
