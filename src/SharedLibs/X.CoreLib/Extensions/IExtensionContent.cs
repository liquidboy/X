using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib.Extensions
{
    public interface IExtensionContent
    {
        event EventHandler<EventArgs> SendMessage;
        void RecieveMessage(object message);
        void Unload();
    }
}
