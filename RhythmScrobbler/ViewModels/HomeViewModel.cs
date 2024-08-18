using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Views;

namespace RhythmScrobbler.ViewModels;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    public HomeViewModel()
    {
    }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;

        ToggleWatchersCommand = ReactiveCommand.Create(ToggleWatcher);
        
    }
    
    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    private ObservableCollection<GameViewModel> _games = new()
    {
        new GameViewModel(new Game() { Name = "Clone Hero", Type = GameType.CloneHero }),
        new GameViewModel(new Game() { Name = "YARG", Type = GameType.YARG }),
    };

    public ObservableCollection<GameViewModel> Games
    {
        get => _games;
        set => this.RaiseAndSetIfChanged(ref _games, value);
    }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }
    
    
    public ReactiveCommand<Unit, Unit> ToggleWatchersCommand { get; }

    private void ToggleWatcher()
    {
        IsEnabled = !IsEnabled;
    }

    // public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>
    // {
    //     new GameViewModel(new Game() { Name = "Clone Hero", Type = GameType.CloneHero }),
    //     new GameViewModel(new Game() { Name = "YARG", Type = GameType.YARG }),
    // };

}