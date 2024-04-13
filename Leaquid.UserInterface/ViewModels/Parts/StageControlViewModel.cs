using System;
using System.Linq;
using Avalonia;
using DynamicData.Binding;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class StageControlViewModel : ViewModelBase
{
  public StageControlViewModel(FramingViewModel framing, Watch<IPlayer> currentPlayer)
  {
    Framing = framing;
    var size = framing.Framed.Size;
    ZoomLevels = new[]
    {
      new ZoomLevel("Full Scene", () => Framing.AdjustTo(size), true),
      new ZoomLevel("Area", () => ZoomToPlayer(size.Width/2), false),
      new ZoomLevel("Player", () => ZoomToPlayer(size.Width/5), false),
    };
    Framing
      .WhenPropertyChanged(x => x.Size)
      .Subscribe(bounds =>
      {
        RefreshZoom();
      });
    currentPlayer.InitializeAndTrackWith(player =>
    {
      player.Bounds.InitializeAndTrackWith(bounds =>
      {
        var center = bounds.Center;
        _translateToPlayer = new Rect(size).Center - new Point(center.X,center.Y);
        RefreshZoom();
      });
    });
  }

  private void RefreshZoom() => ZoomLevels.First(x => x.IsActive).Refresh();
  private void ZoomToPlayer(double width) => Framing.AdjustTo(new Size(width,1), _translateToPlayer);
  private Point _translateToPlayer;

  public FramingViewModel Framing
  {
    get => _framing;
    set => this.RaiseAndSetIfChanged(ref _framing, value);
  }
  private FramingViewModel _framing = null!;

  public ZoomLevel[] ZoomLevels
  {
    get => _zoomLevels;
    set => this.RaiseAndSetIfChanged(ref _zoomLevels, value);
  }

  private ZoomLevel[] _zoomLevels = null!;

}

internal class DesignStageControlViewModel() : StageControlViewModel(new DesignFramingViewModel(), new Watch<IPlayer>());
