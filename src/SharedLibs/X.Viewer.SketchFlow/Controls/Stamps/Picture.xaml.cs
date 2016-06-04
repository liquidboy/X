using CoreLib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using X.Services.Extensions;
using X.Viewer.SketchFlow.Controls.Pickers;

namespace X.Viewer.SketchFlow.Controls.Stamps
{


    public sealed partial class Picture : UserControl, IStamp
    {
        public event EventHandler PerformAction;

        public Picture()
        {
            this.InitializeComponent();

            this.Loaded += Picture_Loaded;
            this.Unloaded += Picture_Unloaded;
            cpMain.ColorTypes = new List<string>() { "Stroke", "Fill" };

            tlLeftCenterToolbar.AddTab("Colors", isSelected: true);
            tlLeftCenterToolbar.AddTab("Image");
            tlLeftCenterToolbar.OnTabChanged += TlLeftCenterToolbar_OnTabChanged;
        }

        private void Picture_Loaded(object sender, RoutedEventArgs e)
        {
            LoadPictureLibrary();
        }

        private void TlLeftCenterToolbar_OnTabChanged(object sender, EventArgs e)
        {
            var tab = (X.UI.LiteTab.Tab)sender;

            cpMain.Visibility = Visibility.Collapsed;
            ipMain.Visibility = Visibility.Collapsed;

            switch (tab.Name)
            {
                case "Colors": cpMain.Visibility = Visibility.Visible; break;
                case "Image": ipMain.Visibility = Visibility.Visible; break;
            }
        }

        private void Picture_Unloaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void edges_PerformAction(object sender, EventArgs e)
        {
            var ea = e as ResizeMoveEdgesEventArgs;

            if (ea.ActionType == eActionTypes.ToolbarTopRight)
            {
                if (spToolbar.Visibility == Visibility.Visible) spToolbar.Visibility = Visibility.Collapsed;
                else spToolbar.Visibility = Visibility.Visible;
            }
            else if (ea.ActionType == eActionTypes.CenterLeft) {
                if (leftCenterToolBar.Visibility == Visibility.Visible) leftCenterToolBar.Visibility = Visibility.Collapsed;
                else leftCenterToolBar.Visibility = Visibility.Visible;
            }

            PerformAction?.Invoke(this, e);
        }

        private void butClose_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.CloseStamp } );
        }

        private void butStamp_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.CreateFromStamp });
        }

     

        public void UpdateRotation(double angle)
        {
            ((CompositeTransform)el.RenderTransform).Rotation = angle;
            grdGridRotationMarkers.RotationAngle = angle;
            
        }

        private void cpMain_ColorChanged(object sender, EventArgs e)
        {
            var cpea = e as ColorPickerEventArgs;
            //if(cpea.ColorType == "Stroke") el.Stroke = (Brush)sender;
            //else if (cpea.ColorType == "Fill") el.Fill = (Brush)sender;
        }

        private ImagePickerEventArgs SelectedImage;
        private void ipMain_TextChanged(object sender, EventArgs e)
        {
            SelectedImage = e as ImagePickerEventArgs;
        }

        private void butGridMarker_Click(object sender, RoutedEventArgs e)
        {
            PerformAction?.Invoke(this, new PictureEventArgs() { ActionType = eActionTypes.ToggleGridMarkers });

            if (grdGridMarkers.Visibility == Visibility.Visible) grdGridMarkers.Visibility = Visibility.Collapsed;
            else  grdGridMarkers.Visibility = Visibility.Visible;

            grdGridRotationMarkers.Visibility = grdGridMarkers.Visibility;
        }


        public string GenerateXAML(string uid, double scaleX, double scaleY, double left, double top)
        {
            var rotationAngle = ((CompositeTransform)el.RenderTransform).Rotation;
            var leftToUse = left;
            var topToUse = top;
            var rotationXaml = $"<Image.RenderTransform><CompositeTransform Rotation=\"{ rotationAngle }\" /></Image.RenderTransform>";
            if (rotationAngle == 0) rotationXaml = "";
            var tag = SelectedImage == null ? string.Empty: SelectedImage.Text;
            return $"<Image x:Name=\"{uid}\" HorizontalAlignment=\"Left\" VerticalAlignment=\"Top\" Height=\"{ (this.Height * (1 / scaleY)) }\" Width=\"{ (this.Width * (1 / scaleX)) }\"  Canvas.Left=\"{ leftToUse }\" Canvas.Top=\"{ topToUse }\" RenderTransformOrigin=\"0.5,0.5\" Tag=\"{tag}\" Source=\"{{Binding Tag ,ElementName={uid}, Converter={{StaticResource ExtensionToImageSourceConverter}}}}\" Stretch=\"{ el.Stretch }\" >{ rotationXaml }</Image>";
        }

        public void PopulateFromUIElement(UIElement element)
        {
            throw new NotImplementedException();
        }

        private void butLock_Click(object sender, RoutedEventArgs e)
        {
            butLock.Visibility = Visibility.Collapsed;
            butUnlock.Visibility = Visibility.Visible;
            edges.IsLocked = true;
        }

        private void butUnlock_Click(object sender, RoutedEventArgs e)
        {
            butLock.Visibility = Visibility.Visible;
            butUnlock.Visibility = Visibility.Collapsed;
            edges.IsLocked = false;
        }

        public void GenerateFromXAML(UIElement template)
        {
            if (template is Image) {
                var elTemplate = template as Image;
                try { ((CompositeTransform)el.RenderTransform).Rotation = ((CompositeTransform)elTemplate.RenderTransform).Rotation; } catch { }
                el.Source = elTemplate.Source;
                el.Stretch = elTemplate.Stretch;
                //el.Stroke = elTemplate.Stroke;
                //el.StrokeThickness = elTemplate.StrokeThickness;
                //el.Fill = elTemplate.Fill;
            }
        }

        public void UpdateStrokeThickness(double thickness) { }
        public string GetData() { return string.Empty; }

        public async void LoadPictureLibrary()
        {
            var sender = Services.Extensions.ExtensionsService.Instance as IUWPSender;
            var results = await sender.MakeCall("Spritesheet");
            

            //todo : work out how to move this into a reusable library that pulls content from an extension
            // and lets u use that content .. In this case that content is a SpriteSheet Image and SpriteSheet XMl
            foreach (var result in results) {

                var vsPackageName = ExtensionHelper.GetPropertyFromResults(result, "AppExtensionDisplayName");
                var vsSpriteSheetImg = ExtensionHelper.GetPropertyFromResults(result, "spritesheet-img");
                var vsSpriteSheetXml = ExtensionHelper.GetPropertyFromResults(result, "spritesheet-xml"); 


                if (vsPackageName.Value != null) {
                    var el = sender.FindExtensionLiteInstance((string)vsPackageName.Value);
                    if (vsSpriteSheetImg.Value != null && vsSpriteSheetXml.Value != null)
                    {
                        var packageDirectory = el.AppExtension.Package.InstalledLocation;
                        var publicDirectory = await packageDirectory.GetFolderAsync("public");

                        //xml spriteseet
                        //http://stackoverflow.com/questions/23311287/read-xml-file-from-storage-with-wp8-1-storagefile-api
                        var spriteSheetXmlFile = await publicDirectory.GetFileAsync(vsSpriteSheetXml.Value.ToString());
                        XDocument spriteSheetXml;
                        using (var stream = await spriteSheetXmlFile.OpenReadAsync())
                        {
                            spriteSheetXml = XDocument.Load(stream.AsStreamForRead());
                        }
                        var sprites = ExtensionHelper.GetElement("sprite", spriteSheetXml);

                        //img spritesheet
                        var spriteSheetFile = await publicDirectory.GetFileAsync(vsSpriteSheetImg.Value.ToString());

                        ipMain.LoadData(sprites, spriteSheetFile, (string)vsPackageName.Value);
                    }
                }
            }
        }

    }

    public class PictureEventArgs : EventArgs, IStampEventArgs
    {
        public eActionTypes ActionType { get; set; }
        
    }
}
