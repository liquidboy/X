using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using X.Extensions.Popups;
using System.Reflection;
using WeakEvent;

namespace X.Services.Extensions
{


    public class ExtensionsService : ISender, IExtensionsService
    {

        class ExtensionLite {
            public IExtensionManifest Manifest;
            public ExtensionType ExtensionType;
            public IExtension Extension;
            public bool IsShowingExtensionPanel;
        }

        List<ExtensionLite> _extensions = new List<ExtensionLite>();

        private readonly WeakEventSource<EventArgs> _OnInstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnInstallExtension
        {
            add { _OnInstallExtensionSource.Subscribe(value); }
            remove { _OnInstallExtensionSource.Unsubscribe(value); }
        }

        private readonly WeakEventSource<EventArgs> _OnDidInstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnDidInstallExtension
        {
            add { _OnDidInstallExtensionSource.Subscribe(value); }
            remove { _OnDidInstallExtensionSource.Unsubscribe(value); }
        }
        
        private readonly WeakEventSource<EventArgs> _OnUninstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnUninstallExtension
        {
            add { _OnUninstallExtensionSource.Subscribe(value); }
            remove { _OnUninstallExtensionSource.Unsubscribe(value); }
        }

        private readonly WeakEventSource<EventArgs> _OnDidUninstallExtensionSource = new WeakEventSource<EventArgs>();
        public event EventHandler<EventArgs> OnDidUninstallExtension
        {
            add { _OnDidUninstallExtensionSource.Subscribe(value); }
            remove { _OnDidUninstallExtensionSource.Unsubscribe(value); }
        }


        public ExtensionsService() {
            CreateDefaultExtensions();
        }


        public void SendMessage(object msg, ExtensionType type)
        {
            var foundExts = _extensions.Where(x => x.ExtensionType == type);
            foreach (var ext in foundExts) {
                ext.Extension.RecieveMessage(msg);
            }
        }



        private void CreateDefaultExtensions() {
            Install(new HandleNewWindowAsInlineLink()).Wait();
            Install(new HandleNavigationFailedAsInlineToast()).Wait();
            Install(new OSToast()).Wait();
        }

        public dynamic GetExtensionMetadata(Guid guid)
        {
            foreach (var el in _extensions)
            {
                if (el.Manifest.UniqueID == guid) {

                    dynamic item = new ExpandoObject();

                    foreach (var prop in el.Manifest.GetType().GetRuntimeProperties())
                    {
                        var val = prop.GetValue(el.Manifest);
                        if (val != null) ((IDictionary<string, object>)item).Add(prop.Name, val.ToString());
                    }

                    return item;
                }
            }

            return null;
        }

        public List<dynamic> GetExtensionsMetadata() {

            List<dynamic> ret = new List<dynamic>();

            //=========================================================================================
            //we statically define what of an extensions 'properties' to bring across into a dynamic object 
            //that xaml will use for binding (very brittle and requires lots of maintenance)
            //=========================================================================================
            //foreach (var el in _extensions) {
            //    dynamic item = new ExpandoObject();
            //    item.Title = el.Title;
            //    item.Abstract = el.Abstract;
            //    item.CanUninstall = el.CanUninstall;
            //    item.IsExtEnabled = el.IsExtEnabled;
            //    item.Version = el.Version;
            //    item.Publisher = el.Publisher;
            //    ret.Add(item);
            //}


            //=========================================================================================
            //we will "dynamically" bring across ALL properties defined in the extension for use in
            //binding in xaml (less brittle)
            //=========================================================================================
            foreach (var el in _extensions)
            {
                dynamic item = new ExpandoObject();

                foreach (var prop in el.Manifest.GetType().GetRuntimeProperties())
                {
                    var val = prop.GetValue(el.Manifest);
                    if (val != null) ((IDictionary<string, object>)item).Add(prop.Name, val.ToString());
                }

                ret.Add(item);
            }

            return ret;
        }

        public List<dynamic> GetToolbarExtensionsMetadata(ExtensionInToolbarPositions position)
        {

            List<dynamic> ret = new List<dynamic>();

            //=========================================================================================
            //we will "dynamically" bring across ALL properties defined in the extension for use in
            //binding in xaml (less brittle)
            //=========================================================================================
            foreach (var el in _extensions)
            {

                if (((int)el.Manifest.FoundInToolbarPositions & (int)position) > 0)
                {
                    dynamic item = new ExpandoObject();

                    foreach (var prop in el.Manifest.GetType().GetRuntimeProperties())
                    {
                        try {
                            var val = prop.GetValue(el.Manifest);
                            if (val != null) ((IDictionary<string, object>)item).Add(prop.Name, val.ToString());
                        }
                        catch (Exception ex) {
                            //should never get here! (BUT IT DOES BECAUSE OF .NETNative toolchain)
                        }
                    }

                    ret.Add(item);
                }
            }

            return ret;
        }


        private void DoSendMessage(object sender, EventArgs e)
        {
            var senderExt = (IExtension)sender;
            IEnumerable<ExtensionLite> foundExts = null;
            Guid guidToUse = senderExt.ExtensionManifest.UniqueID;

            //if (e is CloseExtensionEventArgs) guidToUse = ((CloseExtensionEventArgs)e).ExtensionUniqueGuid;

            if (e is IReceiver)
                foundExts = _extensions.Where(x => x.ExtensionType == ((IReceiver)e).ReceiverType && x.Manifest?.UniqueID != guidToUse);

            if (foundExts != null) foreach (var ext in foundExts) ext.Extension?.RecieveMessage(e);

        }


        private async Task<bool> validate(string zipPath, IExtension extension) {

            return true;
        }

        public void UnloadExtensions() {
            foreach (var ext in _extensions)
            {
                ext.Extension.SendMessage -= DoSendMessage;
            }

            _extensions.Clear();
        }


        public async Task<IExtensionManifest> Install(IExtensionManifest extension)
        {
            if (extension == null) return null;
            //await validate(string.Empty, extension);
            //OnInstallExtension?.Invoke(extension, EventArgs.Empty);
            //extension.SendMessage += DoSendMessage;
            _extensions.Add(new ExtensionLite() { Extension = null, Manifest = extension, ExtensionType = ExtensionType.UIComponentLazy });
            //OnDidInstallExtension?.Invoke(extension, EventArgs.Empty);
            return extension;
        }

        public async Task<IExtension> Install(IExtension extension)
        {
            await validate(string.Empty, extension);
            _OnInstallExtensionSource?.Raise(extension, EventArgs.Empty);
            extension.SendMessage += DoSendMessage;
            _extensions.Add(new ExtensionLite() { Extension = extension, Manifest = extension.ExtensionManifest, ExtensionType = extension.ExtensionType });
            _OnDidInstallExtensionSource?.Raise(extension, EventArgs.Empty);
            return extension;
        }

        public object CreateInstance(dynamic md)
        {
            //var ctlName = md.BaseUri;
            //ctlName = ctlName.Replace("ms-appx:///", "");
            //var parts = ctlName.Split("/".ToCharArray());
            //parts[parts.Length - 1] = "";
            //ctlName = string.Join(".", parts);
            //ctlName = ctlName.Substring(0, ctlName.Length - 1);
            //ctlName = "X." + ctlName;
            //var type = Type.GetType((string)ctlName);
            //var newEl = Activator.CreateInstance(type);
            //var newElExt = (IExtension)newEl;

            var found = _extensions.Where(x => x.Manifest.UniqueID.ToString() == md.UniqueID).FirstOrDefault();

            if (found.Extension == null)
            {
                ExtensionInToolbarPositions launchPosition = GetExtensionPositionFromString(md.LaunchInDockPositions);
                var newElExt = new Services.ThirdParty._Template(found.Manifest);
                newElExt.SendMessage += DoSendMessage;

                if (launchPosition == ExtensionInToolbarPositions.Left || launchPosition == ExtensionInToolbarPositions.Right)
                    newElExt.Width = 350;
                else newElExt.Height = 200;

                found.Extension = newElExt;
            }

            if (found.Extension != null && found.IsShowingExtensionPanel) return null;

            found.IsShowingExtensionPanel = true;

            return found.Extension;
        }

        private ExtensionInToolbarPositions GetExtensionPositionFromString(string position) {
            if (ExtensionInToolbarPositions.Bottom.ToString() == position)
            {
                return ExtensionInToolbarPositions.Bottom;
            }
            else if (ExtensionInToolbarPositions.BottomFull.ToString() == position)
            {
                return ExtensionInToolbarPositions.BottomFull;
            }
            else if (ExtensionInToolbarPositions.Left.ToString() == position)
            {
                return ExtensionInToolbarPositions.Left;
            }
            if (ExtensionInToolbarPositions.None.ToString() == position)
            {
                return ExtensionInToolbarPositions.None;
            }
            if (ExtensionInToolbarPositions.Right.ToString() == position)
            {
                return ExtensionInToolbarPositions.Right;
            }
            if (ExtensionInToolbarPositions.Top.ToString() == position)
            {
                return ExtensionInToolbarPositions.Top;
            }
            else
                return ExtensionInToolbarPositions.Left;

        }

        public IExtension Install(string zipPath)
        {
            throw new NotImplementedException();
        }

        public void UnInstall(IExtension extension)
        {
            _OnUninstallExtensionSource?.Raise(extension, EventArgs.Empty);
            extension?.OnPaneUnload();
            extension.SendMessage -= DoSendMessage;
            _extensions.Remove(_extensions.Where(x=>x.Manifest.UniqueID == extension.ExtensionManifest.UniqueID).FirstOrDefault());
            _OnDidUninstallExtensionSource?.Raise(extension, EventArgs.Empty);
        }

        public bool UninstallInstance(Guid instanceUniqueId)
        {

            var found = _extensions.Where(x => x.Manifest.UniqueID == instanceUniqueId).FirstOrDefault();
            if (found.Extension != null) {
                found.Extension.OnPaneUnload();
                found.Extension.ExtensionManifest = null;
                found.Extension.SendMessage -= DoSendMessage;
                found.Extension = null;
                found.IsShowingExtensionPanel = false;
            }

            //foreach (var el in _extensions)
            //{
            //    if (el.Extension?.ExtensionManifest.UniqueID == instanceUniqueId)
            //    {
            //        el.Extension.SendMessage -= DoSendMessage;
            //        el.Extension.ExtensionManifest = null;
            //        _extensions.Remove(el);
            //        break;
            //    }
            //}
            return true;
        }

        public IExtension[] GetInstalled()
        {
            throw new NotImplementedException();
        }

        public void UpdateExtension(IExtensionManifest manifest) {
            var found = _extensions.Where(x => x.Manifest.UniqueID.ToString() == manifest.UniqueID.ToString()).FirstOrDefault();
            if (found != null) {
                found.Manifest.IsExtEnabled = manifest.IsExtEnabled;
                found.Manifest.LaunchInDockPositions = manifest.LaunchInDockPositions;
                found.Manifest.FoundInToolbarPositions = manifest.FoundInToolbarPositions;
            }
        }







        //=========================
        //singleton
        //=========================
        private static ExtensionsService instance;

        public static ExtensionsService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ExtensionsService();
                }
                return instance;
            }
        }
    }
}
