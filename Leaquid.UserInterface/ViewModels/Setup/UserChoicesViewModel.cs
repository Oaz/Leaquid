using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Setup;

public class UserChoicesViewModel : ViewModelBase
{
  public UserChoicesViewModel(OptionsViewModel options)
  {
    HostGame = new UserChoiceViewModel("Host Game",
      () => { Selection = new HostedGameViewModel(options.Values); }
    );
    JoinGame = new UserChoiceViewModel("Join Game",
      () => { Selection = new RemotePlayViewModel(); }
    );
    PlayLocalGame = new UserChoiceViewModel("Play Local Game",
      () => { Selection = new LocalGameViewModel(options.Values); }
    );
  }

  public UserChoiceViewModel HostGame { get; }
  public UserChoiceViewModel JoinGame { get; }
  public UserChoiceViewModel PlayLocalGame { get; }

  public ViewModelBase Selection
  {
    get => _selection;
    set => this.RaiseAndSetIfChanged(ref _selection, value);
  }

  private ViewModelBase _selection = null!;
}

internal class DesignUserChoicesViewModel() : UserChoicesViewModel(new OptionsViewModel());