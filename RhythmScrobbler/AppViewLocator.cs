using System;
using ReactiveUI;
using RhythmScrobbler.ViewModels;
using RhythmScrobbler.Views;

namespace RhythmScrobbler;

public class AppViewLocator : ReactiveUI.IViewLocator
{
    public IViewFor? ResolveView<T>(T? viewModel, string? contract = null) => viewModel switch
    {
        FirstViewModel context => new FirstView { DataContext = context },
        _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        

    };
    
 
}