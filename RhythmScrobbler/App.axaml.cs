using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Hqub.Lastfm;
using Microsoft.Extensions.Configuration;
using RhythmScrobbler.Services;
using RhythmScrobbler.ViewModels;
using RhythmScrobbler.Views;
using Splat;

namespace RhythmScrobbler;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        // Singleton
        // Locator.CurrentMutable.RegisterConstant(new FileDialogService(), typeof(FileDialogService));

        // Config from AppSettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(System.AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json",
                optional: true,
                reloadOnChange: true);

        var config = builder.Build();

        var lastFm = config.GetSection("LastFM");
        Locator.CurrentMutable.RegisterConstant(lastFm, typeof(IConfigurationSection));
        
        // var lastfmClient = new LastfmClient(lastFm["ApiKey"], lastFm["SharedSecret"]);
        //
        // Locator.CurrentMutable.RegisterConstant(lastfmClient, typeof(LastfmClient));
        Locator.CurrentMutable.RegisterConstant(new LastFmService(lastFm), typeof(LastFmService));
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}