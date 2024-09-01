using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;

namespace RhythmScrobbler.ViewModels;

public class MainWindowViewModel : ReactiveObject, IScreen
{
    // The Router associated with this Screen.
    // Required by the IScreen interface.
    public RoutingState Router { get; } = new RoutingState();

    private ObservableCollection<IRoutableViewModel> ViewModelCollection { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateHome { get; }

    public ReactiveCommand<Unit, IRoutableViewModel> NavigateLog { get; }

    public MainWindowViewModel()
    {
        ViewModelCollection = new ObservableCollection<IRoutableViewModel>
        {
            new HomeViewModel(this),
            //new SecondViewModel(this)
            // Add other view models here
        };

        // NavigateCommand = ReactiveCommand.CreateFromTask<IRoutableViewModel>(NavigateToViewModel);
        NavigateHome = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(ViewModelCollection[0])
        );

        NavigateLog = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(new LogViewModel(this))
        );

        Router.Navigate.Execute(ViewModelCollection[0]);
    }
    
}