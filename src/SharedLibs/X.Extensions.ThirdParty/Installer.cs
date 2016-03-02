using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X.Extensions.ThirdParty
{
    public static class Installer
    {
        public static List<ExtensionManifest> GetExtensionManifests() {
            var extensions = new List<ExtensionManifest>();
            
            extensions.Add(new ExtensionManifest("zoo", "ms-appx:///Extensions/ThirdParty/zoo/zoo.png", "Sample Extensions", "1.0", "zoo details", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Bottom));
            extensions.Add(new ExtensionManifest("Stitcher", "ms-appx:///Extensions/ThirdParty/Stitcher/Stitcher.png", "Sample Extensions", "1.0", "Stitcher xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Bottom));
            extensions.Add(new ExtensionManifest("Spotify", "ms-appx:///Extensions/ThirdParty/Spotify/Spotify.png", "Sample Extensions", "1.0", " Spotify xxxx", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Pixta", "ms-appx:///Extensions/ThirdParty/Pixta/Pixta.png", "Sample Extensions", "1.0", "Pixta xxxx", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("F12", "ms-appx:///Extensions/ThirdParty/F12/F12.png", "Sample Extensions", "1.0", "f12 xxxxx", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull));
            extensions.Add(new ExtensionManifest("Songist", "ms-appx:///Extensions/ThirdParty/Songist/Songist.png", "Sample Extensions", "1.0", "Songist xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right));
            //extensions.Add(new SmartHeader()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new SecureShell()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new Quant()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Plex", "ms-appx:///Extensions/ThirdParty/Plex/Plex.png", "Sample Extensions", "1.0", "Plex xxxx ", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("LastPass", "ms-appx:///Extensions/ThirdParty/LastPass/LastPass.png", "Sample Extensions", "1.0", "LastPass xxxx", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Bottom));
            //extensions.Add(new Koding()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new HistoryEraser()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new Feedly()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Duo Lingo", "ms-appx:///Extensions/ThirdParty/DuoLingo/DuoLingo.png", "Sample Extensions", "1.0", "DuoLingo language service", ExtensionInToolbarPositions.Left, ExtensionInToolbarPositions.Bottom));
            extensions.Add(new ExtensionManifest("Adblocker Plus", "ms-appx:///Extensions/ThirdParty/ABP/abp.png", "Sample Extensions", "1.0", "Block adverts in the page", ExtensionInToolbarPositions.Top | ExtensionInToolbarPositions.Left, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Advanced Rest Client", "ms-appx:///Extensions/ThirdParty/AdvancedRestClient/AdvancedRestClient.png", "Sample Extensions", "1.0", "A rest client for testing rest services", ExtensionInToolbarPositions.Top | ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Assistant To", "ms-appx:///Extensions/ThirdParty/AssistantTo/AssistantTo.png", "Sample Extensions", "1.0", "personal assistant for managing your web browsing habits", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Black Menu", "ms-appx:///Extensions/ThirdParty/BlackMenu/BlackMenu.png", "Sample Extensions", "1.0", "List of Google services served up in a black menu", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Color Hexa", "ms-appx:///Extensions/ThirdParty/ColorHexa/ColorHexa.png", "Sample Extensions", "1.0", "Configure Color Hex values for the selected site", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Draw IO", "ms-appx:///Extensions/ThirdParty/DrawIO/DrawIO.png", "Sample Extensions", "1.0", "Sample Drawing extension", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Right));
            extensions.Add(new ExtensionManifest("DuckDuckGo", "ms-appx:///Extensions/ThirdParty/DuckDuckGo/DuckDuckGo.png", "Sample Extensions", "1.0", "Search engine duck duck go ...", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("AwesomeScreenshot", "ms-appx:///Extensions/ThirdParty/AwesomeScreenshot/AwesomeScreenshot.png", "Sample Extensions", "1.0", "take great screenshots of any website", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Right));
            extensions.Add(new ExtensionManifest("Google Hangouts", "ms-appx:///Extensions/ThirdParty/GoogleHangouts/GoogleHangouts.png", "Community chat boards", "1.0", "xxxx", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Chrometana", "ms-appx:///Extensions/ThirdParty/Chrometana/Chrometana.png", "Sample Extensions", "1.0", "Cortana as a google nacl extension", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Google Keep", "ms-appx:///Extensions/ThirdParty/GoogleKeep/GoogleKeep.png", "Sample Extensions", "1.0", "Keep your personal information safe from Googles searches", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Bottom));
            extensions.Add(new ExtensionManifest("Html Fire", "ms-appx:///Extensions/ThirdParty/HtmlFire/HtmlFire.png", "Sample Extensions", "1.0", "Quickly optimize HTMl within a chosen website", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Little Alchemy", "ms-appx:///Extensions/ThirdParty/LittleAlchemy/LittleAlchemy.png", "Sample Extensions", "1.0", "Cool little game", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Nitrous", "ms-appx:///Extensions/ThirdParty/Nitrous/Nitrous.png", "Sample Extensions", "1.0", "Nitrous game engine", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right));
            extensions.Add(new ExtensionManifest("OneNote", "ms-appx:///Extensions/ThirdParty/OneNote/OneNote.png", "Sample Extensions", "1.0", "Microsoft Office OneNote", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right));
            extensions.Add(new ExtensionManifest("ooVoo", "ms-appx:///Extensions/ThirdParty/ooVoo/ooVoo.png", "Sample Extensions", "1.0", "ooVoo video player", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Top));
            extensions.Add(new ExtensionManifest("Padlet", "ms-appx:///Extensions/ThirdParty/Padlet/Padlet.png", "Sample Extensions", "1.0", "Playing with colors , schemes and themes", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Top));
            extensions.Add(new ExtensionManifest("Pixlr", "ms-appx:///Extensions/ThirdParty/Pixlr/Pixlr.png", "Sample Extensions", "1.0", "Pixlr the pixel manipulation tool", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Right));
            extensions.Add(new ExtensionManifest("Polarr", "ms-appx:///Extensions/ThirdParty/Polarr/Polarr.png", "Sample Extensions", "1.0", "Polarize your image", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("RatingsPreview", "ms-appx:///Extensions/ThirdParty/RatingsPreview/RatingsPreview.png", "Sample Extensions", "1.0", "View your Youtubes ratings and review them...", ExtensionInToolbarPositions.Right, ExtensionInToolbarPositions.Bottom));
            //extensions.Add(new Rdio()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new SessionBuddy()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new SketchboardIO()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new Storybird()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            extensions.Add(new ExtensionManifest("Teamviewer", "ms-appx:///Extensions/ThirdParty/Teamviewer/Teamviewer.png", "Sample Extensions", "1.0", "Teamviewer vpn access to all your devices", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new Twerk()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new WordOnline()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new WorkFlowy()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));
            //extensions.Add(new wunderlist()); _Template(new ExtensionManifest("xxxx", "ms-appx:///Extensions/ThirdParty/xxxx/xxxx.png", "Sample Extensions", "1.0", "xxxx", ExtensionInToolbarPositions.Top, ExtensionInToolbarPositions.Left));


            return extensions;
        }
    }
}
