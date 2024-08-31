using System;
using System.Collections.Generic;
using LiteDB;
using RhythmScrobbler.Configs;
using RhythmScrobbler.Models;
using Splat;
using Scrobble = Hqub.Lastfm.Entities.Scrobble;

namespace RhythmScrobbler.Services;

public class DbService
{
    private readonly Lazy<LiteDatabase> _lazyDb;
    private LiteDatabase Database => _lazyDb.Value;

    public DbService()
    {
        var config = Locator.Current.GetService<LiteDbConfig>();

        _lazyDb = new Lazy<LiteDatabase>(() => new LiteDatabase(config!.DatabasePath));
        // EnsureIndexes();
    }

    private void EnsureIndexes()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        col.EnsureIndex(x => x.Artist);
    }

    public bool InsertScrobble(RhythmScrobble scrobble)
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        var id = col.Insert(scrobble);

        if (id != null)
            return true;

        return false;
    }

    public List<RhythmScrobble> GetScrobbles()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        return col.Query().OrderBy(s => s.Date).ToList();
    }

    public void DeleteAllScrobbles()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        col.DeleteAll();
    }


    public void Test()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        // Create your new customer instance
        var scrobble = new Scrobble()
        {
            Artist = "King Crimson",
            Track = "21st Schizoid Man",
            Album = "The Court of King Crimson",
            Date = DateTime.Now
        };

        var rhythmScrobble = new RhythmScrobble(scrobble);


        // Insert new customer document (Id will be auto-incremented)
        col.Insert(rhythmScrobble);


        var results = col.Query().ToList();
    }
}