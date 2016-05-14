


using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
namespace X.CoreLib.Shared.Services
{
    public class TileService
    {

        private static TileService Instance = new TileService();

        private static bool _isInitialized = false;

        public enum TileType
        {
            Contact,
            Project,
            Message,
            Attachment,
            Form,
            Filter,
            Archiver,
            DistributionGroup,
            BulkPrint,
            Import,
            ComposeMessage,
            Help,
            Portal,
            FormAdmin,
            Company,
            Passport,
            User
        }



        private TileService()
        {
 
        }

        public static void Init()
        {
            if (_isInitialized) return;
            

            _isInitialized = true;
        }

        public static void Start()
        {
            if (!_isInitialized) throw new Exception("TileService needs to be initialized first");


        }

        public static void Stop()
        {

        }

        public static void Unload()
        {
           //need to do the disposing of the dx surfaces and pipeline here!
        }


        public static async Task<bool> CreateSecondaryTile(Rect confirmationRectShownAt, TileType type, string shortName, string displayName, string tileIdToUse)
        {

            string logoImage = "";

            switch (type)
            {
                case TileType.Contact: logoImage = "contact"; break;
                case TileType.Attachment: logoImage = "attachment"; break;
                case TileType.Form: logoImage = "form"; break;
                case TileType.Message: logoImage = "message"; break;
                case TileType.Project: logoImage = "project"; break;
                case TileType.Company: logoImage = "company"; break;
                case TileType.User: logoImage = "user"; break;
            }

            Uri logo = new Uri("ms-appx:///Assets/logo-" + logoImage + ".png");
            Uri wideLogo = new Uri("ms-appx:///Assets/Logo-wide-" + logoImage + ".png");


            string dynamicTileId = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(tileIdToUse)) dynamicTileId = tileIdToUse;

            string tileActivationArguments = dynamicTileId +
                " WasPinnedAt=" + DateTime.Now.ToLocalTime().ToString();

            var secondaryTile = new SecondaryTile(dynamicTileId,
             shortName,
             displayName,
             tileActivationArguments,
             TileOptions.ShowNameOnLogo | TileOptions.ShowNameOnWideLogo,
             logo,
             wideLogo);

            bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(confirmationRectShownAt, Windows.UI.Popups.Placement.Below);

            return isPinned;
        }

        public static async Task<bool> CreateSecondaryTile(Rect confirmationRectShownAt, TileType type,  string shortName, string displayName)
        {
            return await CreateSecondaryTile(confirmationRectShownAt, type, shortName, displayName, string.Empty);


        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

    }
}
