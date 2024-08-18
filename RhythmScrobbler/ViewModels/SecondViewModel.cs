using System;
using ReactiveUI;

namespace RhythmScrobbler.ViewModels;

public class SecondViewModel : ReactiveObject, IRoutableViewModel
{
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

    public IScreen HostScreen { get; }


    public SecondViewModel()
    {
        
    }
    public SecondViewModel(IScreen screen)
    {
        HostScreen = screen;
    }
}