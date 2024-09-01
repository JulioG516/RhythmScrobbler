using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

public class NewGameViewModel : ViewModelBase
{
    public NewGameViewModel()
    {
    }

    public NewGameViewModel(Game game)
    {
        _name = game.Name;
        _path = game.Path;
        _type = game.Type;

        if (_type == EnumGameType.CloneHero)
        {
            Debug.WriteLine("instanciei o clone hero.");
        }

        SelectPath = ReactiveCommand.Create(SelectAnPath);
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);

    }


    public ReactiveCommand<Unit, Unit> SelectPath { get; }

    public ReactiveCommand<Unit, Unit> ToggleWatcher { get; }


    private void SelectAnPath()
    {
        Debug.WriteLine("Path Clicked " + Name);
    }

    private void ToggleEnabled()
    {
        Debug.WriteLine("Watcher toggle click");
        _isWatcherToggled = !_isWatcherToggled;
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
}