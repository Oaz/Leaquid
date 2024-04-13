using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Media;
using DynamicData;
using Leaquid.Core;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;
using Leaquid.UserInterface.ViewModels.Actors;
using IActor = Leaquid.UserInterface.ViewModels.Actors.IActor;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class StageViewModel : ViewModelBase, IFramed
{
  public StageViewModel(Stage stage)
  {
    Size = new Size(stage.Size.Width, stage.Size.Height);
    Players = new List<ColoredRectangleViewModel>();
    var sourceActors = new SourceList<IActor>().DisposeWith(Me);
    Flood = new FloodViewModel(stage.Flood);
    sourceActors.Add(Flood);
    foreach (var wall in stage.Walls)
      NewColoredRectangle(wall.Bounds, Brushes.DarkSlateGray);
    foreach (var sink in stage.Sinks)
      NewColoredRectangle(sink.Bounds, _waterColor);
    foreach (var tank in stage.Tanks)
      NewColoredRectangle(tank.Bounds, _waterColor);
    try
    {
      AddDecor(stage, sourceActors);
    }
    catch (Exception)
    {
      // ignored
    }

    sourceActors.Connect()
      .Bind(out _actors)
      .Subscribe().DisposeWith(Me);
    stage.Players.Connect()
      .OnItemAdded(player =>
        Players.Add(NewColoredRectangle(player.Bounds, OtherPlayerColor))
      ).Subscribe().DisposeWith(Me);

    ColoredRectangleViewModel NewColoredRectangle(Watch<Bounds> bounds, IBrush brush)
    {
      var cr = new ColoredRectangleViewModel(bounds);
      cr.Brush = brush;
      sourceActors.Add(cr);
      return cr;
    }
  }

  private static void AddDecor(Stage stage, SourceList<IActor> sourceActors)
  {
    sourceActors.Add(new DecorViewModel("castle_pxl")
    {
      Align =
      {
        HCenter = stage.Size.Width / 2,
        Bottom = stage.Size.Height
      }
    });
  }

  public Size Size { get; }

  protected readonly List<ColoredRectangleViewModel> Players;
  protected readonly IImmutableBrush OtherPlayerColor = Brushes.SaddleBrown;
  private readonly IImmutableBrush _waterColor = Brushes.DodgerBlue;

  public ReadOnlyObservableCollection<IActor> Actors => _actors;
  private readonly ReadOnlyObservableCollection<IActor> _actors;

  public FloodViewModel Flood { get; }
}

internal class DesignStageViewModel() : StageViewModel(new DesignStage());

internal class DesignStage : Stage
{
  public DesignStage() : base(DesignFlood.StageSize)
  {
    Flood = DesignFlood.Instance;
    for (int i = 1; i < 10; i++)
    {
      var player = new Player(i);
      Players.Add(player);
      player.Spawn(i * 30, i * 15, 20 + i * 5, PlayingArea);
    }
  }
}