using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
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


    private ObservableCollection<RhythmScrobbleViewModel> _ScrobblesCollection;
    
    public ObservableCollection<RhythmScrobbleViewModel> ScrobblesCollection
    {
        get => _ScrobblesCollection;
        set => this.RaiseAndSetIfChanged(ref _ScrobblesCollection, value);
    }

    
    // public ObservableCollection<RhythmScrobbleViewModel> ScrobblesCollection { get; set; }    
    
    public LogViewModel()
    {
        
    }
    public LogViewModel(IScreen screen)
    {
        HostScreen = screen;
        _dbService = Locator.Current.GetService<DbService>()!;
        _lastFmService = Locator.Current.GetService<LastFmService>()!;
        
        DeleteAllCommand = ReactiveCommand.Create(DeleteAll);
        ReloadCommand = ReactiveCommand.Create(Reload);
        
        var scrobbles = _dbService.GetScrobbles();
        
        ScrobblesCollection = new ObservableCollection<RhythmScrobbleViewModel>(
            scrobbles.Select(s => new RhythmScrobbleViewModel(s))
        );
        
        // Fetch additional info for scrobbles
        FetchAdditionalInfoForNewScrobblesAsync(scrobbles).ConfigureAwait(false);
        // Reload();
    }

    public ICommand ReloadCommand { get; }
    
    private void Reload()
    {
        var scrobbles = _dbService.GetScrobbles();
        var newScrobbles = scrobbles.Where(s => !ScrobblesCollection.Any(existing => existing.Id == s.Guid)).ToList();

        foreach (var scrobble in newScrobbles)
        {
            ScrobblesCollection.Add(new RhythmScrobbleViewModel(scrobble));
        }

        // Fetch additional info for new scrobbles
        FetchAdditionalInfoForNewScrobblesAsync(newScrobbles).ConfigureAwait(false);
    }
    
    private async Task FetchAdditionalInfoForNewScrobblesAsync(List<RhythmScrobble> newScrobbles)
    {
        var tasks = newScrobbles.Select(async scrobble =>
        {
            var trackInfo = await _lastFmService.FetchTrackInfoAsync(scrobble.Artist, scrobble.Track);
            var viewModel = ScrobblesCollection.First(vm => vm.Id == scrobble.Guid);
            viewModel.ImageUrl = trackInfo.Album.Images.Find(x => x.Size.Equals("extralarge"))?.Url ?? string.Empty;
        });

        await Task.WhenAll(tasks);
    }
    private async Task FetchAdditionalInfoForScrobblesAsync()
    {
        var tasks = ScrobblesCollection.Select(async scrobble =>
        {
            var trackInfo = await _lastFmService.FetchTrackInfoAsync(scrobble.Artist, scrobble.Track);
            scrobble.ImageUrl = trackInfo.Album.Images.Find(x => x.Size.Equals("extralarge"))?.Url ?? string.Empty; // Assuming TrackInfo has an ImageUrl property
        });

        await Task.WhenAll(tasks);
    }

    
    public ICommand DeleteAllCommand { get; }

    private void DeleteAll()
    {
        _dbService.DeleteAllScrobbles();;
    }
    
    
}