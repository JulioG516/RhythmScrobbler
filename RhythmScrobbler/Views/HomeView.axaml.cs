using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using RhythmScrobbler.ViewModels;

namespace RhythmScrobbler.Views;

public partial class HomeView : ReactiveUserControl<HomeViewModel>
{
    public HomeView()
    {
        this.WhenActivated(disposables => { });
        AvaloniaXamlLoader.Load(this);
    }
}