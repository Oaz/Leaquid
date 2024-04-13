using System.Drawing;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests;

public class StageTests
{
  [Test]
  public void Stage_800_480()
  {
    var sut = new Stage(new Size(800, 480));
    Assert.That(sut.PlayingArea, Is.EqualTo(
      new Bounds(new Point(50, 10), new Size(700, 278))
    ));
    Assert.That(sut.Walls, Is.EquivalentTo(new[]
    {
      new Wall(50, 288, 192, -1),
      new Wall(740, 288, 192, 1),
    }));
    Assert.That(sut.Tanks, Is.EquivalentTo(new[]
    {
      new Tank(60, 680, 479),
    }));
    Assert.That(sut.Sinks, Is.EquivalentTo(new[]
    {
      new Sink(0, 312, 50, 168),
      new Sink(750, 312, 50, 168),
    }));
  }

  [Test]
  public void Stage_1200_720()
  {
    var sut = new Stage(new Size(1200, 720));
    Assert.That(sut.PlayingArea, Is.EqualTo(
      new Bounds(new Point(75, 10), new Size(1050, 422))
    ));
    Assert.That(sut.Walls, Is.EquivalentTo(new[]
    {
      new Wall(75, 432, 288, -1),
      new Wall(1115, 432, 288, 1),
    }));
    Assert.That(sut.Tanks, Is.EquivalentTo(new[]
    {
      new Tank(85, 1030, 719),
    }));
    Assert.That(sut.Sinks, Is.EquivalentTo(new[]
    {
      new Sink(0, 468, 75, 252),
      new Sink(1125, 468, 75, 252),
    }));
  }
}