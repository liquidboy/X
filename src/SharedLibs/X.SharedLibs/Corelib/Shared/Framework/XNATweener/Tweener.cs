using System;


namespace XNATweener
{
    /// <summary>
    /// This is a Tweener for single float values.
    /// </summary>
    public class Tweener : BaseTweener<float>
    {
        #region Constructors
        /// <summary>
        /// Create a Tweener with info on where to move from and to, how long it should take and the function to use.
        /// </summary>
        /// <param name="from">The starting position</param>
        /// <param name="to">The position reached at the end</param>
        /// <param name="duration">How long befor we reach the end?</param>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public Tweener(float from, float to, float duration, TweeningFunction tweeningFunction)
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
        public Tweener(float from, float to, TimeSpan duration, TweeningFunction tweeningFunction)
            : base(from, to, (float)duration.TotalSeconds, tweeningFunction)
        {
        }

        /// <summary>
        /// Create a stopped tweener with no information on where to move from and to.
        /// Useful in conjunction with the Reset(from, to) call to ready a tweener for later use or lazy
        /// instantiation of a tweener in a property.
        /// </summary>
        /// <param name="tweeningFunction">Which function to use for calculating the current position.</param>
        public Tweener(TweeningFunction tweeningFunction)
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
        public Tweener(float from, float to, TweeningFunction tweeningFunction, float speed)
            : base(from, to, tweeningFunction, speed)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Do the actual update of the position.
        /// </summary>
        /// <param name="timeElapsed">The time that has elapsed since the beginning of the tweener.</param>
        /// <param name="start">Where did the tweener start</param>
        /// <param name="change">How much will the tweener move from start to end</param>
        /// <param name="duration">The total duration of tweening.</param>
        protected override void UpdatePosition(float elapsed, float from, float change, float duration)
        {
            Position = tweeningFunction(elapsed, from, change, duration);
        }

        /// <summary>
        /// Calculate the change value.
        /// </summary>
        /// <param name="to">Where do we want to end</param>
        /// <param name="from">Where we are now</param>
        /// <returns>Returns the new change value</returns>
        protected override float CalculateChange(float to, float from)
        {
            return to - from;
        }

        /// <summary>
        /// Calculate the position we want to end up in. This is nessecary as to is not saved.
        /// </summary>
        /// <returns>Returns the end position when the tweener is finished.</returns>
        protected override float CalculateEndPosition()
        {
            return from + change;
        }

        /// <summary>
        /// Calculate the duration of the tween in seconds given the average speed of movement.
        /// </summary>
        /// <param name="speed">The average movement speed</param>
        /// <returns>The duration of the tweener</returns>
        protected override float CalculateDurationFromSpeed(float speed)
        {
            return change / speed;
        }
        #endregion
    }
}
