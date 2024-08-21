using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Services;
using Splat;

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
        Path = game.Path;
        Type = game.Type;
        
        SelectPath = ReactiveCommand.Create(SelectAnPath);
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);

    }
    
    public ReactiveCommand<Unit, Unit> SelectPath { get; }

    public ReactiveCommand<Unit, Unit> ToggleWatcher { get; }


    private void SelectAnPath()
    {
        Debug.WriteLine("Path Clicked " + Name);
        Path = Guid.NewGuid().ToString().Substring(0, 10);
    }

    private void ToggleEnabled()
    {
        Debug.WriteLine("Watcher toggle click");
        IsWatcherToggled = !IsWatcherToggled;
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

    private GameType _type;

    public GameType Type
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