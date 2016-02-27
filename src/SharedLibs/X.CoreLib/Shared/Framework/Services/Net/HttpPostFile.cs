using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumoNinjaMonkey.Services.Networking
{
    public class HttpPostFile
    {
        public string Name
        {
            get;
            private set;
        }
        public string Filename
        {
            get;
            private set;
        }
        public string Path
        {
            get;
            private set;
        }
        public Stream Stream
        {
            get;
            private set;
        }
        public bool CloseStream
        {
            get;
            private set;
        }
        public string ContentType
        {
            get;
            set;
        }
        public HttpPostFile(string name, string filename, string path)
        {
            this.Name = name;
            this.Filename = System.IO.Path.GetFileName(filename);
            this.Path = path;
            this.CloseStream = true;
        }
        public HttpPostFile(string name, string path)
        {
            this.Name = name;
            this.Filename = System.IO.Path.GetFileName(path);
            this.Path = path;
            this.CloseStream = true;
        }
        public HttpPostFile(string name, string filename, Stream stream, bool closeStream = true)
        {
            this.Name = name;
            this.Filename = System.IO.Path.GetFileName(filename);
            this.Stream = stream;
            this.CloseStream = closeStream;
        }
    }
}
