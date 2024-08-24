using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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


        // Convert the collection to an observable change set
        var changeSet = Games.ToObservableChangeSet(vm => vm);

        // Automatically refresh when the 'Path' property changes
        var autoRefreshedChangeSet = changeSet.AutoRefresh(x => x.IsWatcherToggled);

        // Subscribe to changes and check if all properties are true or false
        var collectionChanged = autoRefreshedChangeSet
            .ToCollection()
            .Select(x => x)
            .Subscribe(viewModels =>
            {
                bool allTrue = viewModels.All(vm => vm.IsWatcherToggled);
                Debug.WriteLine(allTrue);
                
                if (allTrue)
                {
                    this.IsEnabled = true;
                }
                // else
                // {
                //     this.IsEnabled = false;
                // }
            });
        
        // // Subscribe to changes
        // autoRefreshedChangeSet.Subscribe(change =>
        // {
        //     // Handle changes (e.g., print to debug)
        //     foreach (var itemChange in change)
        //     {
        //         var viewModel = itemChange.Current;
        //         Debug.WriteLine($"Path changed in ViewModel: {viewModel.Path}");
        //     }
        // });
    }


    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    // Unique identifier for the routable view model.
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    private ObservableCollection<GameViewModel> _games = new()
    {
        new GameViewModel(new Game() { Name = "Clone Hero", Type = EnumGameType.CloneHero }),
        new GameViewModel(new Game() { Name = "Yarg", Type = EnumGameType.YARG }),
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

     
        // foreach (var gameViewModel in Games)
        // {
        //     gameViewModel.IsWatcherToggled = IsEnabled;
        // }
    }

    // public ObservableCollection<GameViewModel> Games { get; } = new ObservableCollection<GameViewModel>
    // {
    //     new GameViewModel(new Game() { Name = "Clone Hero", Type = GameType.CloneHero }),
    //     new GameViewModel(new Game() { Name = "YARG", Type = GameType.YARG }),
    // };
}