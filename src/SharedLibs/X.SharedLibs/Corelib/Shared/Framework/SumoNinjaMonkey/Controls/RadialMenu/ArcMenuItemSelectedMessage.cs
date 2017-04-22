using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace SumoNinjaMonkey.Framework.Controls.Messages
{
    public class ArcMenuItemSelectedMessage : GenericMessage<string>
    {
        public ArcMenuItemSelectedMessage(string parm)
            : base(parm)
        {
            Identifier = string.Empty;
        }

        public string Identifier { get; set; }
    }

}
