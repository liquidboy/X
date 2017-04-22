using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace SumoNinjaMonkey.Framework.Services
{
    public class ErrorService
    {
        public static bool exceptionHandled;
        public async void HandleException(string userFriendlyMessage, Exception ex)
        {
            if (ErrorService.exceptionHandled)
            {
                return;
            }
            MessageDialog messageDialog = new MessageDialog("An error has occurred :" + userFriendlyMessage, "Error occured");
            UICommand item = new UICommand("Close", delegate(IUICommand e)
            {
                ErrorService.exceptionHandled = false;
            }
            );
            messageDialog.Commands.Add(item);
            await messageDialog.ShowAsync();
            ErrorService.exceptionHandled = true;
        }
    }
}
