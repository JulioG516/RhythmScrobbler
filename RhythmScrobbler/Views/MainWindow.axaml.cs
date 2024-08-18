using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RhythmScrobbler.ViewModels;

namespace RhythmScrobbler.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        this.WhenActivated(disposable => { });
        AvaloniaXamlLoader.Load(this);
        this.AttachDevTools();
    }
}