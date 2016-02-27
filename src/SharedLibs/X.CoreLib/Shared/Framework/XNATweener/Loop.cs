using System;
using System.Collections.Generic;
using System.Text;

namespace XNATweener
{
    /// <summary>
    /// <para>The Loop class is a static class for easy loop control of the Tweener.</para>
    /// <para>You can loop continuousely FrontToBack or BackAndForth or for a specific number of times.</para>
    /// <para>It can be used either by the static methods on this class or by the corresponding methods on the Tweener classes.</para>
    /// </summary>
    public static class Loop
    {
        #region Static methods
        public static void FrontToBack(ITweener tweener)
        {
            tweener.Ended += tweener.Restart;
        }

        public static void FrontToBack(ITweener tweener, int times)
        {
            TimesLoopingHelper helper = new TimesLoopingHelper(tweener, times);
            tweener.Ended += helper.FrontToBack;
        }

        public static void BackAndForth(ITweener tweener)
        {
            tweener.Ended += delegate { tweener.Reverse(); };
        }

        public static void BackAndForth(ITweener tweener, int times)
        {
            TimesLoopingHelper helper = new TimesLoopingHelper(tweener, times);
            tweener.Ended += helper.BackAndForth;
        }
        #endregion

        #region Internal classes
        private struct TimesLoopingHelper
        {
            public TimesLoopingHelper(ITweener tweener, int times)
            {
                this.tweener = tweener;
                this.times = times;
            }

            private int times;
            private ITweener tweener;

            private bool Stop()
            {
                return --times == 0;
            }

            public void FrontToBack()
            {
                if (Stop())
                {
                    tweener.Ended -= FrontToBack;
                }
                else
                {
                    tweener.Reset();
                }
            }

            public void BackAndForth()
            {
                if (Stop())
                {
                    tweener.Ended -= BackAndForth;
                }
                else
                {
                    tweener.Reverse();
                }
            }
        }
        #endregion
    }
}
