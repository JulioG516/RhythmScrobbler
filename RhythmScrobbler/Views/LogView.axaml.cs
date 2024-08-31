using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RhythmScrobbler.ViewModels;

namespace RhythmScrobbler.Views;

public partial class LogView : ReactiveUserControl<LogViewModel>
{
    public LogView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}