using System.Windows.Input;
using Avalonia.Controls;
using ReactiveUI;

namespace RhythmScrobbler.ViewModels;

public class DialogWindowViewModel : ViewModelBase
{
    public DialogWindowViewModel()
    {
        CloseCommand = ReactiveCommand.Create<Window>(window => Close(window));
    }

    public string Message { get; set; }

    public ICommand CloseCommand { get; }

    private void Close(Window window) 
    {
        if (window != null)
        {
            window.Close();
        }
    }
}