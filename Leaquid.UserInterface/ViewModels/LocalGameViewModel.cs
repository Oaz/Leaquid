using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using Leaquid.Core;
using Leaquid.UserInterface.ViewModels.Parts;

namespace Leaquid.UserInterface.ViewModels;

public class LocalGameViewModel : GameViewModel
{
  public LocalGameViewModel(Core.Setup.Options options, int numberOfPlayers = 15)
  {
    var game = new LocalGame(numberOfPlayers, options);
    
    var stage = new LocalStageViewModel(game.Setup.Stage, game.CurrentPlayer);
    stage
      .WhenPropertyChanged(x => x.Leave)
      .Subscribe(p => Exit = p.Value)
      .DisposeWith(Me);
    
    var framing = new FramingViewModel(stage);
    Central = framing;
    BottomLeft = new StageControlViewModel(framing, game.CurrentPlayer);
    TopRight = new LocalGameControlViewModel(game);
    BottomRight = new PlayerControlViewModel(game.CurrentPlayer);
    game.Start();
    
    IAppContext._
      .Cadence(game.Speed)
      .Do(ts => game.Next())
      .Subscribe()
      .DisposeWith(Me);
  }
  
}

internal class DesignGameViewModel()
  : LocalGameViewModel(new Core.Setup.Options(1, 3, 4, 1));