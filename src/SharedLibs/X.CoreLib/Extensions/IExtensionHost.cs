using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface IExtensionHost
    {
        Task InitExtensions();
        void UnInitExtensions();
        void LaunchExtension(Guid extGuid);
        void CloseExtension(Guid extGuid);
    }
}
