﻿using System;
using System.Diagnostics;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using RhythmScrobbler.Models;
using RhythmScrobbler.Services;
using Splat;

namespace RhythmScrobbler.ViewModels;

//TODO: Make the SelectPath command open a dialog
//TODO: Make the switch command and make the logic to watch the song
public class GameViewModel : ViewModelBase 
{
    public GameViewModel()
    {
    }

    public GameViewModel(Game game)
    {
        Name = game.Name;
        Path = game.Path;
        Type = game.Type;

        if (_type == GameType.CloneHero)
        {
            Debug.WriteLine("instanciei o clone hero.");
        }

        SelectPath = ReactiveCommand.Create(SelectAnPath);
        ToggleWatcher = ReactiveCommand.Create(ToggleEnabled);

        // _fileDialog = Locator.Current.GetService<FileDialogService>();

        this.WhenAnyValue(x => x.Path);
        this.WhenAnyValue(x => x.IsWatcherToggled);


    }


    // private FileDialogService _fileDialog;
    public ReactiveCommand<Unit, Unit> SelectPath { get; }

    public ReactiveCommand<Unit, Unit> ToggleWatcher { get; }


    private void SelectAnPath()
    {
        Debug.WriteLine("Path Clicked " + Name);
        Path = Guid.NewGuid().ToString().Substring(0, 10);
    }

    private void ToggleEnabled()
    {
        Debug.WriteLine("Watcher toggle click");
        _isWatcherToggled = !_isWatcherToggled;
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

    private GameType _type;

    public GameType Type
    {
        get => _type;
        set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    private bool _isWatcherToggled;

    public bool IsWatcherToggled
    {
        get => _isWatcherToggled;
        set => this.RaiseAndSetIfChanged(ref _isWatcherToggled, value);
    }
}