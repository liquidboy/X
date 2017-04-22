using System;
using System.Collections.Generic;
using System.Text;


namespace XNATweener
{
    public delegate void PositionChangedHandler<T>(T newPosition);
    public delegate void EndHandler();

    public interface ITweener
    {
        [Obsolete("Use Playing property instead")]
        bool Running { get;}
        bool Playing { get;}
        event EndHandler Ended;

        //void Update(GameTime gameTime);
        void Update();
        [Obsolete("Use Play method instead")]
        void Start();
        [Obsolete("Use Pause method instead")]
        void Stop();
        void Play();
        void Pause();
        void Reset();
        void Restart();
        void Reverse();
    }

    public interface ITweener<T> : ITweener
    {
        T Position { get;}
        event PositionChangedHandler<T> PositionChanged;

        void Reset(T to);
        void Reset(T to, TimeSpan duration);
        void Reset(T to, float speed);
        void Reset(T from, T to, TimeSpan duration);
        void Reset(T from, T to, float speed);
    }
}
