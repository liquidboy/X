
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
namespace SumoNinjaMonkey.Framework.Controls.Innertia
{
    public static class Conductor
    {
        //private static DispatcherTimer innertialCanvasDispatchTimer;

        public static event EventHandler Beat;

        private static int _counter = 4;  //15ms per composition frame = 15 * 6  = ~100 ms per animation tick
        public static bool IsRunning { get; set; }

        private static bool _isInitialized = false;
        private static Canvas _rootCanvas;

        public static void Initialize(Canvas root)
        {
            if (_isInitialized) return;
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            _rootCanvas = root;

            _rootCanvas.PointerPressed += _rootCanvas_PointerPressed;

            //innertialCanvasDispatchTimer = new DispatcherTimer();
            //innertialCanvasDispatchTimer.Interval = TimeSpan.FromMilliseconds(100);
            //innertialCanvasDispatchTimer.Tick += globalDispatchTimerTick;

            _isInitialized = true;
        }

        static void _rootCanvas_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Start();
            //if (IsRunning) Start();
        }

        public static void UnIntialize(){
            CompositionTarget.Rendering -= CompositionTarget_Rendering;
            _isInitialized = false;
            _counter = 8;
            IsRunning = false;
        }


        static void CompositionTarget_Rendering(object sender, object e)
        {
            _counter--;

            if (_counter == 0)
            {
                globalDispatchTimerTick(sender, e);
                _counter = 8;
            }
        }


        private static void globalDispatchTimerTick(object sender, object e)
        {
            if (!IsRunning) return;
            if (Beat != null) Beat(null, EventArgs.Empty);
        }


        public static void Stop()
        {
            //CompositionTarget.Rendering -= CompositionTarget_Rendering;
            //innertialCanvasDispatchTimer.Stop();
            IsRunning = false;
        }

        public static void Start()
        {
            //CompositionTarget.Rendering += CompositionTarget_Rendering;
            //innertialCanvasDispatchTimer.Start();
            IsRunning = true;
        }
    }
}
