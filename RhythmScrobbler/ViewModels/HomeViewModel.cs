using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using RhythmScrobbler.Configs;
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
        _lastFmService = Locator.Current.GetService<LastFmService>()!;


        //TODO: Persistir Paths nos <GameViewModels>, Username e Senha do <LastFMService> 
        ToggleWatchersCommand = ReactiveCommand.Create(ToggleWatcher);
        LoginCommand = ReactiveCommand.CreateFromTask(OpenLoginAsync);
        LogoutCommand = ReactiveCommand.Create(Logout);

        LoadUserConfigAsync().ConfigureAwait(false);
    }

    // Reference to IScreen that owns the routable view model.
    public IScreen HostScreen { get; }

    private LastFmService _lastFmService;

    private UserConfig _userConfig = new();
    private readonly string UserConfigPath = "userConfig.json";

    private async Task LoadUserConfigAsync()
    {
        try
        {
            if (File.Exists(UserConfigPath))
            {
                await using FileStream openStream = File.Open(UserConfigPath, FileMode.Open);
                var user = await JsonSerializer.DeserializeAsync<UserConfig>(openStream);

                if (user == null)
                    return;

                _userConfig.Username = user.Username;
                _userConfig.Password = user.Password;

                IsUserLogged = await _lastFmService.Authenticate(_userConfig.Username, _userConfig.Password);
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            await Interactions.Errors.Handle(e);
        }
    }

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
        if (IsUserLogged)
        {
            _userConfig = new UserConfig()
            {
                Username = _lastFmService.Username,
                Password = _lastFmService.Password
            };

            await using FileStream createStream = File.Create(UserConfigPath);
            await JsonSerializer.SerializeAsync(createStream, _userConfig);
        }


        Debug.WriteLine($"Result do login: {IsUserLogged}");
    }

    public ICommand LogoutCommand { get; }

    public void Logout()
    {
        _lastFmService.Logout();
        IsUserLogged = false;
        if (File.Exists(UserConfigPath))
            File.Delete(UserConfigPath);
    }
}