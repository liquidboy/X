using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

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
            tileTextAttributes[0].InnerText = text;
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




        
    }
}
