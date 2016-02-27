
using GalaSoft.MvvmLight;

using System;

using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using FavouriteMX.Shared.Services;
using GalaSoft.MvvmLight.Command;
using SumoNinjaMonkey.Framework.Services;
using SumoNinjaMonkey.Framework.Controls.Messages;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Xaml.Media;
using Windows.UI;
using FavouriteMX.Shared.Views;
using Windows.UI.Xaml.Controls;

namespace FavouriteMX.Shared.Models
{
    public partial class DefaultViewModel : ViewModelBase
    {
        private bool _isBusy;
        private bool _canShare = true;

        public Brush AccentColor { get; set; }
        public Brush AccentColorLightBy1Degree { get; set; }
        public Brush AccentColorLightBy2Degree { get; set; }

        public Brush BackgroundColor { get; set; }
        public Brush BackgroundDarkBy1Color { get; set; }
        public Brush BackgroundDarkBy2Color { get; set; }

        public string SessionID { get; set; }
        public GlobalState State { get; set; }



        private bool _TopAppBarIsVisible;
        public bool TopAppBarIsVisible
        {
            get
            {
                return this._TopAppBarIsVisible;
            }
            set
            {
                if (value != this._TopAppBarIsVisible)
                {
                    this._TopAppBarIsVisible = value;
                    this.RaisePropertyChanged("TopAppBarIsVisible");
                }
            }
        }

        private bool _BottomAppBarIsVisible;
        public bool BottomAppBarIsVisible
        {
            get
            {
                return this._BottomAppBarIsVisible;
            }
            set
            {
                if (value != this._BottomAppBarIsVisible)
                {
                    this._BottomAppBarIsVisible = value;
                    this.RaisePropertyChanged("BottomAppBarIsVisible");
                }
            }
        }

        private UserControl _TopAppBarUserControl { get; set; }
        public UserControl TopAppBarUserControl
        {
            get
            {
                return this._TopAppBarUserControl;
            }
            set
            {
                if (value != this._TopAppBarUserControl)
                {
                    this._TopAppBarUserControl = value;
                    this.RaisePropertyChanged("TopAppBarUserControl");
                }
            }
        }

        private UserControl _BottomAppBarUserControl { get; set; }
        public UserControl BottomAppBarUserControl
        {
            get
            {
                return this._BottomAppBarUserControl;
            }
            set
            {
                if (value != this._BottomAppBarUserControl)
                {
                    this._BottomAppBarUserControl = value;
                    this.RaisePropertyChanged("BottomAppBarUserControl");
                }
            }
        }

        

        public bool IsBusy
        {
            get
            {
                return this._isBusy;
            }
            set
            {
                if (value != this._isBusy)
                {
                    this._isBusy = value;
                    this.RaisePropertyChanged("IsBusy");
                }
            }
        }
        public RelayCommand BackCommand
        {
            get;
            set;
        }
        public RelayCommand ShareCommand
        {
            get;
            set;
        }
        public virtual bool CanShare
        {
            get
            {
                return this._canShare;
            }
            set
            {
                this._canShare = value;
                this.RaisePropertyChanged("CanShare");
            }
        }
        public DefaultViewModel()
        {
  

            //this.BackCommand = new RelayCommand(delegate
            //{
            //    this.OnBackPressed();
            //    NavigationServiceBase.GoBack();
            //}
            //);
            //this.ShareCommand = new RelayCommand(delegate
            //{
            //    this.Share();
            //}
            //, () => this.CanShare);



            State = new GlobalState();
            try
            {
                FillSessionDataFromDB();
            }
            catch
            {
                AppDatabase.Current.RecreateSystemData();
                FillSessionDataFromDB();
            }


        }
        
        protected virtual void OnBackPressed()
        {
        }
        public virtual void Share()
        {
        }
        public virtual void OnNavigatedTo(object parameters)
        {
        }
        public virtual void OnNavigatedFrom(object parameters)
        {
        }

      


        public void ClearTileNotification()
        {
            try
            {
                if (AppService.IsConnected())
                {
                    TileUpdateManager.CreateTileUpdaterForApplication().Clear();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void SendTileTextNotification(string imageUrl = "ms-appx:///Assets/MainTiles/AlphaPad.jpg")
        {
            try
            {
                if (AppService.IsConnected())
                {
                    //+ TEMPLATE
                    XmlDocument templateContent = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquarePeekImageAndText01);
                    templateContent.GetXml().ToString();

                    //  TEMPLATE + TITLE
                    XmlNodeList elementsByTagName = templateContent.GetElementsByTagName("text");
                    elementsByTagName[0].AppendChild(templateContent.CreateTextNode("Title 01"));

                    //  TEMPLATE + IMAGE
                    XmlNodeList elementsByTagName2 = templateContent.GetElementsByTagName("image");
                    XmlElement xmlElement = (XmlElement)elementsByTagName2.Item(0u);
                    xmlElement.SetAttribute("src", imageUrl);
                    xmlElement.SetAttribute("alt", "Sample SharpDx/XAML/C# apps");

                    //+ NOTIFICATION
                    //  NOTIFICATION + TEMPLATE
                    TileNotification tileNotification = new TileNotification(templateContent);
                    tileNotification.ExpirationTime = new DateTimeOffset?(DateTimeOffset.Now.AddMinutes(1.0));
                    tileNotification.Tag = "Tag 01";

                    //UPDATE APP
                    // -> NOTIFICATION
                    TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
                    TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
                }
            }
            catch (Exception)
            {
            }
        }

        private void FillSessionDataFromDB()
        {
            
            string[] pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.PrimaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("AccentColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.SecondaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy1Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("AccentColorLightBy1Degree");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.ThirdAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy2Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("AccentColorLightBy2Degree");


            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.PrimaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("BackgroundColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.SecondaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy1Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("BackgroundDarkBy1Color");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.ThirdBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy2Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            RaisePropertyChanged("BackgroundDarkBy2Color");

            SessionID = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.UserSessionID).Value;


        }

        public void SendSystemWideMessage(string identifier, string content, string sourceId = "", string action = "", string url1 = "", string aggregateId = "", string text1 = "", int int1 = 0)
        {
            LoggingService.LogInformation("system message ... " + content, "BaseUserPage.SendSystemWideMessage");
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage(content) { Identifier = identifier, SourceId = sourceId, Url1 = url1, Action = action, AggregateId = aggregateId, Text1 = text1, Int1 = int1 });
        }

        public void SendInformationNotification(string msg, double duration)
        {

            LoggingService.LogInformation(msg, "BaseUserPage.SendInformationNotification");

            NotificationService.Show(
                msg,
                "",
                new SolidColorBrush(Colors.White),
                AccentColorLightBy1Degree,
                duration,
                height: 90,
                width: 350,
                autoHide: true,
                metroIcon: "Information",
                scaleIcon: 1.5
                );
        }

        public void MessageBox(
            string question,
            string yesLabel,
            string yesMessengerContent,
            string yesMessengerIdentifier,
            string noLabel,
            string noMessengerContent,
            string noMessengerIdentifier,
            string metroIcon = "Information",
            double translateXIcon = - 20,
            double translateYIcon = - 20,
            double scaleIcon = 2,
            GeneralSystemWideMessage msgToPassAlong = null
            )
        {
            LoggingService.LogInformation(question, "BaseUserPage.MessageBoxYesNo");

            MsgBoxService.Show(
                question,
                "",
                new SolidColorBrush(Colors.Black),
                AccentColor,
                new SolidColorBrush(Colors.White),
                height: 200,
                width: 480,
                metroIcon: metroIcon,
                scaleIcon: scaleIcon,
                translateXIcon: translateXIcon,
                translateYIcon: translateYIcon,
                yesLabel: yesLabel,
                yesMessengerContent: yesMessengerContent,
                yesMessengerIdentifier: yesMessengerIdentifier,
                noLabel: noLabel,
                noMessengerContent: noMessengerContent,
                noMessengerIdentifier: noMessengerIdentifier,
                msgToPassAlong: msgToPassAlong
                );


        }




    }


    





    

}
