using System;
using GalaSoft.MvvmLight.Messaging;
using Popcorn.Utils;

namespace Popcorn.Messaging
{
    public class PlayTrailerMessage : MessageBase
    {
        public string TrailerUrl { get; set; }

        public string MovieTitle { get; set; }

        public Action TrailerEndedAction { get; set; }

        public Action TrailerStoppedAction { get; set; }

        public MediaType Type { get; set; }

        public PlayTrailerMessage(string trailerUrl, string movieTitle, Action trailerEndedAction, Action trailerStoppedAction, MediaType type)
        {
            TrailerUrl = trailerUrl;
            MovieTitle = movieTitle;
            TrailerEndedAction = trailerEndedAction;
            TrailerStoppedAction = trailerStoppedAction;
            Type = type;
        }
    }
}
