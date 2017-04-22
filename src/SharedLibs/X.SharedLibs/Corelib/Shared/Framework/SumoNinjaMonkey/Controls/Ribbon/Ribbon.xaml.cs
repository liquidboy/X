using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using SumoNinjaMonkey.Framework.Controls;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SumoNinjaMonkey.Framework.Controls
{
    public sealed partial class Ribbon : UserControl, INotifyPropertyChanged
    {
        public Ribbon()
        {
            this.InitializeComponent();
            
        }


        private double _selectedItemWidth;
        public double SelectedItemWidth { get { return _selectedItemWidth; } set { _selectedItemWidth = value; this.NotifyPropertyChanged("SelectedItemWidth"); } }

        private double _selectedSubItemWidth;
        public double SelectedSubItemWidth { get { return _selectedSubItemWidth; } set { _selectedSubItemWidth = value; this.NotifyPropertyChanged("SelectedSubItemWidth"); } }
        
        private double _selectedSubFlyout1ItemWidth;
        public double SelectedSubFlyout1ItemWidth { get { return _selectedSubFlyout1ItemWidth; } set { _selectedSubFlyout1ItemWidth = value; this.NotifyPropertyChanged("SelectedSubFlyout1ItemWidth"); } }

        private double _selectedMenuItemWidth;
        public double SelectedMenuItemWidth { get { return _selectedMenuItemWidth; } set { _selectedMenuItemWidth = value; this.NotifyPropertyChanged("SelectedMenuItemWidth"); } }
        

        public ObservableCollection<RibbonTabItem> TabItems { get; set; }
        public ObservableCollection<IconGroup> IconGroups { get; set; }
        public ObservableCollection<IconItem> SelectedSubIconItems { get; set; }

        private SolidColorBrush TabLabelColor_Grey = new SolidColorBrush(new Color() { R = 155, G = 155, B = 155, A = 255 });
        private SolidColorBrush TabLabelColor_White = new SolidColorBrush(Colors.White);
        private SolidColorBrush TabLabelColor_WhiteSmoke = new SolidColorBrush(Colors.WhiteSmoke);
        private SolidColorBrush TabLabelColor_Blue = new SolidColorBrush(Colors.Blue);

        private TextBlock lastSelectedLabel;
        private TextBlock lastSelectedSubLabel;
        private TextBlock lastSelectedMeuItemLabel;
        Windows.UI.Xaml.Shapes.Path currentlySelectedIconPath;
        Windows.UI.Xaml.Shapes.Path currentlySelectedSubIconPath;

        //SelectedSubMenuItems

        private void layoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            this.TabItems = new ObservableCollection<RibbonTabItem>();
            this.IconGroups = new ObservableCollection<IconGroup>();
            this.SelectedSubIconItems = new ObservableCollection<IconItem>();
            

            layoutRoot.DataContext = this;

            BuildCollections(0);


            //FrameworkElement uie = (FrameworkElement)this.Parent;
            this.Height += 100;
            this.Margin = new Thickness(0, 0, 0, -100);


            //Try to select the first tab and have that default!
            //icTabs.UpdateLayout();
            //ContentPresenter cp = (ContentPresenter)icTabs.ItemContainerGenerator.ContainerFromItem(icTabs.Items[0]);
            //DataTemplate dt = cp.ContentTemplate as DataTemplate;
            ////dt.FindName <== not supported :( :( :(
        }
     

        private ObservableCollection<IconItem> CreateTestSubMenuItemCollections()
        {
            
            ObservableCollection<IconItem> iconSubItems1 = new ObservableCollection<IconItem>();
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Folders", IconVector = GetMetroIcon("Foldercube", TabLabelColor_Grey)});
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Add", IconVector = GetMetroIcon("AddFolder", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Close", IconVector = GetMetroIcon("FolderClosed", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Settings", IconVector = GetMetroIcon("FolderSettings", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Information", IconVector = GetMetroIcon("FolderInformation", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Lock", IconVector = GetMetroIcon("FolderLock", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Movies", IconVector = GetMetroIcon("MovieFolder", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Music", IconVector = GetMetroIcon("MusicFolder", TabLabelColor_Grey) });
            iconSubItems1.Add(new IconItem() { DisplayTitle = "Time", IconVector = GetMetroIcon("FolderTime", TabLabelColor_Grey) });

            return iconSubItems1;
        }

        private void BuildCollections(int type)
        {
            if (type == 0)  //BUILD TABS COLLECTION
            {
                this.TabItems.Add(new RibbonTabItem() { ID = 1, DisplayTitle = "FILES", SelectedBackgroundColor = new SolidColorBrush(Colors.Blue), SelectedForegroundColor = TabLabelColor_White, NormalForegroundColor = TabLabelColor_Grey });
                this.TabItems.Add(new RibbonTabItem() { ID = 2, DisplayTitle = "DRAWING", SelectedBackgroundColor = new SolidColorBrush(Colors.Blue), SelectedForegroundColor = TabLabelColor_White, NormalForegroundColor = TabLabelColor_Grey });
                this.TabItems.Add(new RibbonTabItem() { ID = 3, DisplayTitle = "REVIEW", SelectedBackgroundColor = new SolidColorBrush(Colors.Blue), SelectedForegroundColor = TabLabelColor_White, NormalForegroundColor = TabLabelColor_Grey });
                this.TabItems.Add(new RibbonTabItem() { ID = 4, DisplayTitle = "PAGE LAYOUT", SelectedBackgroundColor = new SolidColorBrush(Colors.Blue), SelectedForegroundColor = TabLabelColor_White, NormalForegroundColor = TabLabelColor_Grey });
                this.TabItems.Add(new RibbonTabItem() { ID = 5, DisplayTitle = "VIEW", SelectedBackgroundColor = new SolidColorBrush(Colors.Blue), SelectedForegroundColor = TabLabelColor_White, NormalForegroundColor = TabLabelColor_Grey });

                NotifyPropertyChanged("TabItems");
            }
            else if (type == 1) //BUILD FILE COLLECTION
            {
                this.IconGroups.Clear();

                ObservableCollection<IconItem> iconSubItems1 = CreateTestSubMenuItemCollections();

                ObservableCollection<IconItem> iconItems1 = new ObservableCollection<IconItem>();
                iconItems1.Add(new IconItem() { DisplayTitle = "About", IconVector = GetMetroIcon("About"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Tables", IconItems = iconItems1 });

                ObservableCollection<IconItem> iconItems2 = new ObservableCollection<IconItem>();
                iconItems2.Add(new IconItem() { DisplayTitle = "Blogger", IconVector = GetMetroIcon("Blogger"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Add", IconVector = GetMetroIcon("Add"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "YouTube", IconVector = GetMetroIcon("YouTube"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Images", IconItems = iconItems2 });

                ObservableCollection<IconItem> iconItems3 = new ObservableCollection<IconItem>();
                iconItems3.Add(new IconItem() { DisplayTitle = "Android", IconVector = GetMetroIcon("Android"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Apple", IconVector = GetMetroIcon("Apple"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Digg", IconVector = GetMetroIcon("Digg"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Wordpress", IconVector = GetMetroIcon("Wordpress"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Delicious", IconVector = GetMetroIcon("Delicious"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Illustrations", IconItems = iconItems3 });

                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
            else if (type == 2) //BUILD DRAWING COLLECTION
            {
                this.IconGroups.Clear();

                ObservableCollection<IconItem> iconSubItems1 = CreateTestSubMenuItemCollections();


                ObservableCollection<IconItem> iconItems1 = new ObservableCollection<IconItem>();
                iconItems1.Add(new IconItem() { DisplayTitle = "Check", IconVector = GetMetroIcon("Check"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Clipboard", IconVector = GetMetroIcon("Clipboard"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Cut", IconVector = GetMetroIcon("Cut"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Sketch", IconItems = iconItems1 });

                ObservableCollection<IconItem> iconItems2 = new ObservableCollection<IconItem>();
                iconItems2.Add(new IconItem() { DisplayTitle = "Copy", IconVector = GetMetroIcon("Copy"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Delete", IconVector = GetMetroIcon("Delete"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Desktop", IconVector = GetMetroIcon("Desktop"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Text", IconVector = GetMetroIcon("Text"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Left Align", IconVector = GetMetroIcon("TextLeftAlign"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Right Align", IconVector = GetMetroIcon("TextRightAlign"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Effects", IconItems = iconItems2 });

                ObservableCollection<IconItem> iconItems3 = new ObservableCollection<IconItem>();
                iconItems3.Add(new IconItem() { DisplayTitle = "Disc Warning", IconVector = GetMetroIcon("DiscWarning"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Door", IconVector = GetMetroIcon("Door"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Digg", IconVector = GetMetroIcon("Digg"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Error", IconVector = GetMetroIcon("Error"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Export", IconVector = GetMetroIcon("Export"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Find", IconVector = GetMetroIcon("Find"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Find Select", IconVector = GetMetroIcon("FindSelect"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Find Text", IconVector = GetMetroIcon("FindText"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Brushes", IconItems = iconItems3 });

                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
            else if (type == 3) //BUILD REVIEW COLLECTION
            {
                this.IconGroups.Clear();

                ObservableCollection<IconItem> iconSubItems1 = CreateTestSubMenuItemCollections();

                ObservableCollection<IconItem> iconItems1 = new ObservableCollection<IconItem>();
                iconItems1.Add(new IconItem() { DisplayTitle = "Forbidden", IconVector = GetMetroIcon("Forbidden"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Help", IconItems = iconItems1 });

                ObservableCollection<IconItem> iconItems2 = new ObservableCollection<IconItem>();
                iconItems2.Add(new IconItem() { DisplayTitle = "Garbage", IconVector = GetMetroIcon("Garbage"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Empty", IconVector = GetMetroIcon("GarbageEmpty"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Full", IconVector = GetMetroIcon("GarbageFull"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Editor", IconItems = iconItems2 });

                ObservableCollection<IconItem> iconItems3 = new ObservableCollection<IconItem>();
                iconItems3.Add(new IconItem() { DisplayTitle = "Up", IconVector = GetMetroIcon("NavigationUp"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Down", IconVector = GetMetroIcon("NavigationDown"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Left", IconVector = GetMetroIcon("NavigationLeft"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Right", IconVector = GetMetroIcon("NavigationRight"), IconItems = iconSubItems1 });
                iconItems3.Add(new IconItem() { DisplayTitle = "Notebook", IconVector = GetMetroIcon("Notebook"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Reviewer", IconItems = iconItems3 });

                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
            else if (type == 4) //BUILD LAYOUT COLLECTION
            {
                this.IconGroups.Clear();

                ObservableCollection<IconItem> iconSubItems1 = CreateTestSubMenuItemCollections();


                ObservableCollection<IconItem> iconItems1 = new ObservableCollection<IconItem>();
                iconItems1.Add(new IconItem() { DisplayTitle = "Photo Scenery", IconVector = GetMetroIcon("PhotoScenery"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Redo", IconVector = GetMetroIcon("Redo"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Undo", IconVector = GetMetroIcon("Undo"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Front", IconItems = iconItems1 });

                ObservableCollection<IconItem> iconItems2 = new ObservableCollection<IconItem>();
                iconItems2.Add(new IconItem() { DisplayTitle = "Closed", IconVector = GetMetroIcon("FolderClosed"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Preferences", IconVector = GetMetroIcon("WindowPreferences"), IconItems = iconSubItems1 });
                iconItems2.Add(new IconItem() { DisplayTitle = "Split", IconVector = GetMetroIcon("WindowVerticalsplit"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "Back", IconItems = iconItems2 });


                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
            else if (type == 5) //BUILD VIEW COLLECTION
            {
                this.IconGroups.Clear();

                ObservableCollection<IconItem> iconSubItems1 = CreateTestSubMenuItemCollections();


                ObservableCollection<IconItem> iconItems1 = new ObservableCollection<IconItem>();
                iconItems1.Add(new IconItem() { DisplayTitle = "View", IconVector = GetMetroIcon("ViewDocument"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Time", IconVector = GetMetroIcon("DocumentTime"), IconItems = iconSubItems1 });
                iconItems1.Add(new IconItem() { DisplayTitle = "Warning", IconVector = GetMetroIcon("DocumentWarning"), IconItems = iconSubItems1 });
                this.IconGroups.Add(new IconGroup() { DisplayTitle = "General", IconItems = iconItems1 });



                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
            else
            {
                this.IconGroups.Clear();
                NotifyPropertyChanged("IconGroups");
                NotifyPropertyChanged("IconItems");
            }
        }

        private Windows.UI.Xaml.Shapes.Path GetMetroIcon(string key, Brush iconColor = null )
        {
            if (iconColor == null) iconColor = TabLabelColor_Grey;

            string temp = (string)Application.Current.Resources[key];
            string pthString = @"<Path xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"" 
                Data=""" + temp + @""" />";
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)Windows.UI.Xaml.Markup.XamlReader.Load(pthString);
            pth.Stretch = Stretch.Uniform;
            pth.Fill = iconColor;
            return pth;
        }



        private void grdTab_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.SelectedSubItemWidth = 0;
            this.SelectedSubFlyout1ItemWidth = 0;
            this.SelectedMenuItemWidth = 0;
            sbHighlightSubTabSelected.Stop();
            sbHighlightSubFlyout2TabSelected.Stop();
            sbHighlightMenuItemSelected.Stop();

            this.SelectedSubIconItems.Clear();
            NotifyPropertyChanged("SelectedSubIconItems");

            Grid selectedGridItem = (Grid)sender;
            RibbonTabItem selectedItem = (RibbonTabItem)selectedGridItem.DataContext;

            BuildCollections(selectedItem.ID);
            Level0TabSelected(selectedGridItem, selectedItem);
            
        }

        private void grdIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.sbHighlightMenuItemSelected.Stop();
            this.SelectedMenuItemWidth = 0;
            
            Grid selectedContainer = (Grid)sender;
            

            //SET SELECTED ICON
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)((ContentControl)selectedContainer.Children[0]).Content;

            if (currentlySelectedIconPath != null)
            {

                //unselect it
                currentlySelectedIconPath.Fill = TabLabelColor_Grey;

                if (currentlySelectedIconPath == pth)
                {
                    currentlySelectedIconPath = null;
                    return;
                }

                currentlySelectedIconPath = null;

            }

            // SET THE SUBMENU
            
            this.SelectedSubIconItems.Clear();
            NotifyPropertyChanged("SelectedSubIconItems");



            if (selectedContainer.DataContext != null && selectedContainer.DataContext is IconItem)
            {
                
                IconItem si = (IconItem)selectedContainer.DataContext;
                if (si.IconItems != null)
                {
                    foreach (var i in si.IconItems)
                    {
                        this.SelectedSubIconItems.Add(i);
                    }
                    NotifyPropertyChanged("SelectedSubIconItems");
                }
            }


            //select it
            pth.Fill = TabLabelColor_White;
            currentlySelectedIconPath = pth;

            Level1IconSelected(selectedContainer);
            Level2AnimateMenu(selectedContainer);
        }

        private void grdSubIcon_PointerReleased(object sender, PointerRoutedEventArgs e)
        {

            Grid selectedContainer = (Grid)sender;


            //UNSELECT PREVIOUS SELECTED ICON
            Windows.UI.Xaml.Shapes.Path pth = (Windows.UI.Xaml.Shapes.Path)((ContentControl)selectedContainer.Children[0]).Content;

            if (currentlySelectedSubIconPath != null)
            {

                //unselect it
                currentlySelectedSubIconPath.Fill = TabLabelColor_Grey;

                if (currentlySelectedSubIconPath == pth)
                {
                    currentlySelectedSubIconPath = null;
                    return;
                }

            }


            //select it
            pth.Fill = TabLabelColor_White;
            currentlySelectedSubIconPath = pth;


            Level2IconSelected(selectedContainer);
        }


        private void Level0TabSelected(Grid selectedGridItem, RibbonTabItem selectedItem)
        {
            TextBlock tabLabel = (TextBlock)selectedGridItem.Children[0];

            GeneralTransform gt = layoutRoot.TransformToVisual(selectedGridItem);
            Point pt = gt.TransformPoint(new Point(0, 0));

            if (lastSelectedLabel != null) lastSelectedLabel.Foreground = TabLabelColor_Grey;
            selectedRectangle.Fill = selectedItem.SelectedBackgroundColor;
            tabLabel.Foreground = TabLabelColor_White;
            lastSelectedLabel = tabLabel;


            SplineDoubleKeyFrame kf1 = (SplineDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbHighlightTabSelected.Children[0]).KeyFrames[0];
            kf1.Value = Math.Abs(pt.X);

            this.SelectedItemWidth = Math.Abs(selectedGridItem.ActualWidth);

            sbHighlightTabSelected.Begin();
        }

        private void Level1IconSelected(Grid selectedGridContainer)
        {
            if (lastSelectedSubLabel != null) lastSelectedSubLabel.Foreground = TabLabelColor_Grey;
            Grid root = (Grid)selectedGridContainer.Parent;

            TextBlock label = root.Children[1] as TextBlock;
            lastSelectedSubLabel = label;

            
            GeneralTransform gt = layoutRoot.TransformToVisual(selectedGridContainer);
            Point pt1 = gt.TransformPoint(new Point(0, 0));

            gt = layoutRoot.TransformToVisual(label);
            Point pt2 = gt.TransformPoint(new Point(0, 0));

            selectedSubRectangle.Fill = TabLabelColor_Blue;
            label.Foreground = TabLabelColor_White;

            double leftToUse = Math.Min(Math.Abs(pt1.X), Math.Abs(pt2.X)) - 10;
            double widthToUse = Math.Abs(pt1.X) < Math.Abs(pt2.X) ? selectedGridContainer.ActualWidth + 20 : label.ActualWidth + 20;    //selectedGridContainer.ActualWidth + 20;



            SplineDoubleKeyFrame kf1 = (SplineDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbHighlightSubTabSelected.Children[0]).KeyFrames[0];
            kf1.Value = leftToUse;
            this.SelectedSubItemWidth = Math.Abs(widthToUse);
            sbHighlightSubTabSelected.Begin();

            
        }

        private void Level2AnimateMenu(Grid selectedGridContainer)
        {
            Grid root = (Grid)selectedGridContainer.Parent;
            TextBlock label = root.Children[1] as TextBlock;
            lastSelectedSubLabel = label;



            GeneralTransform gt = layoutRoot.TransformToVisual(selectedGridContainer);
            Point pt1 = gt.TransformPoint(new Point(0, 0));

            gt = layoutRoot.TransformToVisual(label);
            Point pt2 = gt.TransformPoint(new Point(0, 0));


            //underneath flyout connector
            selectedSubRectangleFlyout1.Fill = new SolidColorBrush(Colors.DarkBlue);

            double leftToUse = Math.Min(Math.Abs(pt1.X), Math.Abs(pt2.X)) - 5;
            double widthToUse = Math.Abs(pt1.X) < Math.Abs(pt2.X) ? selectedGridContainer.ActualWidth + 10 : label.ActualWidth + 10;    //selectedGridContainer.ActualWidth + 20;


            SplineDoubleKeyFrame kf1 = (SplineDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbHighlightSubFlyout1TabSelected.Children[0]).KeyFrames[0];
            kf1.Value = leftToUse;
            this.SelectedSubFlyout1ItemWidth = widthToUse;

            //actual sub tab menu container
            selectedSubRectangleFlyout2.Fill = TabLabelColor_WhiteSmoke;
            selectedSubRectangleFlyout2.Height = 90;


            //run 2 storyboards
            sbHighlightSubFlyout1TabSelected.Begin();
            sbHighlightSubFlyout2TabSelected.Begin();
        }

        private void Level2IconSelected(Grid selectedGridContainer)
        {

            //SET SELECTED ICON
            if (lastSelectedMeuItemLabel != null) lastSelectedMeuItemLabel.Foreground = TabLabelColor_Grey;
            Grid root = (Grid)selectedGridContainer.Parent;

            TextBlock label = root.Children[1] as TextBlock;
            lastSelectedMeuItemLabel = label;


            GeneralTransform gt = layoutRoot.TransformToVisual(selectedGridContainer);
            Point pt1 = gt.TransformPoint(new Point(0, 0));

            gt = layoutRoot.TransformToVisual(label);
            Point pt2 = gt.TransformPoint(new Point(0, 0));



            selectedMenuItemRectangle.Fill = TabLabelColor_Blue;
            label.Foreground = TabLabelColor_White;

            double leftToUse = Math.Min(Math.Abs(pt1.X), Math.Abs(pt2.X)) - 10;
            double widthToUse = Math.Abs(pt1.X) < Math.Abs(pt2.X) ? selectedGridContainer.ActualWidth + 20 : label.ActualWidth + 20; 


            SplineDoubleKeyFrame kf1 = (SplineDoubleKeyFrame)((DoubleAnimationUsingKeyFrames)sbHighlightMenuItemSelected.Children[0]).KeyFrames[0];
            kf1.Value = leftToUse;
            this.SelectedMenuItemWidth = widthToUse;
            sbHighlightMenuItemSelected.Begin();
        }






        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }


    }
}
