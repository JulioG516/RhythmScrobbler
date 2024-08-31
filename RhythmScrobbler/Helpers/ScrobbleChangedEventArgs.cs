using System;

namespace RhythmScrobbler.Models;

public class ScrobbleChangedEventArgs : EventArgs
{
    public RhythmScrobble RhythmScrobble { get; set; }
}