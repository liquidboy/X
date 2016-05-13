using System;
using CoreLib.Extensions;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace X.Extension.ThirdParty.Aws
{
    public static partial class Installer
    {
        public static ExtensionManifest GetManifest() {

            return new ExtensionManifest("Aws Management", "ms-appx:///Extensions/ThirdParty/Aws/Aws.png", "Jose Fajardo", "1.0", "Manage your AWS account", ExtensionInToolbarPositions.Bottom, ExtensionInToolbarPositions.BottomFull) { ContentControl = "X.Extensions.ThirdParty.Aws.Content", AssemblyName= "X.Extensions.ThirdParty.Aws", IsExtEnabled = false };
        }
    }

    public class TestService : IBackgroundTask
    {
        private AppServiceConnection appServiceConnection;

        public void Run(IBackgroundTaskInstance taskInstance)
        {

            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            appServiceConnection = details.AppServiceConnection;
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
        }   

        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var msgDef = args.GetDeferral();
            var msg = args.Request.Message;
            var returnData = new ValueSet();

            returnData.Add("content", new Content());

            await args.Request.SendResponseAsync(returnData);
            msgDef.Complete();
        }
    }
}
