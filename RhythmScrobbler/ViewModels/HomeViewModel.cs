using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    public HomeViewModel()
    {
    }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        _lastFmService = Locator.Current.GetService<LastFmService>();

        //TODO: Persistir Paths nos <GameViewModels>, Username e Senha do <LastFMService> 
        ToggleWatchersCommand = ReactiveCommand.Create(ToggleWatcher);
        LoginCommand = ReactiveCommand.CreateFromTask(OpenLoginAsync);
    }

    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    private LastFmService _lastFmService;

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

    private bool _isUserLogged;

    public bool IsUserLogged
    {
        get => _isUserLogged;
        set => this.RaiseAndSetIfChanged(ref _isUserLogged, value);
    }

    public ReactiveCommand<Unit, Unit> ToggleWatchersCommand { get; }

    private void ToggleWatcher()
    {
        // IsEnabled = !IsEnabled;
        _lastFmService.Username = "Meu Mundo";
    }

    public ICommand LoginCommand { get; }

    private async Task OpenLoginAsync()
    {
        IsUserLogged = await Interactions.LoginDialog.Handle(Unit.Default);
        Debug.WriteLine($"Result do login: {IsUserLogged}");
    }
}