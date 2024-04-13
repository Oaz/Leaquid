using System.Drawing;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests;

[TestFixture]
public class SetupTests
{
  [TestCase(1f, 1, 264, 132)]
  [TestCase(2f, 1, 264, 132)]
  [TestCase(3f, 1, 264, 132)]
  [TestCase(1f, 2, 132, 99)]
  [TestCase(2f, 2, 134, 98)]
  [TestCase(3f, 2, 137, 97)]
  [TestCase(1f, 10, 26, 25)]
  [TestCase(2f, 10, 29, 24)]
  [TestCase(3f, 10, 31, 24)]
  public void ComputePlayerWidthAndSpacing(float playerWidthFactor, int playerCount, int expectedPlayerWidth, int expectedPlayerStep)
  {
    var playingArea = new Bounds(new Point(0, 0), new Size(264, 180));
    var (playerWidth, playerStep) = Setup.ComputePlayerWidthAndSpacing(playingArea, playerWidthFactor, playerCount);
    Assert.That(playerWidth, Is.EqualTo(expectedPlayerWidth));
    Assert.That(playerStep, Is.EqualTo(expectedPlayerStep));
    Assert.That(playerWidth*playerCount, Is.GreaterThanOrEqualTo(playingArea.Size.Width-Wall.Width));
  }

  [Test]
  public void RegisterOnePlayer()
  {
    using var setup = new Setup(new Setup.Options(100, 2, 3, 4));
    var player = setup.RegisterPlayer();
    setup.CompleteRegistration();
    Assert.That(setup.Stage.PlayingArea.Origin.X, Is.EqualTo(18));
    Assert.That(setup.Stage.PlayingArea.Width, Is.EqualTo(264));
    Assert.That(setup.Players.Count, Is.EqualTo(1));
    Assert.That(setup.Players, Contains.Item(player));
    Assert.That(player.Bounds.Latest.Value.XMin, Is.EqualTo(18));
  }
  
  [Test]
  public void RegisterMultiplePlayers()
  {
    using var setup = new Setup(new Setup.Options(100, 2, 3, 4));
    int playerCount = 10;
    for (int i = 0; i < playerCount; i++) setup.RegisterPlayer();
    setup.CompleteRegistration();
    
    Assert.That(setup.Players.Count, Is.EqualTo(playerCount));
    Assert.That(setup.Players.Select(p => p.Index), Is.EquivalentTo(Enumerable.Range(0, playerCount)));
    foreach (var player in setup.Players) Assert.That(player.Bounds.Latest.Value.XMin, Is.EqualTo(18+player.Index*24));
  }

}