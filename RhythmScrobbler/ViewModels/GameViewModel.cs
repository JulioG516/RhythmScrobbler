using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

public class GameViewModel : ViewModelBase
{
    public GameViewModel()
    {
    }

    public GameViewModel(Game game)
    {
        Name = game.Name;
        Path = ""; //game.Path;
        Type = game.Type;

        _lastFm = Locator.Current.GetService<LastFmService>()!;

        CurrentRhythmScrobble = new RhythmScrobble();

        // Observable on Path => changed != null or "" create new Service
        SelectPathCommand = ReactiveCommand.CreateFromTask(SelectPath);
        
        // CreateFromTask
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);


        // TODO: Metodo para LAST.Fm
        this.WhenAnyValue(x => x.CurrentRhythmScrobble)
            .Where(x => !string.IsNullOrEmpty(x.Track)
                        && !string.IsNullOrEmpty(x.Album)
                        && !string.IsNullOrEmpty(x.Artist)
                        && IsWatcherToggled
            )
            .Distinct()
            .Throttle(TimeSpan.FromSeconds(5))
            .Subscribe(OnNextSong);
    }

    private async void OnNextSong(RhythmScrobble newValue)
    {
        await _lastFm.ScrobbleTrack(newValue.Track, newValue.Artist, newValue.Album);
        // Debug.WriteLine($"Scrobble changed to: {newValue}");
    }

    private LastFmService _lastFm;
    private FileWatcherService _watcherService;
    public ICommand SelectPathCommand { get; }
    private async Task SelectPath()
    {
        var folder = await Interactions.GetFolderDialog.Handle(Unit.Default);
        
        if (!string.IsNullOrEmpty(folder))
        {
            Debug.WriteLine(folder);
            Path = folder;
            _watcherService = new FileWatcherService(Path, Type);
            // _watcherService.FileChanged += OnChanged;
            _watcherService.ScrobbleChanged += OnScrobbleChanged;
        }
    }
    
    
    
    public ReactiveCommand<Unit, Unit> ToggleWatcher { get; }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Debug.WriteLine("Recebi no VM.");
    }

    private void OnScrobbleChanged(object? sender, ScrobbleChangedEventArgs e)
    {
        // Debug.WriteLine(e.Scrobble);
        CurrentRhythmScrobble = e.RhythmScrobble;
    }

    private void ToggleEnabled()
    {
        if (!string.IsNullOrEmpty(Path))
        {
            IsWatcherToggled = !IsWatcherToggled;
        }
    }

    private string _name;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _path;

    public string Path
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }

    private EnumGameType _type;

    public EnumGameType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    private bool _isWatcherToggled;

    public bool IsWatcherToggled
    {
        get => _isWatcherToggled;
        set => this.RaiseAndSetIfChanged(ref _isWatcherToggled, value);
    }


    private RhythmScrobble _currentRhythmScrobble;

    public RhythmScrobble CurrentRhythmScrobble
    {
        get => _currentRhythmScrobble;
        set => this.RaiseAndSetIfChanged(ref _currentRhythmScrobble, value);
        // {
        //     if (_currentScrobble != value)
        //     {
        //         this.RaiseAndSetIfChanged(ref _currentScrobble, value);
        //     }
        // }
    }
}