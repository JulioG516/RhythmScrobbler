using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RhythmScrobbler.Helpers;
using RhythmScrobbler.ViewModels;

namespace RhythmScrobbler.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposable => { });
        AvaloniaXamlLoader.Load(this);
        this.AttachDevTools();

        Interactions.GetFolderDialog.RegisterHandler(
            async interaction =>
            {
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    Debug.WriteLine("Desktop");
                    
                    var options = new FolderPickerOpenOptions()
                    {
                        AllowMultiple = false,
                        Title = "Select the game installation folder."
                    };

                    var folder = await desktop.MainWindow.StorageProvider.OpenFolderPickerAsync(options);

                    interaction.SetOutput(folder.Any() ? folder.First().Path.LocalPath : null);
                }
            });
    }
}