using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Helpers;
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

        CurrentScrobble = new Scrobble();

        // Observable on Path => changed != null or "" create new Service
        SelectPathCommand = ReactiveCommand.CreateFromTask(SelectPath);
        
        // CreateFromTask
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);


        // TODO: Metodo para LAST.Fm
        this.WhenAnyValue(x => x.CurrentScrobble)
            .Where(x => !string.IsNullOrEmpty(x.SongName)
                        && !string.IsNullOrEmpty(x.Album)
                        && !string.IsNullOrEmpty(x.Artist)
                        && IsWatcherToggled
            )
            .Distinct()
            .Throttle(TimeSpan.FromSeconds(5))
            .Subscribe(OnNextSong);
    }

    private async void OnNextSong(Scrobble newValue)
    {
        await _lastFm.ScrobbleTrack(newValue.SongName, newValue.Artist, newValue.Album);
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
        CurrentScrobble = e.Scrobble;
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


    private Scrobble _currentScrobble;

    public Scrobble CurrentScrobble
    {
        get => _currentScrobble;
        set => this.RaiseAndSetIfChanged(ref _currentScrobble, value);
        // {
        //     if (_currentScrobble != value)
        //     {
        //         this.RaiseAndSetIfChanged(ref _currentScrobble, value);
        //     }
        // }
    }
}