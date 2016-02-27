
using FavouriteMX.Shared.Views;
//using SumoNinjaMonkey.Framework.Controls.DrawingSurface;
using System;
using Windows.UI.Xaml.Input;
//using FavouriteMX.Shared.DxRenderer;
using System.Threading.Tasks;


namespace FavouriteMX.Shared.Services
{
    public class RenderingService
    {
        //private static CommonDX.DeviceManager _deviceManager1;
        //private static CommonDX.DeviceManager _deviceManager2;

        //private static IRenderer _renderer1;
        //private static IRenderer _renderer2;

        //private static SumoNinjaMonkey.Framework.Controls.DrawingSurfaceSIS BackgroundSIS;
        //private static SumoNinjaMonkey.Framework.Controls.DrawingSurfaceSIS MagicSIS;
        private static Windows.UI.Xaml.Media.Imaging.BitmapImage BackgroundBitmapImage;
        private static Windows.UI.Xaml.Controls.Image BackgroundImage;
        private static SumoNinjaMonkey.Framework.Controls.Explosions.DrawingSurface ExplosionSurface;

        public static RenderingService Instance = new RenderingService();
        private static GlobalState _state;

        public static bool IsInitialized = false;

        public static Windows.UI.Xaml.FrameworkElement BackgroundControl
        {
            get
            {
                //if (_state.IsSharpDxRendering)
                //    return BackgroundSIS;
                //else
                    return BackgroundImage;
            }
        }

        public static Windows.UI.Xaml.FrameworkElement ForegroundControl
        {
            get
            {
                if (_state.IsSharpDxRendering)
                    return null;
                else
                    return ExplosionSurface;
            }
        }

        //public static IBackgroundRenderer BackgroundRenderer
        //{
        //    get {
        //        if (_renderer1 != null && _renderer1 is IBackgroundRenderer)
        //        {
        //            return (IBackgroundRenderer)_renderer1;
        //        }
        //        else return null;
        //    }
        //}

        //public static ISpriteRenderer MagicRenderer
        //{
        //    get
        //    {
        //        if (_renderer2 != null && _renderer2 is ISpriteRenderer)
        //        {
        //            return (ISpriteRenderer)_renderer2;
        //        }
        //        else return null;
        //    }
        //}


        private RenderingService()
        {
            

        }

        public static void Init(GlobalState state)
        {
             _state = state;

            //if (state != null && state.IsSharpDxRendering)
            //{                
            //    BaseRenderer.UpdateState((BaseRenderer)BackgroundRenderer, state);
            //    BaseRenderer.UpdateState((BaseRenderer)MagicRenderer, state);
            //}

            if (IsInitialized) return;

            //if (state.IsSharpDxRendering)
            //{
            //    _deviceManager1 = new CommonDX.DeviceManager();
            //    _deviceManager2 = new CommonDX.DeviceManager();

            //    _renderer1 = new DxRenderer.BackgroundComposer() { State = _state };
            //    _renderer2 = new DxRenderer.MagicComposer() { State = _state };


            //    BackgroundSIS = new SumoNinjaMonkey.Framework.Controls.DrawingSurfaceSIS(
            //        (gt) => { _renderer1.Update(gt); },
            //        (tb) => { _renderer1.Render(tb); },
            //        (dm) => { _renderer1.Initialize(dm); },
            //        (e1, e2) => { _renderer1.InitializeUI(e1, e2); },
            //        (uri) => { _renderer1.LoadLocalAsset(uri); },
            //        () => { _renderer1.Unload(); },
            //        _deviceManager1);
            //        //_renderer1, _deviceManager1);



            //    MagicSIS = new SumoNinjaMonkey.Framework.Controls.DrawingSurfaceSIS(
            //        (gt) => { _renderer2.Update(gt); },
            //        (tb) => { _renderer2.Render(tb); },
            //        (dm) => { _renderer2.Initialize(dm); },
            //        (e1, e2) => { _renderer2.InitializeUI(e1, e2); },
            //        (uri) => { _renderer2.LoadLocalAsset(uri); },
            //        () => { _renderer2.Unload(); },
            //        _deviceManager2);
            //        //_renderer2, _deviceManager2);
            //}
            //else
            //{
                //FALLBACK TO IMAGE/WRITEABLEBITMAP RENDERING

                BackgroundBitmapImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                BackgroundBitmapImage.UriSource = new Uri(_state.DefaultBackgroundUri);
                BackgroundImage = new Windows.UI.Xaml.Controls.Image();
                BackgroundImage.Source = BackgroundBitmapImage;
                BackgroundImage.Stretch = Windows.UI.Xaml.Media.Stretch.UniformToFill;
                BackgroundImage.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                BackgroundImage.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;


                //FALLBACK TO XAML EXPLOSIONS

                ExplosionSurface = new SumoNinjaMonkey.Framework.Controls.Explosions.DrawingSurface();
                ExplosionSurface.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
                ExplosionSurface.VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch;
            //}

            


            IsInitialized = true;

        }


        public static void Start()
        {
            //if (!IsInitialized) throw new Exception("Renderer needs to be initialized first");
            if (!IsInitialized) return;

            //if (_state.IsSharpDxRendering)
            //{
            //    if (BackgroundSIS != null) BackgroundSIS.IsRunning = true;
            //    if (MagicSIS != null) MagicSIS.IsRunning = true;
            //}
            //else
            //{
            //    //FALLBACK TO IMAGE/WRITEABLEBITMAP RENDERING
            //}


        }

        public static void Stop()
        {

            if (!IsInitialized) return;
 
            //if (_state.IsSharpDxRendering)
            //{
            //    if (BackgroundSIS != null) BackgroundSIS.IsRunning = false;
            //    if (MagicSIS != null) MagicSIS.IsRunning = false;
            //}
            //else
            //{
            //    //FALLBACK TO IMAGE/WRITEABLEBITMAP RENDERING
            //}

        }

        public static void Unload()
        {
            Stop();

            //if (_state.IsSharpDxRendering)
            //{


            //    if (BackgroundSIS != null)
            //    {
            //        BackgroundSIS.Unload();
            //        BackgroundSIS = null;
            //    }

            //    if (MagicSIS != null)
            //    {
            //        MagicSIS.Unload();
            //        MagicSIS = null;
            //    }

            //    if (_renderer1 != null)
            //    {
            //        _renderer1.Unload();
            //        _renderer1 = null;
            //    }

            //    if (_renderer2 != null)
            //    {
            //        _renderer2.Unload();
            //        _renderer2 = null;
            //    }

            //    if (_deviceManager1 != null)
            //    {
            //        _deviceManager1.Dispose();
            //        _deviceManager1 = null;
            //    }

            //    if (_deviceManager2 != null)
            //    {
            //        _deviceManager2.Dispose();
            //        _deviceManager2 = null;
            //    }
            //}
            //else
            //{
                //FALLBACK TO IMAGE/WRITEABLEBITMAP RENDERING
                BackgroundImage.Source = null;
                BackgroundImage = null;
                BackgroundBitmapImage = null;

                ExplosionSurface = null;
                
            //}

            IsInitialized = false;



           //need to do the disposing of the dx surfaces and pipeline here!
        }

        public static void DoExplosion(double x, double y)
        {
            if (ExplosionSurface != null)
            {
                ExplosionSurface.DoExplosion(x - 10, y - 10);
            }
        }

        public static async Task ChangeBackground(string localUri, string folder)
        {
            //if (_state.IsSharpDxRendering)
            //{
            //    var br = RenderingService.BackgroundRenderer;
            //    br.ChangeBackground(localUri, folder);
            //}
            //else
            //{
                string path;
                Windows.Storage.StorageFile storageFile = null;
                if (folder == string.Empty)
                {
                    path = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
                    storageFile = await Windows.Storage.StorageFile.GetFileFromPathAsync(path + localUri);
                }
                else if (folder == "PicturesLibrary")
                {

                    var localUriParts = localUri.Split("\\".ToCharArray());

                    var foundFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync(localUriParts[0]);
                    storageFile = await foundFolder.GetFileAsync(localUriParts[1]);
                }
                else if (folder == "PublicPicturesLibrary")
                {

                    var localUriParts = localUri.Split("\\".ToCharArray());

                    var foundFolder = await Windows.Storage.KnownFolders.PicturesLibrary.GetFolderAsync(localUriParts[0]);
                    storageFile = await foundFolder.GetFileAsync(localUriParts[1]);

                }

                if (storageFile != null)
                {
                    using (var ms = await storageFile.OpenReadAsync())
                    {
                        BackgroundBitmapImage.SetSource(ms);
                        //await BackgroundBitmapImage.SetSourceAsync(ms);  //<== FAILS TO UPDATE IMAGE ON SURFACE RT (GENERATION 1) , 
                                                                           //    doesn't throw an error (appears successful)
                    }
                }
                

            //}

        }
    }
}
