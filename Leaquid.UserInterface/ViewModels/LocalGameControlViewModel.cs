using Leaquid.Core;

namespace Leaquid.UserInterface.ViewModels;

public class LocalGameControlViewModel(LocalGame localGame) : ViewModelBase
{
  public void NextPlayer() => localGame.NextPlayer();
  public void PreviousPlayer() => localGame.PreviousPlayer();
}