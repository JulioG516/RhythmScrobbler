using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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
        Locator.CurrentMutable.RegisterConstant(new FileDialogService(), typeof(FileDialogService));
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