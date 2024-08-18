using System.Diagnostics;
using System.Reactive;
using ReactiveUI;
using RhythmScrobbler.Models;

namespace RhythmScrobbler.ViewModels;

public class GameViewModel : ViewModelBase
{
    public GameViewModel()
    {
    }

    public GameViewModel(Game game)
    {
        _name = game.Name;
        _path = game.Path;

        SelectPath = ReactiveCommand.Create(SelectAnPath);
    }


    
    
    public ReactiveCommand<Unit, Unit> SelectPath { get; }

    private void SelectAnPath()
    {
        Debug.WriteLine("Path Clicked");
    }
    
    private string _name;

    public string Name
    {
        get => _name;
        set => this.RaiseAndSetIfChanged(ref _name, value);
    }

    private string _path;

    public string Path
    {
        get => _path;
        set => this.RaiseAndSetIfChanged(ref _path, value);
    }
}