using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FavouriteMX.Shared.Services;
using FavouriteMX.Shared.Views.Controls;
using GalaSoft.MvvmLight.Messaging;
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Foundation;
using System.IO;
using System.Runtime.Serialization.Json;

namespace FavouriteMX.Shared.Views
{
    public class BaseUserControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Brush AccentColor { get; set; }
        public Brush AccentColorLightBy1Degree { get; set; }
        public Brush AccentColorLightBy2Degree { get; set; }

        public Brush BackgroundColor { get; set; }
        public Brush BackgroundDarkBy1Color { get; set; }
        public Brush BackgroundDarkBy2Color { get; set; }
        
        public string SessionID { get; set; }
        public GlobalState State { get; set; }

        public BaseUserControl()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;


            try
            {
                FillSessionDataFromDB();
            }
            catch {

                AppDatabase.Current.RecreateSystemData();
                FillSessionDataFromDB();
            }
        }

        public void UnloadBaseControl()
        {
            this.AccentColor = null;
            this.AccentColorLightBy1Degree = null;
            this.AccentColorLightBy2Degree = null;

            this.BackgroundColor = null;
            this.BackgroundDarkBy1Color = null;
            this.BackgroundDarkBy2Color = null;
        }

        private void FillSessionDataFromDB()
        {
            string[] pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.PrimaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.SecondaryAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy1Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColorLightBy1Degree");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.ThirdAccentColor).Value.ToString().Split(",".ToCharArray());
            AccentColorLightBy2Degree = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("AccentColorLightBy2Degree");


            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.PrimaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundColor = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundColor");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.SecondaryBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy1Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundDarkBy1Color");

            pac = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.ThirdBackgroundColor).Value.ToString().Split(",".ToCharArray());
            BackgroundDarkBy2Color = new SolidColorBrush(new Color() { R = Byte.Parse(pac[0]), G = Byte.Parse(pac[1]), B = Byte.Parse(pac[2]), A = Byte.Parse(pac[3]) });
            OnPropertyChanged("BackgroundDarkBy2Color");


            SessionID = AppDatabase.Current.RetrieveInstanceAppState(FavouriteMX.Shared.Services.AppDatabase.AppSystemDataEnums.UserSessionID).Value;

        }

        public void UpdateTheme()
        {

        }


        public void SendSystemWideMessage(string identifier, string content, string sourceId = "", string action = "", string url1 = "", string aggregateId = "", string text1 = "", string text2 = "")
        {
            LoggingService.LogInformation("system message ... " + content, "BaseUserControl.SendSystemWideMessage");
            Messenger.Default.Send<GeneralSystemWideMessage>(new GeneralSystemWideMessage(content) { Identifier = identifier, SourceId = sourceId, Url1 = url1, Action = action, AggregateId = aggregateId, Text1 = text1, Text2 = text2 });
        }

        public void SendInformationNotification(string msg, double duration)
        {

            LoggingService.LogInformation(msg, "BaseUserControl.SendInformationNotification");

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
            double translateXIcon =  - 20,
            double translateYIcon = - 20,
            double scaleIcon = 2,
            GeneralSystemWideMessage msgToPassAlong = null,
            string imageIcon = ""
            )
        {
            LoggingService.LogInformation(question, "BaseUserControl.MessageBoxYesNo");

            //MsgBoxService.Show(
            //    question,
            //    "", 
            //    new SolidColorBrush(Colors.Black),
            //    AccentColor,
            //    new SolidColorBrush(Colors.White),
            //    height: 200,
            //    width: 480,
            //    metroIcon: metroIcon,
            //    scaleIcon: scaleIcon,
            //    translateXIcon: translateXIcon,
            //    translateYIcon: translateYIcon,
            //    yesLabel: yesLabel,
            //    yesMessengerContent: yesMessengerContent,
            //    yesMessengerIdentifier: yesMessengerIdentifier,
            //    noLabel: noLabel,
            //    noMessengerContent: noMessengerContent,
            //    noMessengerIdentifier: noMessengerIdentifier
            //    );

            MsgBoxService.Show(
                question,
                "",
                new SolidColorBrush(Colors.Black),
                AccentColor,
                new SolidColorBrush(Colors.White),
                height: 150,
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

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        public Windows.UI.Xaml.Shapes.Path RetrieveMetroIconPath(string key, Brush iconColor = null)
        {
            LoggingService.LogInformation("Load Metro Icon Path", "BaseUserControl.RetrieveMetroIconPath");

            if (iconColor == null) iconColor = new SolidColorBrush(Windows.UI.Colors.White);

            string temp = (string)Application.Current.Resources[key];
            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + temp + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = iconColor;
            pth.Width = 25;
            pth.Height = 25;

            return pth;

        }

        public string SplitPart(string concatValue,int indexToGet, string splitter = "|")
        {
            string[] parts = concatValue.Split(splitter.ToCharArray());

            if (indexToGet > parts.Length) return parts[parts.Length-1];
            else return parts[indexToGet];
        }



        public async Task<StorageFile> FileExists(string subFolder, string filename, int type = 1)
        {
            var files = await GetFilesAsync(subFolder,2);

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

        public void UnloadControl(){}


        public void Bang(Point p)
        {
            //if (p != null && RenderingService.MagicRenderer != null && RenderingService.MagicRenderer is SumoNinjaMonkey.Framework.Controls.DrawingSurface.ISpriteRenderer)
            //{
            //    ((SumoNinjaMonkey.Framework.Controls.DrawingSurface.ISpriteRenderer)RenderingService.MagicRenderer).AddSprite(p.X, p.Y, 0, 0.15d);
            //}
            //else
            //{
                RenderingService.DoExplosion(p.X, p.Y);
            //}
        }


        public T Deserialize<T>(string json)
        {
            var _Bytes = Encoding.Unicode.GetBytes(json);
            using (MemoryStream _Stream = new MemoryStream(_Bytes))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(T));
                return (T)_Serializer.ReadObject(_Stream);
            }
        }

        public string Serialize(object instance)
        {
            var MinValueUtc = new DateTime(0L, DateTimeKind.Utc);

            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(instance.GetType());
                _Serializer.WriteObject(_Stream, instance);
                _Stream.Position = 0;
                using (StreamReader _Reader = new StreamReader(_Stream))
                { return _Reader.ReadToEnd(); }
            }
        }

        

    }
}
