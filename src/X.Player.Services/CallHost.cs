using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace X.Player.Services
{
    public sealed class CallHost : IBackgroundTask
    {
        private BackgroundTaskDeferral backgroundTaskDeferral;
        private AppServiceConnection appServiceConnection;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            backgroundTaskDeferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            appServiceConnection = details.AppServiceConnection;
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            appServiceConnection.ServiceClosed += AppServiceConnection_ServiceClosed;
        }

        private void AppServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            backgroundTaskDeferral?.Complete();
        }

        private async void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var msgDef = args.GetDeferral();
            var msg = args.Request.Message;
            var returnData = new ValueSet();

            var command = msg["Command"] as string;

            switch (command) {
                case "LoadFlickrPhoto":
                    //X.Services.Extensions.ExtensionsService.Instance.SendMessage("LoadFlickrPhoto", ExtensionType.UIComponent);
                    break;
            }
            
            await args.Request.SendResponseAsync(returnData);
            msgDef.Complete();
        }
    }
}
