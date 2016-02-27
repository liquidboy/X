
using FavouriteMX.Shared.Views;
using System;
using Windows.UI.Xaml.Input;

namespace FavouriteMX.Shared.Services
{
    public class GestureService
    {

        private static GestureService Instance = new GestureService();

        public static event EventHandler OnGestureRaised;

        private GestureService()
        {


        }

        public static void Init()
        {
            

        }
        private static Windows.UI.Input.GestureRecognizer _gr;

        public static void Start(BaseUserPage pageWithPointerEventsToRegister)
        {
            if (pageWithPointerEventsToRegister == null) return;

            pageWithPointerEventsToRegister.PointerPressed += page_PointerPressed;
            pageWithPointerEventsToRegister.PointerMoved += page_PointerMoved;
            pageWithPointerEventsToRegister.PointerReleased += page_PointerReleased;


            _gr = new Windows.UI.Input.GestureRecognizer();
            _gr.CrossSliding += gr_CrossSliding;
            _gr.Dragging += gr_Dragging;
            _gr.Holding += gr_Holding;
            _gr.ManipulationCompleted += gr_ManipulationCompleted;
            _gr.ManipulationInertiaStarting += gr_ManipulationInertiaStarting;
            _gr.ManipulationStarted += gr_ManipulationStarted;
            _gr.ManipulationUpdated += gr_ManipulationUpdated;
            _gr.RightTapped += gr_RightTapped;
            _gr.Tapped += gr_Tapped;
            _gr.GestureSettings = 
                Windows.UI.Input.GestureSettings.ManipulationRotate 
                | Windows.UI.Input.GestureSettings.ManipulationTranslateX 
                | Windows.UI.Input.GestureSettings.ManipulationTranslateY 
                | Windows.UI.Input.GestureSettings.ManipulationScale 
                | Windows.UI.Input.GestureSettings.ManipulationRotateInertia 
                | Windows.UI.Input.GestureSettings.ManipulationScaleInertia 
                | Windows.UI.Input.GestureSettings.ManipulationTranslateInertia 
                | Windows.UI.Input.GestureSettings.Tap
                | Windows.UI.Input.GestureSettings.CrossSlide
                ;


        }

        public static void Stop(BaseUserPage pageWithPointerEventsToUnRegister)
        {
            if (pageWithPointerEventsToUnRegister == null) return;

            if (_gr != null)
            {
                _gr.CrossSliding -= gr_CrossSliding;
                _gr.Dragging -= gr_Dragging;
                _gr.Holding -= gr_Holding;
                _gr.ManipulationCompleted -= gr_ManipulationCompleted;
                _gr.ManipulationInertiaStarting -= gr_ManipulationInertiaStarting;
                _gr.ManipulationStarted -= gr_ManipulationStarted;
                _gr.ManipulationUpdated -= gr_ManipulationUpdated;
                _gr.RightTapped -= gr_RightTapped;
                _gr.Tapped -= gr_Tapped;

                pageWithPointerEventsToUnRegister.PointerPressed -= page_PointerPressed;
                pageWithPointerEventsToUnRegister.PointerMoved -= page_PointerMoved;
                pageWithPointerEventsToUnRegister.PointerReleased -= page_PointerReleased;

                _gr = null;
            }
        }


        static void gr_Tapped(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.TappedEventArgs args)
        {
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { TappedEventArgs = args });

        }
        static void gr_RightTapped(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.RightTappedEventArgs args)
        {
            //Debug.WriteLine("gr_RightTapped");
        }
        static void gr_Holding(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.HoldingEventArgs args)
        {
            //Debug.WriteLine("gr_Holding");
        }
        static void gr_Dragging(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.DraggingEventArgs args)
        {
            //Debug.WriteLine("gr_Dragging");
        }
        static void gr_CrossSliding(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.CrossSlidingEventArgs args)
        {
            //Debug.WriteLine("gr_CrossSliding");
        }
        static void gr_ManipulationUpdated(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationUpdatedEventArgs args)
        {
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { ManipulationUpdatedArgs = args });
        }
        static void gr_ManipulationStarted(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationStartedEventArgs args)
        {
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { ManipulationStartedArgs = args });
        }
        static void gr_ManipulationCompleted(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationCompletedEventArgs args)
        {
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { ManipulationCompletedArgs = args });
        }
        static void gr_ManipulationInertiaStarting(Windows.UI.Input.GestureRecognizer sender, Windows.UI.Input.ManipulationInertiaStartingEventArgs args)
        {
            //if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { ManipulationInertiaStartingArgs = args });
            
        }

        static void page_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var ps = e.GetIntermediatePoints(null);
            if (ps != null && ps.Count > 0)
            {
                _gr.ProcessUpEvent(ps[0]);
                e.Handled = true;
                _gr.CompleteGesture();
            }
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { ReleasedPointerRoutedEventArgs = e });
        }

        static void page_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            _gr.ProcessMoveEvents(e.GetIntermediatePoints(null));
            e.Handled = true;
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { MovedPointerRoutedEventArgs = e });
        }

        static void page_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var ps = e.GetIntermediatePoints(null);
            if (ps != null && ps.Count > 0)
            {
                _gr.ProcessDownEvent(ps[0]);
                e.Handled = true;
            }
            if (OnGestureRaised != null) OnGestureRaised(sender, new CustomGestureArgs() { PressedPointerRoutedEventArgs = e });
        }

    }


    public class CustomGestureArgs: EventArgs
    {
        public Windows.UI.Input.ManipulationStartedEventArgs ManipulationStartedArgs;
        public Windows.UI.Input.ManipulationUpdatedEventArgs ManipulationUpdatedArgs;
        public Windows.UI.Input.ManipulationCompletedEventArgs ManipulationCompletedArgs;

        public Windows.UI.Input.ManipulationInertiaStartingEventArgs ManipulationInertiaStartingArgs;

        public PointerRoutedEventArgs PressedPointerRoutedEventArgs;
        public PointerRoutedEventArgs MovedPointerRoutedEventArgs;
        public PointerRoutedEventArgs ReleasedPointerRoutedEventArgs;

        public Windows.UI.Input.TappedEventArgs TappedEventArgs;
    }
}
