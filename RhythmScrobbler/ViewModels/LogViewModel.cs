using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

public class LogViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public IScreen HostScreen { get; }

    private DbService _dbService;
    private LastFmService _lastFmService;


    private ObservableCollection<RhythmScrobbleViewModel> _scrobblesCollection;

    public ObservableCollection<RhythmScrobbleViewModel> ScrobblesCollection
    {
        get => _scrobblesCollection;
        set => this.RaiseAndSetIfChanged(ref _scrobblesCollection, value);
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

        var canExecuteDelete = this.WhenAnyValue(s => s.ScrobblesCollection)
            .Select(scrobbles => scrobbles != null && scrobbles.Count > 0);


        DeleteAllCommand = ReactiveCommand.Create(DeleteAll, canExecuteDelete);
        ReloadCommand = ReactiveCommand.Create(Reload);


        Load();
    }

    public ICommand ReloadCommand { get; }

    private void Load()
    {
        var scrobbles = _dbService.GetScrobbles();

        ScrobblesCollection = new ObservableCollection<RhythmScrobbleViewModel>(
            scrobbles.Select(s => new RhythmScrobbleViewModel(s))
        );

        // Fetch additional info for scrobbles
        FetchAdditionalInfoForNewScrobblesAsync(scrobbles).ConfigureAwait(false);
    }
    
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

    public ICommand DeleteAllCommand { get; }

    private void DeleteAll()
    {
        _dbService.DeleteAllScrobbles();
        Load();
    }
}