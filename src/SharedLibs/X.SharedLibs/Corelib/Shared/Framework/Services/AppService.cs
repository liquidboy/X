
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace X.CoreLib.Shared.Services
{
    public partial class AppService
    {
        private static bool _isEnabled = false;

        public static event EventHandler NetworkConnectionChanged;
        public static AppWideSetting AppSetting  = new  AppWideSetting();

        public static bool IsConnected()
        {
            try
            {
                if (NetworkInformation.GetInternetConnectionProfile() == null)
                {
                    return false;
                }
                else
                {
                    var nc = NetworkInformation.GetInternetConnectionProfile().GetNetworkConnectivityLevel();
                    //if (nc == null) return false;
                    if (nc == NetworkConnectivityLevel.None) return false;
                    if (nc == NetworkConnectivityLevel.LocalAccess) return true; //return false;
                    if (nc == NetworkConnectivityLevel.ConstrainedInternetAccess ||
                        nc == NetworkConnectivityLevel.InternetAccess) return true;

                    return false;
                }
            }
            catch //(Exception ex) 
            { 
            
            }
            return true;
        }

        public async static void Init()
        {

            //TODO:LOGIC TO DETERMINE WHEN TO PULL FROM CLOUD
            await GetSettingsLocalAndCloud();
            
        }



        private async static Task GetSettingsLocalAndCloud()
        {
            //bool doPullFromCloud = false;

            //DEFAULT SETTINGS
            var defaultSetting = new AppWideSetting();

            defaultSetting.ExplorerMXDocumentsFolder = "ExplorerMX";
            defaultSetting.FavouriteMXPictureFolder = "FavouriteMX";
            defaultSetting.WatchMXVideoFolder = "WatchMX";
            defaultSetting.FlickrKey = "102e389a942747faebb958c4db95c098";
            defaultSetting.FlickrSecret = "774b263b4d3a2578";
            defaultSetting.AMSUrl = "https://developermx.azure-mobile.net/";
            defaultSetting.AMSKey = "bnIWZFbEKBzNJXtXgMgAxHLtsOaYfW28";

            string forStorage = string.Format(
                "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                defaultSetting.ExplorerMXDocumentsFolder,
                defaultSetting.FavouriteMXPictureFolder,
                defaultSetting.WatchMXVideoFolder,
                defaultSetting.FlickrKey,
                defaultSetting.FlickrSecret,
                defaultSetting.AMSUrl,
                defaultSetting.AMSKey
            );

           

            //PULL FROM LOCAL
            var appState = AppDatabase.Current.RetrieveAppState("AppWideSetting");
            if (appState.Count == 0)
            {
                //use default settings
                AppSetting.ExplorerMXDocumentsFolder = defaultSetting.ExplorerMXDocumentsFolder;
                AppSetting.FavouriteMXPictureFolder = defaultSetting.FavouriteMXPictureFolder;
                AppSetting.WatchMXVideoFolder = defaultSetting.WatchMXVideoFolder;
                AppSetting.FlickrKey = defaultSetting.FlickrKey;
                AppSetting.FlickrSecret = defaultSetting.FlickrSecret;
                AppSetting.AMSUrl = defaultSetting.AMSUrl;
                AppSetting.AMSKey = defaultSetting.AMSKey;

                //save default settings to local store for future use
                AppDatabase.Current.SaveAppState("AppWideSetting", forStorage);
                
            }
            else
            {
                //use local settings
                var parts = appState[0].Value.Split("|".ToCharArray());

                AppSetting.ExplorerMXDocumentsFolder = parts[0];
                AppSetting.FavouriteMXPictureFolder = parts[1];
                AppSetting.WatchMXVideoFolder = parts[2];
                AppSetting.FlickrKey = parts[3];
                AppSetting.FlickrSecret = parts[4];
                AppSetting.AMSUrl = parts[5];
                AppSetting.AMSKey = parts[6];

            }


            //always pull from cloud to update local incase its changed from factory defaults
            try
            {
                var found = await AzureMobileService.Current.RetrieveAppWideSettingFromCloudAsync();

                if (found != null && found.Count == 0)
                {
                    //should never happen
                    AzureMobileService.Current.PushToCloud(defaultSetting);
                }
                else
                {
                    AppSetting.ExplorerMXDocumentsFolder = found[0].ExplorerMXDocumentsFolder;
                    AppSetting.FavouriteMXPictureFolder = found[0].FavouriteMXPictureFolder;
                    AppSetting.WatchMXVideoFolder = found[0].WatchMXVideoFolder;
                    AppSetting.FlickrKey = found[0].FlickrKey;
                    AppSetting.FlickrSecret = found[0].FlickrSecret;
                    AppSetting.AMSUrl = found[0].AMSUrl;
                    AppSetting.AMSKey = found[0].AMSKey;
                    
                    string forStorage2 = string.Format(
                        "{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                        AppSetting.ExplorerMXDocumentsFolder,
                        AppSetting.FavouriteMXPictureFolder,
                        AppSetting.WatchMXVideoFolder,
                        AppSetting.FlickrKey,
                        AppSetting.FlickrSecret,
                        AppSetting.AMSUrl,
                        AppSetting.AMSKey
                    );


                    //save default settings to local store for future use
                    AppDatabase.Current.SaveAppState("AppWideSetting", forStorage2);
                }
            }
            catch // (Exception ex)
            {

            }
            
                
                


           

            

            
        }



        private static void NetworkInformation_NetworkStatusChanged(object sender)
        {
            if (NetworkConnectionChanged != null) NetworkConnectionChanged(IsConnected(), EventArgs.Empty);
        }

        public static void Start()
        {
            if (_isEnabled) return;
                
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;

            _isEnabled = true;
        }

        public static void Stop()
        {
            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;

            _isEnabled = false;
        }




        public static void MessageBox(
            string question,
            string yesLabel,
            string yesMessengerContent,
            string yesMessengerIdentifier,
            string noLabel,
            string noMessengerContent,
            string noMessengerIdentifier,
            string metroIcon = "Information",
            double translateXIcon = -20,
            double translateYIcon = -20,
            double scaleIcon = 2,
            GeneralSystemWideMessage msgToPassAlong = null,
            string imageIcon = ""
            )
        {
            MsgBoxService.Show(
                question,
                "",
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(new Color() { R = 236, G = 236, B = 236, A = 255 }),
                new SolidColorBrush(Colors.Black),
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
                msgToPassAlong: msgToPassAlong,
                imageIcon: imageIcon
                );


        }

        public static void InputBox(
            string question,
            string yesLabel,
            string yesMessengerContent,
            string yesMessengerIdentifier,
            string noLabel,
            string noMessengerContent,
            string noMessengerIdentifier,
            string metroIcon = "Information",
            double translateXIcon = -20,
            double translateYIcon = -20,
            double scaleIcon = 2,
            GeneralSystemWideMessage msgToPassAlong = null,
            string imageIcon = ""
            )
        {
            InputBoxService.Show(
                question,
                "",
                new SolidColorBrush(Colors.Black),
                new SolidColorBrush(new Color() { R = 236, G = 236, B = 236, A = 255 }),
                new SolidColorBrush(Colors.Black),
                height: 260,
                width: 580,
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
                msgToPassAlong: msgToPassAlong,
                imageIcon: imageIcon
                );


        }



        public static void SendInformationNotification(string msg, double duration, string imageIcon = "")
        {

            LoggingService.LogInformation(msg, "BaseUserPage.SendInformationNotification");

            NotificationService.Show(
                msg,
                "",
                new SolidColorBrush(Colors.White),
                new SolidColorBrush(new Color() { R = 197, G = 197, B = 197, A = 255 }),
                duration,
                height: 90,
                width: 350,
                autoHide: true,
                metroIcon: "Information",
                scaleIcon: 1.5,
                imageIcon: imageIcon
                );
        }

    }
}
