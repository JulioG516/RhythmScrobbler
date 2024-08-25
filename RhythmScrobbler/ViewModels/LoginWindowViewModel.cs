using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Hqub.Lastfm.Entities;
using ReactiveUI;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

public class LoginWindowViewModel : ViewModelBase
{
    public LoginWindowViewModel()
    {
        var canExecute = this.WhenAnyValue(
            x => x.Username, x => x.Password,
            (userName, password) =>
                !string.IsNullOrEmpty(userName) &&
                !string.IsNullOrEmpty(password));

        LoginCommand = ReactiveCommand.CreateFromTask<bool>(async () => { return await LoginAsync(); }, canExecute);
        CloseCommand = ReactiveCommand.Create(() => false);
        
        _lastFm = Locator.Current.GetService<LastFmService>();
    }

    private LastFmService _lastFm;

    private string _username;

    public string Username
    {
        get => _username;
        set => this.RaiseAndSetIfChanged(ref _username, value);
    }

    private string _password;

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    public ReactiveCommand<Unit, bool> CloseCommand { get; }

    public ReactiveCommand<Unit, bool> LoginCommand { get; }

    private async Task<bool> LoginAsync()
    {
        try
        {
            await _lastFm.Authenticate(Username, Password);

            _lastFm.Username = Username;
            _lastFm.Password = Password;

            return false;
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return false;
        }
    }
}