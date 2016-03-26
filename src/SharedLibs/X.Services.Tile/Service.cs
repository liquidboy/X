using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace X.Services.Tile
{
    public class Service
    {
        //https://msdn.microsoft.com/en-us/library/windows/apps/xaml/hh868253.aspx
        
        public static void UpdatePrimaryTile(string text, string imgSrc, string imgAlt, TileTemplateType templateType) {

            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(templateType);  //TileWide310x150ImageAndText01

            //version
            XmlNodeList tileVersionAttributes = tileXml.GetElementsByTagName("visual");
            ((XmlElement)tileVersionAttributes[0]).SetAttribute("version", "2");
            //tileXml.FirstChild.FirstChild.Attributes[0].NodeValue = "2";

            //text
            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");
            if(tileTextAttributes.Count > 0) tileTextAttributes[0].InnerText = text;
            //tileTextAttributes[1].InnerText = "xxxxx";

            //image
            XmlNodeList tileImageAttributes = tileXml.GetElementsByTagName("image");

            ////image from apps package
            //((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/redWide.png");
            //((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");

            ////image from apps local storage
            //((XmlElement)tileImageAttributes[0]).SetAttribute("src", "ms-appdata:///local/redWide.png");
            //((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");
            ((XmlElement)tileImageAttributes[0]).SetAttribute("src", imgSrc);
            ((XmlElement)tileImageAttributes[0]).SetAttribute("alt", imgAlt);

            ////image from the web
            //((XmlElement)tileImageAttributes[0]).SetAttribute("src", "http://www.contoso.com/redWide.png");
            //((XmlElement)tileImageAttributes[0]).SetAttribute("alt", "red graphic");

            //create notifcation
            //TileNotification tileNotification = new TileNotification(tileXml);
            ScheduledTileNotification stn = new ScheduledTileNotification(tileXml, DateTimeOffset.Now.AddSeconds(8));

            //notification expires in 
            //tileNotification.ExpirationTime = DateTimeOffset.UtcNow.AddMinutes(10);

            //forced clear notification for app
            //Windows.UI.Notifications.TileUpdateManager.CreateTileUpdaterForApplication().Clear();

            //send notification to app tile
            //TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            TileUpdateManager.CreateTileUpdaterForApplication().AddToSchedule(stn);

        }

        public static async void UpdateSecondaryTile(string guid, string text, string icon, string imgSrc150x150, string imgSrc310x150, string imgSrc310x310) {

            if (Windows.UI.StartScreen.SecondaryTile.Exists(guid))
            {
                SecondaryTile secondaryTile = new SecondaryTile(guid);

                secondaryTile.VisualElements.Square150x150Logo = new Uri(imgSrc150x150);
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;

                secondaryTile.VisualElements.Wide310x150Logo = new Uri(imgSrc310x150);
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;

                secondaryTile.VisualElements.Square310x310Logo = new Uri(imgSrc310x310);
                secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;

                bool isUpdated = await secondaryTile.UpdateAsync();
            }
            else {

                // During creation of secondary tile, an application may set additional arguments on the tile that will be passed in during activation.
                // These arguments should be meaningful to the application. In this sample, we'll pass in the date and time the secondary tile was pinned.
                string tileActivationArguments = guid + " TabWasPinnedAt=" + DateTime.Now.ToLocalTime().ToString();

                // Create a Secondary tile with all the required arguments.
                // Note the last argument specifies what size the Secondary tile should show up as by default in the Pin to start fly out.
                // It can be set to TileSize.Square150x150, TileSize.Wide310x150, or TileSize.Default.  
                // If set to TileSize.Wide310x150, then the asset for the wide size must be supplied as well.
                // TileSize.Default will default to the wide size if a wide size is provided, and to the medium size otherwise. 
                SecondaryTile secondaryTile = new SecondaryTile(guid, text, tileActivationArguments, new Uri(imgSrc150x150),  TileSize.Square150x150);
                secondaryTile.BackgroundColor = Windows.UI.Colors.Black;
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;

                secondaryTile.VisualElements.Wide310x150Logo = new Uri(imgSrc310x150);
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;

                secondaryTile.VisualElements.Square310x310Logo = new Uri(imgSrc310x310);
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
                
                bool isPinned = await secondaryTile.RequestCreateAsync();

            }
        }


        public static async void DeleteSecondaryTile(string guid)
        {
            if (Windows.UI.StartScreen.SecondaryTile.Exists(guid))
            {
                SecondaryTile secondaryTile = new SecondaryTile(guid);
                await secondaryTile.RequestDeleteAsync();
            }
        }
    }
}
