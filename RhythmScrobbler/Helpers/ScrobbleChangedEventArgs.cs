using System;

namespace RhythmScrobbler.Helpers;

public class ScrobbleChangedEventArgs : EventArgs
{
    public RhythmScrobble RhythmScrobble { get; set; }
}