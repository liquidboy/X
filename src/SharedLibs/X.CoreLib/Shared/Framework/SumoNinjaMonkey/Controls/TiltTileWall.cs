using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed class TiltTileWall : Control
    {

        Canvas mainCanvas;

        public int Rows { get; set; }
        public int Columns { get; set; }
        public double TileWidth { get; set; }
        public double TileHeight { get; set; }
        
        //row, column
        public List<string> ActionsList { get; set; }
        public List<object> LabelsList { get; set; }
        public List<bool> EnabledList { get; set; }
        public List<string> ResourcePathList { get; set; }

        public string ClickIdentifier { get; set; } //only applicable if clickaction=messenger
        public string ClickAggregateId { get; set; } //only applicable if clickaction=messenger

        public TiltTile.eClickAction ClickAction { get; set; }

        public Brush NormalTileBackground { get; set; }
        public Brush SelectedTileBackground { get; set; }
        public Brush DisabledTileBackground { get; set; }


        public TiltTileWall()
        {
            this.DefaultStyleKey = typeof(TiltTileWall);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (mainCanvas == null)
            {

                mainCanvas = (Canvas)GetTemplateChild("mainCanvas");

                LoadTiles();
            }

        }

        public void LoadTiles()
        {

            bool hasLabels = LabelsList != null;
                


            int labelCounter = 0;
            int enabledCounter = 0;
            int resourcePathListCounter = 0;
            int actionsCounter = 0;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    TiltTile tt = new TiltTile() { Width = TileWidth, Height = TileHeight };
                    tt.SetValue(Canvas.LeftProperty, (c * (TileWidth + 5)) + 5);
                    tt.SetValue(Canvas.TopProperty, (r * (TileHeight + 5)) + 5);
                    
                    tt.NormalBackground = NormalTileBackground; 
                    tt.SelectedBackground = SelectedTileBackground; 
                    tt.DisabledBackground = DisabledTileBackground;
                    tt.ClickMessengerIdentifier = ClickIdentifier;
                    tt.ClickMessengerAggregateId = ClickAggregateId;
                    if (ActionsList != null && ActionsList.Count > actionsCounter) { tt.ClickMessengerAction = (string)ActionsList[actionsCounter]; actionsCounter++; }
                    tt.ClickAction = ClickAction;
                    tt.PointerReleasedHandled = true;
                    if (hasLabels && LabelsList.Count > labelCounter) { tt.LabelFontSize = 12; tt.Label = (string)LabelsList[labelCounter]; labelCounter++; }
                    if (EnabledList != null && EnabledList.Count > enabledCounter) { tt.IsDisabled = (bool)EnabledList[enabledCounter]; enabledCounter++; }
                    if (ResourcePathList != null && ResourcePathList.Count > resourcePathListCounter) { tt.LoadPathIcon( (string)ResourcePathList[resourcePathListCounter] ); resourcePathListCounter++; }


                    //tt.Label = Labels[r, c]; //unfortunately this isnt initialized at this point
                    mainCanvas.Children.Add(tt);
                }
            }
            IsTilesHidden = false;
        }

        public bool IsTilesHidden { get; set; }

        public void HideTiles()
        {
            //i need to add this here rather than in xaml as it interferes with the "EntranceThemeTransition"
            mainCanvas.ChildrenTransitions.Add(new AddDeleteThemeTransition());

            mainCanvas.Children.Clear();

            mainCanvas.ChildrenTransitions.Clear();

            IsTilesHidden = true;
        }

        public void ShowTiles()
        {
            mainCanvas.ChildrenTransitions.Clear();

            mainCanvas.ChildrenTransitions.Add(new EntranceThemeTransition());

            LoadTiles();

            IsTilesHidden = false;
        }

    }
}
