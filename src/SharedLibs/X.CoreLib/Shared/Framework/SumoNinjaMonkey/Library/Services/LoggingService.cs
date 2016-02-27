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
    public class LoggingService
    {
        private static bool _isEnabled = false;
        
        public static List<LogMessage> LoggingMessages = new List<LogMessage>();
        private static List<LogMessage> _shadowLoggingMessages = new List<LogMessage>();

        private static bool _isSavingToDb = false;
        private static SqliteDatabase _db;

        private static DispatcherTimer _dtSave;
        private static bool _isInitialized = false;

        public static void Init(SqliteDatabase db)
        {
            if (db == null) return;
            if (_isInitialized) return;

            _dtSave = new DispatcherTimer();
            _dtSave.Interval = TimeSpan.FromSeconds(60); //attempt
            _dtSave.Tick += (o, a) => { 
                _dtSave.Stop();
                if (!_db.SqliteDb.IsInTransaction)
                {
                    PersistLoggingInformation();
                }
                _dtSave.Start(); 
            };

            _db = db;

            _db.SqliteDb.CreateTable<LogMessage>();
            
            _isInitialized = true;

        }

        public static void Start()
        {
            _isEnabled = true;
            _dtSave.Start();
        }

        public static void Stop()
        {
            _dtSave.Stop();
            _isEnabled = false;
        }

        public async static Task<bool> Clear() {
            if (_db!=null && !_db.SqliteDb.IsInTransaction)
            {
                await ClearLoggingInformationAsync();
            }

            return false;
        }

        public async void LogError(string userFriendlyMessage, string message, Exception ex = null)
        {
            if (!LoggingService._isEnabled)
            {
                return;
            }


            var newMsg = new LogMessage()
            {
                FriendlyMessage = userFriendlyMessage,
                Message = message,
                Exception = null,
                Type = 3
            };

            WriteMsg(newMsg);
        }

        public static async void LogInformation(string userFriendlyMessage, string message)
        {
            if (!LoggingService._isEnabled)
            {
                return;
            }

            var newMsg = new LogMessage()
                {
                    FriendlyMessage = userFriendlyMessage,
                    Message = message,
                    Exception = null,
                    Type = 1
                };

            WriteMsg(newMsg);
        }

        private static async void WriteMsg(LogMessage newMsg)
        {

            if (newMsg == null) return;

            await Task.Run(() => {

                newMsg.DateStamp = DateTime.UtcNow;

                if (_isSavingToDb)
                {
                    _shadowLoggingMessages.Add(newMsg);
                }
                else
                {
                    LoggingMessages.Add(newMsg);
                }

                Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage("writing ...") { Identifier = "LB", SourceId = "WriteMsg" });

            });


        }

        private static async void PersistLoggingInformation()
        {
            if ( LoggingMessages==null 
                || LoggingMessages.Count()==0 
                || !_isEnabled 
                || _db == null ) 
            {
                return;
            }

            
            await Task.Run(() => {
                
                
                _isSavingToDb = true;
                try
                {
                    //foreach (var item in LoggingMessages)
                    //{
                    _db.SqliteDb.InsertAll(LoggingMessages);
                    //_db.SqliteDb.Commit();
                    //}
                }
                catch (Exception ex){
                    //what is the error?
                    var msg = ex.Message;
                }

                LoggingMessages.Clear();

                _isEnabled = false;
                LoggingMessages.AddRange(_shadowLoggingMessages);

                
                _shadowLoggingMessages.Clear();
                _isSavingToDb = false;
                _isEnabled = true;
            
            });

        }

        private static async Task<bool> ClearLoggingInformationAsync()
        {
            
            if (LoggingMessages == null
                || _isSavingToDb
                || _db == null)
            {
                return false;
            }



            await Task.Run(() => {


                _isSavingToDb = true;
                try
                {
                    _db.SqliteDb.DeleteAll<LogMessage>();

                    
                }
                catch (Exception ex)
                {
                    var msg = ex.Message;
                }

                LoggingMessages.Clear();
                _shadowLoggingMessages.Clear();

                _isSavingToDb = false;

            });

            return true;
        }
    }


    public class LogMessage
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string FriendlyMessage { get; set; }

        public string Message { get; set; }
        public string Exception { get; set; }
        public int Type { get; set; }

        public DateTime DateStamp { get; set; }
    }



}
