using Leaquid.Core;
using Leaquid.UserInterface.ViewModels;

namespace Leaquid.Multigame.ViewModels;

public class EmbedGameViewModel : ViewModelBase
{
  public EmbedGameViewModel(Setup.Options options, int numberOfPlayers)
  {
    NumberOfPlayers = numberOfPlayers;
    Width = 400+10*numberOfPlayers;
    Height = (int)(Width*0.4);
    Game = new LocalGameViewModel(options, numberOfPlayers);
  }
  
  public int NumberOfPlayers { get; }
  public int Width { get; }
  public int Height { get; }
  public GameViewModel Game { get; }
}

public class DesignEmbedGameViewModel : EmbedGameViewModel
{
  public DesignEmbedGameViewModel() : base(new Setup.Options(1, 3, 1, 1), 15)
  {
    
  }
}