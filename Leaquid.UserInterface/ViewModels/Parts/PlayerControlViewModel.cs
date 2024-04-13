using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class PlayerControlViewModel(Watch<IPlayer> currentPlayer) : ViewModelBase
{
  public void Up() => currentPlayer.Latest.Value.Up();
  public void Down() => currentPlayer.Latest.Value.Down();
  public void Left() => currentPlayer.Latest.Value.Left();
  public void Right() => currentPlayer.Latest.Value.Right();
}

