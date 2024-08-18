using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Views;

namespace RhythmScrobbler.ViewModels;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>
    {
        new GameViewModel(new Game() {Name = "Clone Hero"}),
        new GameViewModel(new Game(){Name = "YARG"}),
    };
    
    public HomeViewModel()
    {
    }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}