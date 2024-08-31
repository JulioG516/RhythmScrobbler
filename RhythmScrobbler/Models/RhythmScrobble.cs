using System;
using Hqub.Lastfm.Entities;

namespace RhythmScrobbler.Models;

public class RhythmScrobble
{
    public string Guid { get; set; } = System.Guid.NewGuid().ToString().Substring(0, 10);
    public string Artist { get; set; }
    public string Track { get; set; }
    public string Album { get; set; }

    public DateTime Date { get; set; }

    public bool Accepted { get; set; }
    public string ImageUrl { get; set; }
    public string AdditionalInfo { get; set; }

    public RhythmScrobble()
    {
    }

    public RhythmScrobble(Scrobble scrobble)
    {
        Artist = scrobble.Artist;
        Track = scrobble.Track;
        Album = scrobble.Album;
        Date = scrobble.Date;
    }

    public override string ToString()
    {
        return $"Artist: {Artist}\nSong: {Track}\nAlbum: {Album}";
    }

    protected bool Equals(RhythmScrobble other)
    {
        return Track == other.Track && Artist == other.Artist && Album == other.Album;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((RhythmScrobble)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Track, Artist, Album);
    }
}