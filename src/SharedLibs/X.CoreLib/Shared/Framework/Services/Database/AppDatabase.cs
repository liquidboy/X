
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using X.CoreLib.SQLite;
using SumoNinjaMonkey.Framework;
using SumoNinjaMonkey.Framework.Controls.Messages;
using SumoNinjaMonkey.Framework.Services;
using System.Linq;
using Windows.Foundation;
using System;
using System.Runtime.Serialization;


namespace X.CoreLib.Shared.Services
{
    public partial class AppDatabase : SqliteDatabase
    {
        private static AppDatabase database = null;

        public static AppDatabase Current
        {
            get
            {
                AppDatabase result;
                lock (SqliteDatabase.lockobj)
                {
                    if (AppDatabase.database == null)
                    {
                        AppDatabase.database = new AppDatabase();
                    }
                    result = AppDatabase.database;
                }
                return result;
            }
        }



        
        private AppDatabase()
            : base("App.db")
        {

            this.SqliteDb.CreateTable<TableDasboard>();
            this.SqliteDb.CreateTable<FolderItem>();
            this.SqliteDb.CreateTable<MenuItem>();
            this.SqliteDb.CreateTable<AppState>();
            this.SqliteDb.CreateTable<UIElementState>();
            this.SqliteDb.CreateTable<Solution>();
            this.SqliteDb.CreateTable<Project>();
            this.SqliteDb.CreateTable<Scene>();
            this.SqliteDb.CreateTable<CacheCallResponse>();
            this.SqliteDb.CreateTable<SearchRequest>();
            this.SqliteDb.CreateTable<GroupsRequest>();

            this.SqliteDb.CreateTable<YoutubePersistedItem>();
            this.SqliteDb.CreateTable<YoutubeHistoryItem>();


            //this.SqliteDb.DropTable<TwitterPersistedItem>();
            this.SqliteDb.CreateTable<TwitterPersistedItem>();
        }


        public void Unload()
        {
            
            database.SqliteDb.Close();
            database.SqliteDb.Dispose();
            database = null;

        }











        #region INSTANCES
        public List<AppState> AppStates { get; set; }


        public void LoadInstances()
        {
            this.AppStates = RetrieveAppStates();
        }
        public AppState RetrieveInstanceAppState(AppSystemDataEnums appSystemData)
        {
            var found = AppStates.Where(x => x.Name == ((int)appSystemData).ToString());
            return found.FirstOrDefault();
        }
        public void UpdateInstanceAppState(AppSystemDataEnums appSystemData, string value)
        {
            var found = RetrieveInstanceAppState(appSystemData);
            
            if(found!=null){
                found.Value = value;
                this.SqliteDb.Update(found);
            }
        }

        #endregion

        #region SYSTEMDATA

        public enum AppSystemDataEnums
        {
            PrimaryAccentColor = 100,
            SecondaryAccentColor = 101,
            ThirdAccentColor = 102,

            PrimaryBackgroundColor = 200,
            SecondaryBackgroundColor = 201,
            ThirdBackgroundColor = 202,

            UserSessionID = 1000,
        }



        public void RecreateSystemData()
        {

            var o = this.AppStates.Count();

            this.SqliteDb.Query<AppState>("DELETE FROM AppState");

            AddAppState(((int)AppSystemDataEnums.PrimaryAccentColor).ToString(), "39,118,255,255");  //R,G,B,A
            AddAppState(((int)AppSystemDataEnums.SecondaryAccentColor).ToString(), "255,0,185,255"); //R,G,B,A
            AddAppState(((int)AppSystemDataEnums.ThirdAccentColor).ToString(), "0,255,23,255"); //R,G,B,A


            AddAppState(((int)AppSystemDataEnums.PrimaryBackgroundColor).ToString(), "255,255,255,255");  //R,G,B,A
            AddAppState(((int)AppSystemDataEnums.SecondaryBackgroundColor).ToString(), "238,238,238,255"); //R,G,B,A
            AddAppState(((int)AppSystemDataEnums.ThirdBackgroundColor).ToString(), "224,224,224,255"); //R,G,B,A


            //new session
            string sid =  Guid.NewGuid().ToString();
            AddAppState(((int)AppSystemDataEnums.UserSessionID).ToString(), sid);
            if(Windows.UI.Xaml.Window.Current!=null)
                AddUpdateUIElementState(sid, "", 0, 0, Windows.UI.Xaml.Window.Current.Bounds.Width, Windows.UI.Xaml.Window.Current.Bounds.Height, 1, false, null, null); 
            else
                AddUpdateUIElementState(sid, "", 0, 0, 1366, 768, 1, false, null, null); 

            LoadInstances();

        }
        #endregion

        #region LAYOUT DATA
        public List<LayoutDetail> GetLayoutDetails()
        {
            List<LayoutDetail> layoutDetails = new List<LayoutDetail>();

            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 1366, 768), Width = 1366, Height = 768, Dpi = 148, Scale = 140, Title = "1366 x 768", MetroIcon = "Display1", DisplaySize = "10.6'" });
            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 1920, 1080), Width = 1920, Height = 1080, Dpi = 207, Scale = 140, Title = "1920 x 1080", MetroIcon = "Display1", DisplaySize = "10.6'" });
            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 2560, 1440), Width = 2560, Height = 1440, Dpi = 277, Scale = 180, Title = "2560 x 1440", MetroIcon = "Display1", DisplaySize = "10.6'" });
            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 1280, 800), Width = 1280, Height = 800, Dpi = 125, Scale = 100, Title = "1280 x 800", MetroIcon = "Display1", DisplaySize = "12'" });
            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 1920, 1080), Width = 1920, Height = 1080, Dpi = 96, Scale = 100, Title = "1920 x 1080", MetroIcon = "Display1", DisplaySize = "23'" });
            layoutDetails.Add(new LayoutDetail() { Dimension = new Rect(0, 0, 2560, 1440), Width = 2560, Height = 1440, Dpi = 109, Scale = 100, Title = "2560 x 1440", MetroIcon = "Display1", DisplaySize = "27'" });

            return layoutDetails;

        }

        public List<LayoutOrientation> GetLayoutOrientations()
        {
            List<LayoutOrientation> layoutOrientations = new List<LayoutOrientation>();
            layoutOrientations.Add(new LayoutOrientation() { Title = "Landscape", MetroIcon = "Landscape1" });
            layoutOrientations.Add(new LayoutOrientation() { Title = "Portrait", MetroIcon = "Portrait1" });

            return layoutOrientations;
        }

        public LayoutDetail GetLayoutDetail(int index)
        {
            return GetLayoutDetails()[index];
        }

        public LayoutOrientation GetLayoutOrientation(int index)
        {
            return GetLayoutOrientations()[index];
        }
        #endregion

        #region EFFECT DATA
        public List<EffectDetail> GetEffectDetails()
        {
            List<EffectDetail> ret = new List<EffectDetail>();

            ret.Add(new EffectDetail() { Title = "Affine Transform 2D", Class = "SharpDX.Direct2D1.Effects.AffineTransform2D" });
            ret.Add(new EffectDetail() { Title = "Arithmetic Composite", Class = "SharpDX.Direct2D1.Effects.ArithmeticComposite" });
            ret.Add(new EffectDetail() { Title = "Atlas", Class = "SharpDX.Direct2D1.Effects.Atlas" });
            ret.Add(new EffectDetail() { Title = "BitmapSource Effect", Class = "SharpDX.Direct2D1.Effects.BitmapSourceEffect" });
            ret.Add(new EffectDetail() { Title = "Blend", Class = "SharpDX.Direct2D1.Effects.Blend" });
            ret.Add(new EffectDetail() { Title = "Border", Class = "SharpDX.Direct2D1.Effects.Border" });
            ret.Add(new EffectDetail() { Title = "Brightness", Class = "SharpDX.Direct2D1.Effects.Brightness" });
            ret.Add(new EffectDetail() { Title = "Color Management", Class = "SharpDX.Direct2D1.Effects.ColorManagement" });
            ret.Add(new EffectDetail() { Title = "Color Matrix", Class = "SharpDX.Direct2D1.Effects.ColorMatrix" });
            ret.Add(new EffectDetail() { Title = "Composite", Class = "SharpDX.Direct2D1.Effects.Composite" });
            ret.Add(new EffectDetail() { Title = "Convolve Matrix", Class = "SharpDX.Direct2D1.Effects.ConvolveMatrix" });
            ret.Add(new EffectDetail() { Title = "Crop", Class = "SharpDX.Direct2D1.Effects.Crop" });
            ret.Add(new EffectDetail() { Title = "Directional Blur", Class = "SharpDX.Direct2D1.Effects.DirectionalBlur" });
            ret.Add(new EffectDetail() { Title = "Discrete Transfer", Class = "SharpDX.Direct2D1.Effects.DiscreteTransfer" });
            ret.Add(new EffectDetail() { Title = "Displacement Map", Class = "SharpDX.Direct2D1.Effects.DisplacementMap" });
            ret.Add(new EffectDetail() { Title = "Distant Diffuse", Class = "SharpDX.Direct2D1.Effects.DistantDiffuse" });
            ret.Add(new EffectDetail() { Title = "Distant Specular", Class = "SharpDX.Direct2D1.Effects.DistantSpecular" });
            ret.Add(new EffectDetail() { Title = "Dpi Compensation", Class = "SharpDX.Direct2D1.Effects.DpiCompensation" });
            ret.Add(new EffectDetail() { Title = "Flood", Class = "SharpDX.Direct2D1.Effects.Flood" });
            ret.Add(new EffectDetail() { Title = "Gamma Transfer", Class = "SharpDX.Direct2D1.Effects.GammaTransfer" });
            ret.Add(new EffectDetail() { Title = "Gaussian Blur", Class = "SharpDX.Direct2D1.Effects.GaussianBlur" });
            ret.Add(new EffectDetail() { Title = "Histogram", Class = "SharpDX.Direct2D1.Effects.Histogram" });
            ret.Add(new EffectDetail() { Title = "Hue Rotate", Class = "SharpDX.Direct2D1.Effects.HueRotate" });
            ret.Add(new EffectDetail() { Title = "Linear Transfer", Class = "SharpDX.Direct2D1.Effects.LinearTransfer" });
            ret.Add(new EffectDetail() { Title = "Luminance To Alpha", Class = "SharpDX.Direct2D1.Effects.LuminanceToAlpha" });
            ret.Add(new EffectDetail() { Title = "Morphology", Class = "SharpDX.Direct2D1.Effects.Morphology" });
            ret.Add(new EffectDetail() { Title = "Namespace Doc", Class = "SharpDX.Direct2D1.Effects.NamespaceDoc" });
            ret.Add(new EffectDetail() { Title = "Point Diffuse", Class = "SharpDX.Direct2D1.Effects.PointDiffuse" });
            ret.Add(new EffectDetail() { Title = "Point Specular", Class = "SharpDX.Direct2D1.Effects.PointSpecular" });
            ret.Add(new EffectDetail() { Title = "Premultiply", Class = "SharpDX.Direct2D1.Effects.Premultiply" });
            ret.Add(new EffectDetail() { Title = "Saturation", Class = "SharpDX.Direct2D1.Effects.Saturation" });
            ret.Add(new EffectDetail() { Title = "Scale", Class = "SharpDX.Direct2D1.Effects.Scale" });
            ret.Add(new EffectDetail() { Title = "Shadow", Class = "SharpDX.Direct2D1.Effects.Shadow" });
            ret.Add(new EffectDetail() { Title = "Spot Diffuse", Class = "SharpDX.Direct2D1.Effects.SpotDiffuse" });
            ret.Add(new EffectDetail() { Title = "Spot Specular", Class = "SharpDX.Direct2D1.Effects.SpotSpecular" });
            ret.Add(new EffectDetail() { Title = "Table Transfer", Class = "SharpDX.Direct2D1.Effects.TableTransfer" });
            ret.Add(new EffectDetail() { Title = "Tile", Class = "SharpDX.Direct2D1.Effects.Tile" });
            ret.Add(new EffectDetail() { Title = "Transform 3D", Class = "SharpDX.Direct2D1.Effects.Transform3D" });
            ret.Add(new EffectDetail() { Title = "Turbulence", Class = "SharpDX.Direct2D1.Effects.Turbulence" });
            ret.Add(new EffectDetail() { Title = "UnPremultiply", Class = "SharpDX.Direct2D1.Effects.UnPremultiply" });


            return ret;

        }
        #endregion

        #region FONTS

        #endregion
    }


    public class EffectDetail
    {
        public string Title { get; set; }
        public string Class { get; set; }
    }

    public class LayoutDetail
    {
        public Rect Dimension { get; set; }
        public string Title { get; set; }
        public int Dpi { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Scale { get; set; }
        public string MetroIcon { get; set; }
        public string DisplaySize { get; set; }
    }

    public class LayoutOrientation
    {
        public string Title { get; set; }
        public string MetroIcon { get; set; }
    }














}
