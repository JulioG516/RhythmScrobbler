using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hqub.Lastfm.Entities;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

public class LogViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public IScreen HostScreen { get; }

    private DbService _dbService;
    private LastFmService _lastFmService;
    
    public ObservableCollection<RhythmScrobble> ScrobblesCollection { get; }= new ObservableCollection<RhythmScrobble>();
    
    public LogViewModel()
    {
        
    }
    public LogViewModel(IScreen screen)
    {
        HostScreen = screen;
        _dbService = Locator.Current.GetService<DbService>()!;
        _lastFmService = Locator.Current.GetService<LastFmService>()!;
        
        TestCommand = ReactiveCommand.Create(TestDb);

        ScrobblesCollection = new ObservableCollection<RhythmScrobble>(_dbService.GetScrobbles());
        
        // Fetch additional info for scrobbles
        FetchAdditionalInfoForScrobblesAsync().ConfigureAwait(false);
    }
    
    private async Task FetchAdditionalInfoForScrobblesAsync()
    {
        var tasks = ScrobblesCollection.Select(async scrobble =>
        {
            var trackInfo = await _lastFmService.FetchTrackInfoAsync(scrobble.Artist, scrobble.Track);
            scrobble.ImageUrl = trackInfo.Images.First().Url; // Assuming TrackInfo has an ImageUrl property
        });

        await Task.WhenAll(tasks);
    }

    
    public ICommand TestCommand { get; }

    private void TestDb()
    {
        _dbService.Test();;
    }
}