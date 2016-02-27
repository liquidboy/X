using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;
using SQLite;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace SumoNinjaMonkey.Framework.Services
{
    public class AlertService
    {
        private static bool _isEnabled = false;

        public static List<AlertMessage> AlertMessages = new List<AlertMessage>();
        private static List<AlertMessage> _shadowAlertMessages = new List<AlertMessage>();

        private static bool _isSavingToDb = false;
        //private static SqliteDatabase _db;

        ////private static DispatcherTimer _dtSave;


        public static void Init(SqliteDatabase db)
        {
            if (db == null) return;

            ////_dtSave = new DispatcherTimer();
            ////_dtSave.Interval = TimeSpan.FromSeconds(60); //attempt
            ////_dtSave.Tick += (o, a) => { 
            ////    _dtSave.Stop();
            ////    if (!_db.SqliteDb.IsInTransaction)
            ////    {
            ////        PersistAlertInformation();
            ////    }
            ////    _dtSave.Start(); 
            ////};

            //_db = db;

            //_db.SqliteDb.CreateTable<AlertMessage>();
        }

        public static void Start()
        {
            _isEnabled = true;
            //_dtSave.Start();
        }

        public static void Stop()
        {
            //_dtSave.Stop();
            _isEnabled = false;
        }



        public static async void LogAlertMessage(string userFriendlyMessage, string message)
        {
            //if (!AlertService._isEnabled)
            //{
            //    return;
            //}

            //var newMsg = new AlertMessage()
            //    {
            //        FriendlyMessage = userFriendlyMessage,
            //        Message = message,
            //        Type = 1
            //    };

            //WriteMsg(newMsg);
        }

        private static async void WriteMsg(AlertMessage newMsg)
        {

            //if (newMsg == null) return;

            //await Task.Run(() =>
            //{

            //    newMsg.DateStamp = DateTime.UtcNow;

            //    if (_isSavingToDb)
            //    {
            //        _shadowAlertMessages.Add(newMsg);
            //    }
            //    else
            //    {
            //        AlertMessages.Add(newMsg);
            //    }

            //    Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("alert ...") { Identifier = "AB", SourceId = "AlertService.WriteMsg" });

            //});


        }

        private static async void PersistAlertInformation()
        {
            //if (AlertMessages == null
            //    || AlertMessages.Count() == 0
            //    || !_isEnabled
            //    || _db == null)
            //{
            //    return;
            //}


            //await Task.Run(() =>
            //{

            //    _isSavingToDb = true;

            //    //foreach (var item in LoggingMessages)
            //    //{

            //    _db.SqliteDb.InsertAll(AlertMessages, true);
            //    _db.SqliteDb.Commit();
            //    //}

            //    AlertMessages.Clear();

            //    _isEnabled = false;
            //    AlertMessages.AddRange(_shadowAlertMessages);


            //    _shadowAlertMessages.Clear();
            //    _isSavingToDb = false;
            //    _isEnabled = true;

            //});

        }
    }


    public class AlertMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string FriendlyMessage { get; set; }

        public string Message { get; set; }
        public int Type { get; set; }

        public DateTime DateStamp { get; set; }
    }



}
