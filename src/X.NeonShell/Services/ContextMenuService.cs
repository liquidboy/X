using SumoNinjaMonkey.Framework.Controls.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using X.NeonShell.Controls;

namespace X.NeonShell.Services
{
    public class ContextMenuService
    {
        private static Grid _rootControl;
        private static StackPanel _MsgboxContainer;
        public static void Init(Grid control, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center)
        {
            ContextMenuService._rootControl = control;
            _MsgboxContainer = new StackPanel();
            _MsgboxContainer.Orientation = Orientation.Vertical;
            _MsgboxContainer.HorizontalAlignment = horizontalAlignment;
            _MsgboxContainer.VerticalAlignment = verticalAlignment;
            _MsgboxContainer.ChildrenTransitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection();
            _MsgboxContainer.ChildrenTransitions.Add(new Windows.UI.Xaml.Media.Animation.RepositionThemeTransition());
            _rootControl.Children.Add(_MsgboxContainer);


        }

        public enum ContextMenuType
        {
            PhotoDetailFlickrUserPicture,
            PhotoDetailFlickrCommentUserPicture,
            TreePublicFlickrUser,
            SectionHeaderFlickrUser
        }
        //double timeToLive,
        public async static void Show(
            string message,
            string title,
            ContextMenuType type,
            double width = 400,
            double height = 180,
            GeneralSystemWideMessage msgToPassAlong = null
            )
        {

            if (ContextMenuService._rootControl != null)
            {
                DispatchedHandler invokedHandler = new DispatchedHandler(() =>
                {
                    if (ContextMenuService._rootControl == null)
                    {
                        return;
                    }


                    //modal adorner
                    Rectangle rectModalAdorner = new Rectangle();
                    rectModalAdorner.Fill = new SolidColorBrush(Colors.Black);
                    rectModalAdorner.Opacity = 0.4;
                    rectModalAdorner.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rectModalAdorner.VerticalAlignment = VerticalAlignment.Stretch;
                    rectModalAdorner.SetValue(Canvas.ZIndexProperty, -2);
                    ContextMenuService._rootControl.Children.Add(rectModalAdorner);


                    //message background
                    Rectangle rectBackground = new Rectangle();
                    rectBackground.HorizontalAlignment = HorizontalAlignment.Stretch;
                    rectBackground.VerticalAlignment = VerticalAlignment.Center;
                    rectBackground.Height = height + 80;
                    rectBackground.SetValue(Canvas.ZIndexProperty, -1);
                    rectBackground.SetBinding(Rectangle.FillProperty, new Windows.UI.Xaml.Data.Binding() { Path = new PropertyPath("UI.Theme.AccentBackground1") });
                    ContextMenuService._rootControl.Children.Add(rectBackground);

                    ContextMenuService._rootControl.Visibility = Visibility.Visible;

                    //message
                    ContextMenuView view = new ContextMenuView(
                            message,
                            "",
                            msgToPassAlong: msgToPassAlong
                            );

                    view.Width = width;
                    view.Height = height;
                    view.Margin = new Thickness(3);
                    view.HorizontalAlignment = HorizontalAlignment.Center;
                    view.VerticalAlignment = VerticalAlignment.Center;

                    if (type == ContextMenuType.PhotoDetailFlickrUserPicture)
                    {
                        //view.LoadMainContent(new PhotoDetailFlickrUserPictureContextMenu());
                    }
                    else if (type == ContextMenuType.PhotoDetailFlickrCommentUserPicture)
                    {
                        //view.LoadMainContent(new PhotoDetailFlickrCommentUserPictureContextMenu() { MessageToPassAlong = msgToPassAlong });

                    }
                    else if (type == ContextMenuType.TreePublicFlickrUser)
                    {
                        //view.LoadMainContent(new PublicFlickrUserPictureContextMenu() { MessageToPassAlong = msgToPassAlong });
                    }
                    else if (type == ContextMenuType.SectionHeaderFlickrUser)
                    {
                        //view.LoadMainContent(new SectionHeaderPictureContextMenu() { MessageToPassAlong = msgToPassAlong });
                    }

                    view.Show();
                    ContextMenuService._rootControl.Children.Add(view);



                });

                await ContextMenuService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);


            }

        }

        public async static void Hide()
        {
            DispatchedHandler invokedHandler = new DispatchedHandler(() =>
            {
                ContextMenuService._rootControl.Children.Clear();
                ContextMenuService._rootControl.Visibility = Visibility.Collapsed;
            });
            await ContextMenuService._rootControl.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, invokedHandler);
        }


    }
}
