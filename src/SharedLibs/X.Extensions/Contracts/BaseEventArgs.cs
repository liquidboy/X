using CoreLib.Extensions;
using System;

namespace X.Extensions
{
    public class BaseEventArgs : EventArgs, IReceiver
    {
        private ExtensionType _receiverType;
        public ExtensionType ReceiverType { get { return _receiverType; } set { _receiverType = value; } }
    }
}
