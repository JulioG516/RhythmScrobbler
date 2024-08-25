using Avalonia.Controls;
using Avalonia.ReactiveUI;
using System;
using ReactiveUI;

using RhythmScrobbler.ViewModels;


namespace RhythmScrobbler.Views;

//Window
public partial class LoginWindow : ReactiveWindow<LoginWindowViewModel>
{
    public LoginWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;

        this.WhenActivated(action => action(ViewModel!.LoginCommand.Subscribe(x => Close(x))));
        this.WhenActivated(action => action(ViewModel!.CloseCommand.Subscribe(x => Close(false))));
    }

 
}