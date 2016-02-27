//using SharpDX;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace XNATweener
{
    /// <summary>
    /// This is a tweener for Vector2 values
    /// </summary>
    public class Vector2Tweener : BaseTweener<Vector2>
    {
        #region Constructors
        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public Vector2Tweener(Vector2 from, Vector2 to, float duration, TweeningFunction tweeningFunction)
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
        public Vector2Tweener(Vector2 from, Vector2 to, TimeSpan duration, TweeningFunction tweeningFunction)
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
        public Vector2Tweener(TweeningFunction tweeningFunction)
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
        public Vector2Tweener(Vector2 from, Vector2 to, TweeningFunction tweeningFunction, float speed)
            : base(from, to, tweeningFunction, speed)
        {
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
        protected override void UpdatePosition(float elapsed, Vector2 from, Vector2 change, float duration)
        {
            Position = new Vector2(tweeningFunction(elapsed, from.X, change.X, duration),
                                   tweeningFunction(elapsed, from.Y, change.Y, duration));
        }

        /// <summary>
        /// Calculate the change value. Usually this is to - from.
        /// </summary>
        /// <param name="to">Where do we want to end</param>
        /// <param name="from">Where we are now</param>
        /// <returns>Returns the new change value</returns>
        protected override Vector2 CalculateChange(Vector2 to, Vector2 from)
        {
            return to - from;
        }

        /// <summary>
        /// Calculate the position we want to end up in. This is nessecary as to is not saved.
        /// Usually this is from + change
        /// </summary>
        /// <returns>
        /// Returns the end position when the tweener is finished.
        /// </returns>
        protected override Vector2 CalculateEndPosition()
        {
            return from + change;
        }

        /// <summary>
        /// Calculate the duration of the tween in seconds given the average speed of movement.
        /// Usually this is change / speed
        /// </summary>
        /// <param name="speed">The average movement speed</param>
        /// <returns>The duration of the tweener</returns>
        protected override float CalculateDurationFromSpeed(float speed)
        {
            return change.Length() / speed;
        } 
        #endregion
    }
}
