using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Configs;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.Services;
using Splat;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace RhythmScrobbler.ViewModels;

public class HomeViewModel : ReactiveObject, IRoutableViewModel
{
    public HomeViewModel()
    {
    }

    public HomeViewModel(IScreen screen)
    {
        HostScreen = screen;
        _lastFmService = Locator.Current.GetService<LastFmService>()!;
        _dbService = Locator.Current.GetService<DbService>()!;

        var games = _dbService.RetrieveGames();
        if (games.Count != 0)
        {
            foreach (var game in games)
            {
                var existingGameViewModel = _games.FirstOrDefault(gvm => gvm.Type.Equals(game.Type));
                if (existingGameViewModel != null)
                {
                    existingGameViewModel.Path = game.Path;
                }
            }
        }


        LoginCommand = ReactiveCommand.CreateFromTask(OpenLoginAsync);
        LogoutCommand = ReactiveCommand.Create(Logout);

        LoadUserConfigAsync().ConfigureAwait(false);
    }

    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    private readonly LastFmService _lastFmService;

    private readonly DbService _dbService;

    private UserConfig _userConfig = new();


    private async Task LoadUserConfigAsync()
    {
        try
        {
            var user = _dbService.RetrieveUserConfig();
            if (user == null)
                return;

            _userConfig.Username = user.Username;
            _userConfig.Password = user.Password;

            IsUserLogged = await _lastFmService.Authenticate(_userConfig.Username, _userConfig.Password);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            await Interactions.Errors.Handle(e);
        }
    }

    // Unique identifier for the routable view model.
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);


    private ObservableCollection<GameViewModel> _games =
    [
        new GameViewModel(new Game() { Name = "Clone Hero", Type = EnumGameType.CloneHero }),
        new GameViewModel(new Game() { Name = "Yarg", Type = EnumGameType.YARG })
    ];

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

    // public ReactiveCommand<Unit, Unit> ToggleWatchersCommand { get; }


    public ICommand LoginCommand { get; }

    private async Task OpenLoginAsync()
    {
        IsUserLogged = await Interactions.LoginDialog.Handle(Unit.Default);
        if (IsUserLogged)
        {
            _userConfig = new UserConfig()
            {
                Username = _lastFmService.Username,
                Password = _lastFmService.Password
            };

            _dbService.SaveUserConfig(_userConfig);
        }


        Debug.WriteLine($"Result do login: {IsUserLogged}");
    }

    public ICommand LogoutCommand { get; }

    private void Logout()
    {
        _lastFmService.Logout();
        IsUserLogged = false;
        _dbService.DeleteUserConfig();
    }
}