using System;

namespace RhythmScrobbler.Helpers;

public class ScrobbleChangedEventArgs : EventArgs
{
    public Scrobble Scrobble { get; set; }
}