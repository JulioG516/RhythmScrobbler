using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.Models;
using RhythmScrobbler.Services;

namespace RhythmScrobbler.ViewModels;

//TODO: Make the SelectPath command open a dialog
//TODO: Make the switch command and make the logic to watch the song
public class GameViewModel : ViewModelBase
{
    public GameViewModel()
    {
    }

    public GameViewModel(Game game)
    {
        Name = game.Name;
        Path = "";//game.Path;
        Type = game.Type;

        CurrentScrobble = new Scrobble();
        
        // Observable on Path => changed != null or "" create new Service
        SelectPath = ReactiveCommand.CreateFromTask(async () =>
        {
            var folder = await Interactions.GetFolderDialog.Handle(Unit.Default);

            if (!string.IsNullOrEmpty(folder))
            {
                Debug.WriteLine(folder);
                Path = folder;
                _watcherService = new FileWatcherService(Path, Type);
                _watcherService.FileChanged += OnChanged;
                _watcherService.ScrobbleChanged += OnScrobbleChanged;
            }
        });
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);

        ScrobbleCommand = ReactiveCommand.Create(() =>
        {
            Debug.WriteLine($"Scrobble Command: {Path}");
        });


        // this.WhenAnyValue(x => x.CurrentScrobble)
        //     .Where(x => !x.Equals(CurrentScrobble))
        //     .ToProperty(this, x => x.CurrentScrobble);
        //
        //  this.WhenAnyValue(x => x.CurrentScrobble)
        //      .DistinctUntilChanged() // Filters out consecutive duplicate values
        //      .ToProperty(this, x => x.CurrentScrobble);
        // 
        
        // TODO: Metodo para LAST.Fm
        this.WhenAnyValue(x => x.CurrentScrobble)
            .Subscribe(newValue => Debug.WriteLine($"Scrobble changed to: {newValue}"));

    }

    private FileWatcherService _watcherService;
    public ICommand SelectPath { get; }

    public ReactiveCommand<Unit, Unit> ToggleWatcher { get; }
    
    public ICommand ScrobbleCommand { get; }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        Debug.WriteLine("Recebi no VM.");
    }

    private void OnScrobbleChanged(object sender, ScrobbleChangedEventArgs e)
    {
        // Debug.WriteLine(e.Scrobble);
        CurrentScrobble = e.Scrobble;
    }
    
    private void ToggleEnabled()
    {
        if (!string.IsNullOrEmpty(Path))
        {
            Debug.WriteLine("Tem path.");
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
        set
        {
            if (_currentScrobble != value)
            {
                this.RaiseAndSetIfChanged(ref _currentScrobble, value);
            }
        }
    }

}