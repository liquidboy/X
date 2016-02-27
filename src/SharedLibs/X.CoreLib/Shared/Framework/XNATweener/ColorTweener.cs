using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

//using SharpDX;

namespace XNATweener
{
    public class ColorTweener : BaseTweener<Color>
    {
        #region Constructors
        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public ColorTweener(Color from, Color to, float duration, TweeningFunction tweeningFunction)
            : base(from, to, duration, tweeningFunction)
        {
        }

        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public ColorTweener(Color from, Color to, TimeSpan duration, TweeningFunction tweeningFunction)
            : base(from, to, (float)duration.TotalSeconds, tweeningFunction)
        {
        }

        /// <summary>
        /// Create a stopped tweener with no information on where to move from and to.
        /// Useful in conjunction with the Reset(from, to) call to ready a tweener for later use or lazy
        /// instantiation of a tweener in a property.
        /// </summary>
        /// <param name="duration">The duration of tweening.</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public ColorTweener(TweeningFunction tweeningFunction)
            : base(tweeningFunction)
        {
        }

        /// <summary>
        /// Create a Tweener with info on where to move from and to, but set the duration using the movement
        /// speed instead of a set timespan.
        /// Note that the speed is used to calculate how fast the tweener should move if it moved in a linear
        /// fashion. This can be upset by the tweening function that can cause the actual movement speed to vary
        /// considerably. So the speed can be looked at as an average speed during the lifetime of the tweener.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">The average movement speed of the tweener</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public ColorTweener(Color from, Color to, TweeningFunction tweeningFunction, float speed)
            : base(from, to, tweeningFunction, speed)
        {
        }
        #endregion

        #region Properties
        private byte tweeningDirections = 0;

        private const byte DIRECTION_R_UP = 0x40;
        private const byte DIRECTION_R_DOWN = 0x80;
        internal short directionR
        {
            get
            {
                if ((tweeningDirections & DIRECTION_R_DOWN) != 0)
                {
                    return -1;
                }
                else if ((tweeningDirections & DIRECTION_R_UP) != 0)
                {
                    return 1;
                }
                return 0;
            }
            set
            {
                switch (Math.Sign(value))
                {
                    case -1:
                        tweeningDirections |= DIRECTION_R_DOWN;
                        break;
                    case 1:
                        tweeningDirections |= DIRECTION_R_UP;
                        break;
                }
            }
        }

        private const byte DIRECTION_G_UP = 0x10;
        private const byte DIRECTION_G_DOWN = 0x20;
        internal short directionG
        {
            get
            {
                if ((tweeningDirections & DIRECTION_G_DOWN) != 0)
                {
                    return -1;
                }
                else if ((tweeningDirections & DIRECTION_G_UP) != 0)
                {
                    return 1;
                }
                return 0;
            }
            set
            {
                switch (Math.Sign(value))
                {
                    case -1:
                        tweeningDirections |= DIRECTION_G_DOWN;
                        break;
                    case 1:
                        tweeningDirections |= DIRECTION_G_UP;
                        break;
                }
            }
        }

        private const byte DIRECTION_B_UP = 0x04;
        private const byte DIRECTION_B_DOWN = 0x08;
        protected short directionB
        {
            get
            {
                if ((tweeningDirections & DIRECTION_B_DOWN) != 0)
                {
                    return -1;
                }
                else if ((tweeningDirections & DIRECTION_B_UP) != 0)
                {
                    return 1;
                }
                return 0;
            }
            set
            {
                switch (Math.Sign(value))
                {
                    case -1:
                        tweeningDirections |= DIRECTION_B_DOWN;
                        break;
                    case 1:
                        tweeningDirections |= DIRECTION_B_UP;
                        break;
                }
            }
        }

        private const byte DIRECTION_A_UP = 0x01;
        private const byte DIRECTION_A_DOWN = 0x02;
        protected short directionA
        {
            get
            {
                if ((tweeningDirections & DIRECTION_A_DOWN) != 0)
                {
                    return -1;
                }
                else if ((tweeningDirections & DIRECTION_A_UP) != 0)
                {
                    return 1;
                }
                return 0;
            }
            set
            {
                switch (Math.Sign(value))
                {
                    case -1:
                        tweeningDirections |= DIRECTION_A_DOWN;
                        break;
                    case 1:
                        tweeningDirections |= DIRECTION_A_UP;
                        break;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Do the actual update of the position.
        /// Usually we will use the tweening function here.
        /// </summary>
        /// <param name="elapsed"></param>
        /// <param name="from"></param>
        /// <param name="change">How much will the tweener move from start to end</param>
        /// <param name="duration">The total duration of tweening.</param>
        protected override void UpdatePosition(float elapsed, Color from, Color change, float duration)
        {
            Position = new Color((directionR != 0) ? (byte)tweeningFunction(elapsed, from.R, change.R * directionR, duration) : from.R,
                                 (directionG != 0) ? (byte)tweeningFunction(elapsed, from.G, change.G * directionG, duration) : from.G,
                                 (directionB != 0) ? (byte)tweeningFunction(elapsed, from.B, change.B * directionB, duration) : from.B,
                                 (directionA != 0) ? (byte)tweeningFunction(elapsed, from.A, change.A * directionA, duration) : from.A);
        }

        /// <summary>
        /// Calculate the change value. Usually this is to - from.
        /// </summary>
        /// <param name="to">Where do we want to end</param>
        /// <param name="from">Where we are now</param>
        /// <returns>Returns the new change value</returns>
        protected override Color CalculateChange(Color to, Color from)
        {
            tweeningDirections = 0;
            int changeR = to.R - from.R;
            int changeG = to.G - from.G;
            int changeB = to.B - from.B;
            int changeA = to.A - from.A;
            directionR = (short)changeR;
            directionG = (short)changeG;
            directionB = (short)changeB;
            directionA = (short)changeA;
            return new Color((byte)Math.Abs(changeR),
                            (byte)Math.Abs(changeG),
                            (byte)Math.Abs(changeB), 
                            (byte)Math.Abs(changeA));
        }

        /// <summary>
        /// Calculate the position we want to end up in. This is nessecary as to is not saved.
        /// Usually this is from + change
        /// </summary>
        /// <returns>
        /// Returns the end position when the tweener is finished.
        /// </returns>
        protected override Color CalculateEndPosition()
        {
            return new Color((byte)(from.R + (change.R * directionR)),
                            (byte)(from.G + (change.G * directionG)),
                            (byte)(from.B + (change.B * directionB)),
                            (byte)(from.A + (change.A * directionA)));
        }

        /// <summary>
        /// Calculate the duration of the tween in seconds given the average speed of movement.
        /// Usually this is change / speed
        /// </summary>
        /// <param name="speed">The average movement speed</param>
        /// <returns>The duration of the tweener</returns>
        protected override float CalculateDurationFromSpeed(float speed)
        {
            return (change.R + change.G + change.B + change.A) / speed;
        } 
        #endregion
    }
}
