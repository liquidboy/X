using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NChardet
{
    /// <summary>
    /// 实现了一个默认的字符集检测观察器。
    /// </summary>
    public class DefaultCharsetDetectionObserver : NChardet.ICharsetDetectionObserver
    {
        public string Charset = null;

        public void Notify(string charset)
        {
            Charset = charset;
        }

    }

    /// <summary>
    /// 实现了一个默认的字符集检测器。
    /// </summary>
    public sealed class DefaultEncodeDetector
    {
        private DefaultEncodeDetector()
        {
        }

        /// <summary>
        /// 获取指定文本文件的编码字符集。
        /// </summary>
        /// <param name="filename">文件名。</param>
        /// <returns>返回检测到的字符集，找不到匹配的字符集则返回 System.Text.Encoding.Default。</returns>
        public static System.Text.Encoding GetEncodingOfFile(string filename)
        {
            int count;
            byte[] buf;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                if (fs.Length < 1) return System.Text.Encoding.Default;
                buf = new byte[fs.Length];
                count = fs.Read(buf, 0, buf.Length);
            }
            if (buf.Length > 2 && buf[0] == 0xEF && buf[1] == 0xBB && buf[2] == 0xBF) //UTF8 判定
            {
                return System.Text.Encoding.UTF8;
            }
            else if (buf.Length >= 2 && buf[0] == 0xFF && buf[1] == 0xFE)
            {
                if (buf.Length >= 4 && buf[2] == 0x00 && buf[3] == 0x00) //UTF32 判定
                {
                    return System.Text.Encoding.UTF32;
                }
                else
                {
                    return System.Text.Encoding.Unicode; //Unicode 判定
                }
            }
            else if (buf.Length >= 2 && buf[0] == 0xFE && buf[1] == 0xFF) //BigEndianUnicode 判定
            {
                return System.Text.Encoding.BigEndianUnicode;
            }
            else if (buf.Length >= 4 && buf[0] == 0x00 && buf[1] == 0x00 && buf[2] == 0xFE && buf[3] == 0xFF) //UTF-32BE 判定
            {
                return System.Text.Encoding.GetEncoding("UTF-32BE");
            }
            else
            {
                DefaultCharsetDetectionObserver cdo = new DefaultCharsetDetectionObserver();
                Detector detector = new Detector();
                detector.Init(cdo);
                if (detector.isAscii(buf, count))
                {
                    return System.Text.Encoding.ASCII;
                }
                else
                {
                    detector.DoIt(buf, count, false);
                    detector.Done();
                    if (string.IsNullOrEmpty(cdo.Charset))
                    {
                        return System.Text.Encoding.Default;
                    }
                    else
                    {
                        return System.Text.Encoding.GetEncoding(cdo.Charset);
                    }
                }
            }
        }

    }

}
