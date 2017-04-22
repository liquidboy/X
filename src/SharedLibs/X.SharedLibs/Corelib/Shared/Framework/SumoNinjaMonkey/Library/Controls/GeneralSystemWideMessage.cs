using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;

namespace SumoNinjaMonkey.Framework.Controls.Messages
{
    public class GeneralSystemWideMessage : GenericMessage<string>
    {
        public GeneralSystemWideMessage(string content)
            : base(content)
        {
            Identifier = string.Empty;
        }

        public string Identifier { get; set; }
        public string Action { get; set; }
        public string SourceId { get; set; }
        public string AggregateId { get; set; } 

        public string Url1 { get; set; }
        public string Url2 { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public int Int1 { get; set; }
        public int Int2 { get; set; }
        public long Long1 { get; set; }
        public long Long2 { get; set; }
        public double Dbl1 { get; set; }
        public double Dbl2 { get; set; }
        public float float1 { get; set; }
        public float float2 { get; set; }
        public float float3 { get; set; }
        public DateTime Date1 { get; set; }
        public DateTime Date2 { get; set; }
        public Guid Guid1 { get; set; }
        public Guid Guid2 { get; set; }
        public object Data1 { get; set; }
        public object Data2 { get; set; }

        public GeneralSystemWideMessage Clone()
        {
            GeneralSystemWideMessage clonedMsg = new GeneralSystemWideMessage(Content);


            clonedMsg.Identifier = Identifier;
            clonedMsg.Action  = Action;
            clonedMsg.SourceId = SourceId;
            clonedMsg.AggregateId = AggregateId;

            clonedMsg.Url1 = Url1;
            clonedMsg.Url2 = Url2;
            clonedMsg.Text1 = Text1;
            clonedMsg.Text2 = Text2;
            clonedMsg.Int1 = Int1;
            clonedMsg.Int2 = Int2;
            clonedMsg.Long1 = Long1;
            clonedMsg.Long2 = Long2;
            clonedMsg.Dbl1 = Dbl1;
            clonedMsg.Dbl2 = Dbl2;
            clonedMsg.float1 = float1;
            clonedMsg.float2 = float2;
            clonedMsg.float3 = float2;
            clonedMsg.Date1 = Date1;
            clonedMsg.Date2 = Date2;
            clonedMsg.Guid1 = Guid1;
            clonedMsg.Guid2 = Guid2;
            clonedMsg.Data1 = Data1;
            clonedMsg.Data2 = Data2;

            return clonedMsg;
        }
    }

}
