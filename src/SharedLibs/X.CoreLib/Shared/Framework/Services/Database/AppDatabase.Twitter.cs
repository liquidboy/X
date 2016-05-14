
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using X.CoreLib.SQLite;
using SumoNinjaMonkey.Framework;
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using System.Linq;
using Windows.Foundation;
using System;
using System.Runtime.Serialization;


namespace X.CoreLib.Shared.Services
{
    public partial class AppDatabase
    {



        //ADD
        public async void AddUpdateTwitterPersistedItem(TwitterPersistedItem item)
        {
            LoggingService.LogInformation("writing to db 'Scene'", "AppDatabase.AddUpdateTwitterPersistedItem");
            var found = RetrieveTwitterPersistedItem(item.TwitterId);

            if (found != null && found.Count() > 0)
            {
                this.SqliteDb.Update(item);
            }
            else
            {
                var newId = this.SqliteDb.Insert(item);
            }

            Messenger.Default.Send(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "TwitterPersistedItem" });
            Messenger.Default.Send(new GeneralSystemWideMessage("") { Identifier = "TWITTER", AggregateId = item.Id.ToString(), Action = "UPDATED" });

        }






        //UPDATE
        public void UpdateTwitterPersistedItemField(int id, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'TwitterPersistedItem'", "AppDatabase.UpdateTwitterPersistedItemField");
            this.SqliteDb.Execute("UPDATE TwitterPersistedItem set " + fieldName + " = ? where uid = ?", value, id);
            Messenger.Default.Send(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send(new GeneralSystemWideMessage("") { Identifier = "TWITTER", AggregateId = id.ToString(), Action = "UPDATED" });
        }

        public void UpdateTwitterPersistedItemField(string uid, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'TwitterPersistedItem'", "AppDatabase.UpdateTwitterPersistedItemField");
            this.SqliteDb.Execute("UPDATE TwitterPersistedItem set " + fieldName + " = ? where uid = ?", value, uid);
            Messenger.Default.Send(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send(new GeneralSystemWideMessage("") { Identifier = "TWITTER", AggregateId = uid.ToString(), Action = "UPDATED" });
        }

        public void UpdateTwitterPersistedItemField(long twitterId, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'TwitterPersistedItem'", "AppDatabase.UpdateTwitterPersistedItemField");
            this.SqliteDb.Execute("UPDATE TwitterPersistedItem set " + fieldName + " = ? where twitterId = ?", value, twitterId);
            Messenger.Default.Send(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send(new GeneralSystemWideMessage("") { Identifier = "TWITTER", AggregateId = twitterId.ToString(), Action = "UPDATED" });
        }









        //DELETE
        public void DeleteTwitterPersistedItems(string grouping1)
        {
            if (!string.IsNullOrEmpty(grouping1))
            {
                LoggingService.LogInformation("delete from db 'TwitterPersistedItem'", "AppDatabase.DeleteTwitterPersistedItems");
                this.SqliteDb.Execute("delete from TwitterPersistedItem where Grouping1 = ?", grouping1);
            }
            Messenger.Default.Send(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteTwitterPersistedItems" });

        }

        public void DeleteTwitterPersistedItems(DateTime oldestTimespan)
        {

            LoggingService.LogInformation("delete from db 'TwitterPersistedItem'", "AppDatabase.DeleteOldTwitterPersistedItems");
            this.SqliteDb.Execute("delete from TwitterPersistedItem where Timestamp < ?", oldestTimespan);

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteTwitterPersistedItems" });

        }
        public void DeleteTwitterPersistedItem(long twitterId)
        {

            LoggingService.LogInformation("delete from db 'TwitterPersistedItem'", "AppDatabase.DeleteTwitterPersistedItems");
            this.SqliteDb.Execute("delete from TwitterPersistedItem where TwitterId = ?", twitterId);
            
            Messenger.Default.Send(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteTwitterPersistedItems" });

        }
        public void DeleteTwitterPersistedItem(int id)
        {

            LoggingService.LogInformation("delete from db 'TwitterPersistedItem'", "AppDatabase.DeleteTwitterPersistedItems");
            this.SqliteDb.Execute("delete from TwitterPersistedItem where Id = ?", id);

            Messenger.Default.Send(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteTwitterPersistedItems" });

        }











        //RETRIEVE

        public List<TwitterPersistedItem> RetrieveTwitterByGrouping(string grouping1)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveTwitterByGrouping" });
            LoggingService.LogInformation("retrieve from db 'TwitterPersistedItem'", "AppDatabase.RetrieveTwitterByGrouping");
            return this.SqliteDb.Query<TwitterPersistedItem>("SELECT * FROM TwitterPersistedItem WHERE Grouping1 = ?", grouping1);
        }
        public List<TwitterPersistedItem> RetrieveTwitterPersistedItem(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveTwitterPersistedItem" });
            LoggingService.LogInformation("retrieve from db 'TwitterPersistedItem'", "AppDatabase.RetrieveTwitterPersistedItem");
            return this.SqliteDb.Query<TwitterPersistedItem>("SELECT * FROM TwitterPersistedItem WHERE Id = ?", id);
        }
        public List<TwitterPersistedItem> RetrieveTwitterPersistedItem(long twitterId)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveTwitterPersistedItem" });
            LoggingService.LogInformation("retrieve from db 'TwitterPersistedItem'", "AppDatabase.RetrieveTwitterPersistedItem");
            return this.SqliteDb.Query<TwitterPersistedItem>("SELECT * FROM TwitterPersistedItem WHERE TwitterId = ?", twitterId);
        }







    }


    public class TwitterPersistedItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }


        public long TwitterId { get; set; }
        public string Name { get; set; }
        public string TextRaw { get; set; }
        public DateTime TextDateTime { get; set; }
        public string AvatarUrl { get; set; }
        public string NameAt { get; set; }



        public string Grouping1 { get; set; }
        public DateTime Timestamp { get; set; }


    }


}
