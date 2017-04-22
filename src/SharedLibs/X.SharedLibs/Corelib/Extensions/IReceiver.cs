using CoreLib.Extensions;
using System;

namespace CoreLib.Extensions
{
    public interface IReceiver
    {
        ExtensionType ReceiverType
        {
            get; set;
        }
    }
}
