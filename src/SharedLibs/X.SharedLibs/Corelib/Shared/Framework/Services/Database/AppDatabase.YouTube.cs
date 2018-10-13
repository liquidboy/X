
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
        public async void AddUpdateYoutubePersistedItem(YoutubePersistedItem item)
        {
            LoggingService.LogInformation("writing to db 'Scene'", "AppDatabase.AddUpdateYoutubePersistedItem");
            var found = RetrieveYoutubePersistedItem(item.Id);
            
            if (found != null && found.Count() > 0)
            {
                this.Connection.Update(item);
                //await mstScene.UpdateAsync(scene);
            }
            else
            {
                var newId = this.Connection.Insert(item);
                //await mstScene.InsertAsync(scene);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "YoutubePersistedItem" });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = item.Id.ToString(), Action = "UPDATED" });

        }
        public async void AddUpdateYoutubeHistoryItem(YoutubeHistoryItem item)
        {
            LoggingService.LogInformation("writing to db 'Scene'", "AppDatabase.AddUpdateYoutubeHistoryItem");
            var found = RetrieveYoutubeHistoryItem(item.Id);

            if (found != null && found.Count() > 0)
            {
                this.Connection.Update(item);
            }
            else
            {
                var newId = this.Connection.Insert(item);
            }

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "YoutubeHistoryItem" });

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = item.Id.ToString(), Action = "UPDATED" });

        }









        //UPDATE
        public void UpdateYoutubePersistedItemField(int id, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'YoutubePersistedItem'", "AppDatabase.UpdateYoutubePersistedItemField");
            this.Connection.Execute("UPDATE YoutubePersistedItem set " + fieldName + " = ? where uid = ?", value, id);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = id.ToString(), Action = "UPDATED" });
        }
        public void UpdateYoutubeHistoryItemField(int id, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'YoutubeHistoryItem'", "AppDatabase.UpdateYoutubeHistoryItemField");
            this.Connection.Execute("UPDATE YoutubeHistoryItem set " + fieldName + " = ? where uid = ?", value, id);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = id.ToString(), Action = "UPDATED" });
        }


        public void UpdateYoutubePersistedItemField(string uid, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'YoutubePersistedItem'", "AppDatabase.UpdateYoutubePersistedItemField");
            this.Connection.Execute("UPDATE YoutubePersistedItem set " + fieldName + " = ? where uid = ?", value, uid);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = uid.ToString(), Action = "UPDATED" });
        }
        public void UpdateYoutubeHistoryItemField(string uid, string fieldName, object value, bool sendAggregateUpdateMessage = true)
        {
            LoggingService.LogInformation("writing to db 'YoutubeHistoryItem'", "AppDatabase.UpdateYoutubeHistoryItemField");
            this.Connection.Execute("UPDATE YoutubeHistoryItem set " + fieldName + " = ? where uid = ?", value, uid);
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("updating ...") { Identifier = "DB", SourceId = "UIElementState" });
            if (sendAggregateUpdateMessage) Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("") { Identifier = "YOUTUBE", AggregateId = uid.ToString(), Action = "UPDATED" });
        }













        //DELETE

        public void DeleteYoutubePersistedItems(string grouping)
        {
            if (!string.IsNullOrEmpty(grouping))
            {
                LoggingService.LogInformation("delete from db 'YoutubePersistedItem'", "AppDatabase.DeleteYoutubePersistedItems");
                this.Connection.Execute("delete from YoutubePersistedItem where Grouping = ?", grouping);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteYoutubePersistedItems" });
            
        }
        public void DeleteYoutubeHistoryItems(string grouping)
        {
            if (!string.IsNullOrEmpty(grouping))
            {
                LoggingService.LogInformation("delete from db 'YoutubeHistoryItem'", "AppDatabase.DeleteYoutubeHistoryItems");
                this.Connection.Execute("delete from YoutubeHistoryItem where Grouping = ?", grouping);
            }
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteYoutubeHistoryItems" });

        }
        public void DeleteYoutubePersistedItems(DateTime oldestTimespan)
        {

            LoggingService.LogInformation("delete from db 'YoutubePersistedItem'", "AppDatabase.DeleteOldYoutubePersistedItems");
            this.Connection.Execute("delete from YoutubePersistedItem where Timestamp < ?", oldestTimespan);

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteYoutubePersistedItems" });

        }
        public void DeleteYoutubeHistoryItems(DateTime oldestTimespan)
        {

            LoggingService.LogInformation("delete from db 'YoutubePersistedItem'", "AppDatabase.DeleteYoutubeHistoryItems");
            this.Connection.Execute("delete from YoutubeHistoryItem where Timestamp < ?", oldestTimespan);

            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("deleting ...") { Identifier = "DB", SourceId = "DeleteYoutubeHistoryItems" });

        }
















        //RETRIEVE

        public List<YoutubePersistedItem> RetrieveYouTubeByGrouping(string grouping1)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYouTubeByGrouping" });
            LoggingService.LogInformation("retrieve from db 'YoutubePersistedItem'", "AppDatabase.RetrieveYouTubeByGrouping");
            return this.Connection.Query<YoutubePersistedItem>("SELECT * FROM YoutubePersistedItem WHERE Grouping1 = ?", grouping1);
        }
        public List<YoutubeHistoryItem> RetrieveYouTubeHistoryByGrouping(string grouping1)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYouTubeHistoryByGrouping" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYouTubeHistoryByGrouping");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE Grouping1 = ?", grouping1);
        }
        public List<YoutubePersistedItem> RetrieveYoutubePersistedItem(string uniqueid)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubePersistedItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubePersistedItem'", "AppDatabase.RetrieveYoutubePersistedItem");
            return this.Connection.Query<YoutubePersistedItem>("SELECT * FROM YoutubePersistedItem WHERE Uid = ?", uniqueid);
        }
        public List<YoutubeHistoryItem> RetrieveYoutubeHistoryItem(string uniqueid)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubeHistoryItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYoutubeHistoryItem");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE Uid = ?", uniqueid);
        }
        public List<YoutubeHistoryItem> RetrieveYoutubeHistoryItemByID(string id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubeHistoryItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYoutubeHistoryItem");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE id = ?", id);
        }
        public List<YoutubePersistedItem> RetrieveYoutubePersistedItem(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubePersistedItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubePersistedItem'", "AppDatabase.RetrieveYoutubePersistedItem");
            return this.Connection.Query<YoutubePersistedItem>("SELECT * FROM YoutubePersistedItem WHERE Id = ?", id);
        }
        public List<YoutubeHistoryItem> RetrieveYoutubeHistoryItem(int id)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubeHistoryItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYoutubeHistoryItem");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE Id = ?", id);
        }
        public List<YoutubePersistedItem> RetrieveYoutubePersistedItem(DateTime timestamp)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubePersistedItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubePersistedItem'", "AppDatabase.RetrieveYoutubePersistedItem");
            return this.Connection.Query<YoutubePersistedItem>("SELECT * FROM YoutubePersistedItem WHERE timestamp > ?", timestamp);
        }
        public List<YoutubeHistoryItem> RetrieveYoutubeHistoryItem(DateTime timestamp)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubeHistoryItem" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYoutubeHistoryItem");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE timestamp > ?", timestamp);
        }
        public List<YoutubePersistedItem> RetrieveYoutubePersistedItemByGrouping(string grouping)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubePersistedItemByGrouping" });
            LoggingService.LogInformation("retrieve from db 'YoutubePersistedItem'", "AppDatabase.RetrieveYoutubePersistedItemByGrouping");
            return this.Connection.Query<YoutubePersistedItem>("SELECT * FROM YoutubePersistedItem WHERE Grouping = ?", grouping);
        }
        public List<YoutubeHistoryItem> RetrieveYoutubeHistoryItemByGrouping(string grouping)
        {
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("retrieving ...") { Identifier = "DB", SourceId = "RetrieveYoutubeHistoryItemByGrouping" });
            LoggingService.LogInformation("retrieve from db 'YoutubeHistoryItem'", "AppDatabase.RetrieveYoutubeHistoryItemByGrouping");
            return this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE Grouping = ?", grouping);
        }


        public bool DoesYouTubePersistedItemExist(string UID)
        {
            var found  = this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubePersistedItem WHERE Uid = ?", UID);
            if (found != null && found.Count() > 0)
            {
                return true;
            }

            return false;
        }
        public bool DoesYoutubeHistoryItemExist(string UID, string grouping)
        {
            var found = this.Connection.Query<YoutubeHistoryItem>("SELECT * FROM YoutubeHistoryItem WHERE Uid = ? and Grouping = ?", UID, grouping);
            if (found != null && found.Count() > 0)
            {
                return true;
            }

            return false;
        }







  

    }


    public class YoutubePersistedItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImagePath { get; set; }
        public double Size { get; set; }

        public string VideoId { get; set; }
        public string VideoLink { get; set; }

        public string Grouping { get; set; }
        public DateTime Timestamp { get; set; }
        public string NewUID { get; set; }

    }

    public class YoutubeHistoryItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Uid { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string ImagePath { get; set; }
        public double Size { get; set; }

        public string VideoId { get; set; }
        public string VideoLink { get; set; }

        public string Grouping { get; set; }
        public DateTime Timestamp { get; set; }

        public string VideoLinkFull { get; set; }
        public string VideoLinkFullType { get; set; }

        public string UIStateFull { get; set; }
        public string NewUID { get; set; }
    }


}
