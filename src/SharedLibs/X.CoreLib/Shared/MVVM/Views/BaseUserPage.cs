using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using X.CoreLib.Shared.Services;
using SumoNinjaMonkey.Framework.Services;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.UI.Xaml;

namespace FavouriteMX.Shared.Views
{
    public class BaseUserPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Brush AccentColor {get;set;}
        public Brush AccentColorLightBy1Degree { get; set; }
        public Brush AccentColorLightBy2Degree { get; set; }

        public Brush BackgroundColor { get; set; }
        public Brush BackgroundDarkBy1Color { get; set; }
        public Brush BackgroundDarkBy2Color { get; set; }

        public string SessionID { get; set; }
        public GlobalState State { get; set; }

        public BaseUserPage()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

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

       



        private void FillSessionDataFromDB()
        {
            string[] pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.PrimaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.SecondaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy1Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColorLightBy1Degree");

            pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.ThirdAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy2Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColorLightBy2Degree");


            pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.PrimaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.SecondaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy1Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundDarkBy1Color");

            pac = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.ThirdBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy2Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundDarkBy2Color");

            SessionID = AppDatabase.Current.RetrieveInstanceAppState(AppDatabase.AppSystemDataEnums.UserSessionID).Value;


        }

        public void SendSystemWideMessage(string identifier, string content, string sourceId = "", string action = "", string url1 = "", string aggregateId = "", string text1 = "", int int1 = 0)
        {
            LoggingService.LogInformation("system message ... " + content, "BaseUserPage.SendSystemWideMessage");
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage(content) { Identifier = identifier, SourceId = sourceId, Url1 = url1, Action = action, AggregateId = aggregateId, Text1 = text1, Int1 = int1 });
        }

        public void SendInformationNotification(string msg, double duration, string imageIcon = "")
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
                scaleIcon: 1.5,
                imageIcon: imageIcon
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
            GeneralSystemWideMessage msgToPassAlong = null,
            string imageIcon = ""
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
                msgToPassAlong: msgToPassAlong,
                imageIcon:imageIcon
                );


        }

        public void InputBox(
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
            GeneralSystemWideMessage msgToPassAlong = null,
            string imageIcon = ""
            )
        {
            LoggingService.LogInformation(question, "BaseUserPage.InputBoxYesNo");

            InputBoxService.Show(
                question,
                "",
                new SolidColorBrush(Colors.Black),
                AccentColor,
                new SolidColorBrush(Colors.White),
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

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }




        public virtual void Unload()
        {

        }


        public string SplitPart(string concatValue, int indexToGet, string splitter = "|")
        {
            string[] parts = concatValue.Split(splitter.ToCharArray());

            if (indexToGet > parts.Length) return string.Empty;
            else return parts[indexToGet];
        }

        public async Task<StorageFile> FileExists(string subFolder, string filename, int type = 1)
        {
            var files = await GetFilesAsync(subFolder, 2);

            var file = files.FirstOrDefault(x => x.Name == filename);
            if (file != null)
            {
                return file;
                //return "ms-appdata:///local/" + filename;
            }
            return null;
        }

        private async Task<IReadOnlyList<StorageFile>> GetFilesAsync(string subFolder, int type = 1)
        {
            //var folder = ApplicationData.Current.LocalFolder;
            var parts = subFolder.Split("\\".ToCharArray());
            StorageFolder folderToUse = null;
            foreach (var part in parts)
            {
                StorageFolder tempFolder = null;
                if (folderToUse == null)
                {
                    if (type == 1) tempFolder = await KnownFolders.VideosLibrary.GetFolderAsync(part);
                    else if (type == 2) tempFolder = await KnownFolders.PicturesLibrary.GetFolderAsync(part);
                }
                else tempFolder = await folderToUse.GetFolderAsync(part);

                folderToUse = tempFolder;
            }
            //var folder = await KnownFolders.VideosLibrary.GetFolderAsync(subFolder);
            return await folderToUse.GetFilesAsync(CommonFileQuery.OrderByName)
                               .AsTask().ConfigureAwait(false);
        }


        public childItem FindVisualChild<childItem>(DependencyObject obj) 
            where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }


        public void NotifyGCTotalMemory()
        {
            var t = GC.GetTotalMemory(false);
            SendInformationNotification(t.ToString(), 2);
        }


        
    }
}
