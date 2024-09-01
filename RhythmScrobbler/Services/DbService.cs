using System;
using System.Collections.Generic;
using System.Diagnostics;
using LiteDB;
using RhythmScrobbler.Configs;
using RhythmScrobbler.Helpers;
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
        // DeleteOldScrobblesHisttory();
    }

    private void DeleteOldScrobblesHisttory()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        var x = col.DeleteMany(s => s.Date <= DateTime.Now.AddDays(-3));
    }

    private void EnsureIndexes()
    {
        var col = Database.GetCollection<RhythmScrobble>("Scrobbles");
        col.EnsureIndex(x => x.Artist);

        var colGames = Database.GetCollection<Game>("Games");
        colGames.EnsureIndex(x => x.Type);
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

    public UserConfig? RetrieveUserConfig()
    {
        var col = Database.GetCollection<UserConfig>("UserConfig");
        return col.Query().FirstOrDefault();
    }

    public void SaveUserConfig(UserConfig userConfig)
    {
        var col = Database.GetCollection<UserConfig>("UserConfig");
        col.DeleteAll();
        col.Insert(userConfig);
    }

    public void DeleteUserConfig()
    {
        var col = Database.GetCollection<UserConfig>("UserConfig");
        col.DeleteAll();
    }

    public void InsertGame(Game game)
    {
        var col = Database.GetCollection<Game>("Games");
        var gameDb = col.Query().Where(g => g.Type == game.Type).FirstOrDefault();
        if (gameDb != null)
        {
            col.DeleteMany(g => g.Type == game.Type);
        }

        col.Insert(game);
    }

    public List<Game> RetrieveGames()
    {
        var col = Database.GetCollection<Game>("Games");
        return col.Query().ToList();
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