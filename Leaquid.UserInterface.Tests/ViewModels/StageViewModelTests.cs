using System.Drawing;
using Avalonia.Media;
using DynamicData;
using Leaquid.Core;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;
using Leaquid.UserInterface.ViewModels.Actors;
using Leaquid.UserInterface.ViewModels.Parts;
using Brushes = Avalonia.Media.Brushes;

namespace Leaquid.UserInterface.Tests.ViewModels;

public class StageViewModelTests
{
  [Test]
  public void PlayersAppearOnStageWhenTheyAreAlreadyPresent()
  {
    var stage = new ActiveStage(new Size(400, 240),2,2);
    AddPlayerToStage(0, stage);
    AddPlayerToStage(1, stage);
    var sut = new StageViewModel(stage);

    Assert.That(sut.Actors.OfType<ColoredRectangleViewModel>(), Is.EquivalentTo(new[]
    {
      CVR(25, 144, Wall.Width, 96, Brushes.DarkSlateGray),
      CVR(365, 144, Wall.Width, 96, Brushes.DarkSlateGray),
      CVR(25, 10, 20, Player.Height, Brushes.SaddleBrown),
      CVR(30, 15, 25, Player.Height, Brushes.SaddleBrown),
      CVR(0, 156, 25, 84, Brushes.DodgerBlue),
      CVR(375, 156, 25, 84, Brushes.DodgerBlue),
      CVR(35, 239, 330, 1, Brushes.DodgerBlue),
    }));
  }

  [Test]
  public void PlayersAppearOnStageEachTimeTheyAreAdded()
  {
    var stage = new ActiveStage(new Size(400, 240),2,2);
    var sut = new StageViewModel(stage);

    AddPlayerToStage(0, stage);
    AddPlayerToStage(1, stage);

    Assert.That(sut.Actors.OfType<ColoredRectangleViewModel>(), Is.EquivalentTo(new[]
    {
      CVR(25, 144, Wall.Width, 96, Brushes.DarkSlateGray),
      CVR(365, 144, Wall.Width, 96, Brushes.DarkSlateGray),
      CVR(25, 10, 20, Player.Height, Brushes.SaddleBrown),
      CVR(30, 15, 25, Player.Height, Brushes.SaddleBrown),
      CVR(0, 156, 25, 84, Brushes.DodgerBlue),
      CVR(375, 156, 25, 84, Brushes.DodgerBlue),
      CVR(35, 239, 330, 1, Brushes.DodgerBlue),
    }));
  }

  private static void AddPlayerToStage(int i, Stage stage)
  {
    var player = new Player(i);
    stage.Players.Add(player);
    player.Spawn(i * 30, i * 15, 20 + i * 5, stage.PlayingArea);
  }

  private static ColoredRectangleViewModel CVR(int x, int y, int width, int height, IBrush brush)
  {
    var cvr = new ColoredRectangleViewModel(new Watch<Bounds>()
    {
      Latest = new Bounds(new Point(x, y), new Size(width, height))
    });
    cvr.Brush = brush;
    return cvr;
  }
}