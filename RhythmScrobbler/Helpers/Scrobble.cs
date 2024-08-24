using System;

namespace RhythmScrobbler.Helpers;

public class Scrobble
{
    public string SongName { get; set; }
    public string Artist { get; set; }
    public string Album { get; set; }

    public override string ToString()
    {
        return $"Artist: {Artist}\nSong: {SongName}\nAlbum: {Album}";
    }

    protected bool Equals(Scrobble other)
    {
        return SongName == other.SongName && Artist == other.Artist && Album == other.Album;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Scrobble)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SongName, Artist, Album);
    }
}