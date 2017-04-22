using System.IO;

namespace SumoNinjaMonkey.Services.Networking
{
    public static class StreamExtensions
    {
        public static byte[] ReadToEnd(this Stream input)
        {
            byte[] array = new byte[16384];
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                int count;
                while ((count = input.Read(array, 0, array.Length)) > 0)
                {
                    memoryStream.Write(array, 0, count);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }
    }
}
