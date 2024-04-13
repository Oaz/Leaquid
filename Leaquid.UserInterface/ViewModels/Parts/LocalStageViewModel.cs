using System;
using System.Reactive.Linq;
using Avalonia.Media;
using DynamicData.Kernel;
using Leaquid.Core;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class LocalStageViewModel : StageViewModel
{
  public LocalStageViewModel(Stage stage, Watch<IPlayer> currentPlayer) : base(stage)
  {
    currentPlayer.Latest.IfHasValue(SetCurrentPlayerColor);
    currentPlayer
      .Updates
      .Scan(
        new PlayerChange(Optional<IPlayer>.None, Optional<IPlayer>.None),
        (pc, pl) => new PlayerChange(pc.After, Optional<IPlayer>.ToOptional(pl)))
      .Subscribe(pc =>
      {
        pc.Before.IfHasValue(SetOtherPlayerColor);
        pc.After.IfHasValue(SetCurrentPlayerColor);
      });

  }

  private void SetCurrentPlayerColor(IPlayer pl) => Players[pl.Index].Brush = _currentPlayerColor;
  private void SetOtherPlayerColor(IPlayer pl) => Players[pl.Index].Brush = OtherPlayerColor;

  record PlayerChange(Optional<IPlayer> Before, Optional<IPlayer> After);
  
  private readonly IImmutableBrush _currentPlayerColor = Brushes.LimeGreen;

}