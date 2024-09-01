using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using RhythmScrobbler.Helpers;

namespace RhythmScrobbler.Services;

public class FileWatcherService : IDisposable
{
    private readonly FileSystemWatcher _watcher;

    private readonly EnumGameType _type;
    public event EventHandler<ScrobbleChangedEventArgs> ScrobbleChanged;


    public FileWatcherService(string directoryPath, EnumGameType gameType)
    {
        // _Scrobble = "";
        var fileName = gameType switch
        {
            EnumGameType.CloneHero => Constants.CloneHeroFileName,
            EnumGameType.YARG => Constants.YargFileName,
            _ => string.Empty
        };
        _type = gameType;


        _watcher = new FileSystemWatcher
        {
            Path = directoryPath,
            Filter = fileName, // Set the file extension to monitor (optional)
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes,
            EnableRaisingEvents = true
        };


        // Subscribe to events
        _watcher.Changed += OnChanged;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Debug.WriteLine(e);
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;

        if (_type == EnumGameType.CloneHero)
        {
            Debug.WriteLine("Clone Hero");
        }
        else
        {
            Debug.WriteLine("YARG");
        }


        Thread.SpinWait(15);


        if (_type == EnumGameType.CloneHero)
        {
            var x = File.ReadAllLines(e.FullPath);
            if (x.Length > 0)
            {
                var rhythmScrobble = new RhythmScrobble()
                {
                    Track = x[0],
                    Artist = x[1],
                    Album = x[2]
                };

                ScrobbleChanged?.Invoke(this,
                    new ScrobbleChangedEventArgs { RhythmScrobble = rhythmScrobble });
            }
        }
        else if (_type == EnumGameType.YARG)
        {
            var jsonString = File.ReadAllText(e.FullPath);
            if (string.IsNullOrEmpty(jsonString))
                return;

            var songInfo = JsonSerializer.Deserialize<YargSongInfo>(jsonString);

            if (songInfo != null)
            {
                var rhythmScrobble = new RhythmScrobble
                {
                    Track = songInfo.Name,
                    Artist = songInfo.Artist,
                    Album = songInfo.Album
                };

                ScrobbleChanged?.Invoke(this,
                    new ScrobbleChangedEventArgs { RhythmScrobble = rhythmScrobble });
            }
        }
    }


    public void Dispose()
    {
        _watcher.Dispose();
    }
}