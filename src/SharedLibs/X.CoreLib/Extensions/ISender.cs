using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface ISender
    {
        void SendMessage(object msg, ExtensionType type);
    }
}
