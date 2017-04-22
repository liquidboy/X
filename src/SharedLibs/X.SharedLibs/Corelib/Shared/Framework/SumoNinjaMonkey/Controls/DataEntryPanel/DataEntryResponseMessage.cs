using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace SumoNinjaMonkey.Framework.Controls.Messages
{
    public class DataEntryResponseMessage : GenericMessage<string>
    {
        public DataEntryResponseMessage(string parm)
            : base(parm)
        {
            Identifier = string.Empty;
        }

        public string Identifier { get; set; }
    }

}
