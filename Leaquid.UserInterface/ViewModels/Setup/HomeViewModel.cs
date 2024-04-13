namespace Leaquid.UserInterface.ViewModels.Setup;

public class HomeViewModel : ViewModelBase
{
  public HomeViewModel()
  {
    Options = new OptionsViewModel();
    UserChoices = new UserChoicesViewModel(Options);
  }
  public UserChoicesViewModel UserChoices { get; }
  public OptionsViewModel Options { get; }
}