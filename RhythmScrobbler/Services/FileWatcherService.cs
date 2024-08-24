using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.Models;

namespace RhythmScrobbler.Services;

public class FileWatcherService : IDisposable
{
    private readonly FileSystemWatcher _watcher;
    private string _path;

    public event FileSystemEventHandler FileChanged;
    public event EventHandler<ScrobbleChangedEventArgs> ScrobbleChanged;

    // private readonly MemoryCache _memCache;
    // private readonly CacheItemPolicy _cacheItemPolicy;
    // private const int CacheTimeMilliseconds = 1000;

    public FileWatcherService(string directoryPath, EnumGameType gameType)
    {
        _path = directoryPath;
        var fileName = gameType switch
        {
            EnumGameType.CloneHero => Constants.CloneHeroFileName,
            EnumGameType.YARG => "YARG.txt",
            _ => string.Empty
        };


        _watcher = new FileSystemWatcher
        {
            Path = directoryPath,
            Filter = fileName, // Set the file extension to monitor (optional)
            NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Attributes,
            EnableRaisingEvents = true
        };

        // _memCache = MemoryCache.Default;
        // _cacheItemPolicy = new CacheItemPolicy()
        // {
        //     RemovedCallback = OnRemovedFromCache
        // };

        // Subscribe to events
        _watcher.Changed += OnChanged;
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        // _cacheItemPolicy.AbsoluteExpiration =
        //     DateTimeOffset.Now.AddMilliseconds(CacheTimeMilliseconds);
        //
        // // Only add if it is not there already (swallow others)
        // _memCache.AddOrGetExisting(e.Name, e, _cacheItemPolicy);

        Debug.WriteLine(e);
        if (e.ChangeType != WatcherChangeTypes.Changed)
            return;
        
        // Debug.WriteLine($"Changed: {e.FullPath}");
        
        Thread.SpinWait(15);
        
        var x = File.ReadAllLines(e.FullPath);
        if (x.Length > 0)
        {
            // Debug.WriteLine("Mudancas: ");
            // foreach (var se in x)
            // {
            //     Debug.WriteLine(se);
            // }
        
            this.ScrobbleChanged?.Invoke(sender,
                new ScrobbleChangedEventArgs()
                {
                    Scrobble = new Scrobble()
                    {
                        SongName = x[0],
                        Artist = x[1],
                        Album = x[2]
                    }
                });
        }

        // this.FileChanged?.Invoke(sender, e);
    }

    // // Handle cache item expiring
    // private void OnRemovedFromCache(CacheEntryRemovedArguments args)
    // {
    //     if (args.RemovedReason != CacheEntryRemovedReason.Expired) return;
    //
    //     // Now actually handle file event
    //     var e = (FileSystemEventArgs)args.CacheItem.Value;
    //
    //     if (e.ChangeType != WatcherChangeTypes.Changed)
    //         return;
    //
    //
    //     Debug.WriteLine($"Changed: {e.FullPath}");
    //
    //     Thread.SpinWait(10);
    //
    //     var x = File.ReadAllLines(e.FullPath);
    //     if (x.Length > 0)
    //     {
    //         // Debug.WriteLine("Mudancas: ");
    //         // foreach (var se in x)
    //         // {
    //         //     Debug.WriteLine(se);
    //         // }
    //
    //         this.ScrobbleChanged?.Invoke(this,
    //             new ScrobbleChangedEventArgs()
    //             {
    //                 Scrobble = new Scrobble()
    //                 {
    //                     SongName = x[0],
    //                     Artist = x[1],
    //                     Album = x[2]
    //                 }
    //             });
    //     }
    // }

    public void Dispose()
    {
        _watcher.Dispose();
    }
}