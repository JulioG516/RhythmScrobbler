using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RhythmScrobbler.ViewModels;

namespace RhythmScrobbler.Views;

public partial class SecondView : ReactiveUserControl<SecondViewModel>
{
    public SecondView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}