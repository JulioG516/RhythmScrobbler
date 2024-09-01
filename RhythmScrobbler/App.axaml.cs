using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.Configuration;
using RhythmScrobbler.Configs;
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
        
        // Config from AppSettings.json
        var builder = new ConfigurationBuilder()
            .SetBasePath(System.AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json",
                optional: true,
                reloadOnChange: true);

        var config = builder.Build();

        var lastFm = config.GetSection("LastFM");
        var lastFmConfig = new LastFmConfig()
        {
            ApiKey = lastFm["ApiKey"]!,
            SharedSecret = lastFm["SharedSecret"]!
        };

        var dbConfig = new LiteDbConfig { DatabasePath = AppContext.BaseDirectory + @"\RhythmScrobbler.db" };


        Locator.CurrentMutable.RegisterConstant(dbConfig, typeof(LiteDbConfig));
        Locator.CurrentMutable.RegisterLazySingleton(() => new DbService(),
            typeof(DbService));

        Locator.CurrentMutable.RegisterConstant(lastFmConfig, typeof(LastFmConfig));
        Locator.CurrentMutable.RegisterConstant(new LastFmService(), typeof(LastFmService));
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