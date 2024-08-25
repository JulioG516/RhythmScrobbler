using System.Diagnostics;
using System.Linq;
using System.Reactive;
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

        Interactions.Errors.RegisterHandler(async interaction =>
        {
            var dialog = new DialogWindow();

            dialog.DataContext = new DialogWindowViewModel() { Message = interaction.Input.Message };
            Debug.WriteLine($"Erro Message: {interaction.Input.Message}");

            await dialog.ShowDialog(this);
            interaction.SetOutput(Unit.Default);
        });

        Interactions.LoginDialog.RegisterHandler(async interaction =>
        {
            var vm = new LoginWindowViewModel();
            var dialog = new LoginWindow();
            dialog.DataContext = vm;

            var result = await dialog.ShowDialog<bool>(this);
            interaction.SetOutput(result);
            
        });
    }
}