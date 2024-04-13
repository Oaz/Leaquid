using System.Drawing;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Actors;

public class PlayerTests
{
  private Player _player = null!;
  private Bounds _playingArea;

  [SetUp]
  public void Setup()
  {
    _player = new Player(1);
    _playingArea = new Bounds(new Point(10,12), new Size(190,88));
  }

  [TestCase(1,1,5, 10,12,5)]
  [TestCase(11,13,5, 11,13,5)]
  [TestCase(310,215,5, 195, 97,5)]
  [TestCase(310,215,10, 190, 97,10)]
  [TestCase(191,98,10, 190, 97,10)]
  public void Spawn(int x, int y, int width, int expectedX, int expectedY, int expectedWidth)
  {
    _player.Spawn(x, y, width, _playingArea);

    Assert.That(_player.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(expectedX, expectedY)));
    Assert.That(_player.Bounds.Latest.Value.Size, Is.EqualTo(new Size(expectedWidth, Player.Height)));
  }

  [TestCase(10,12, 10,12)]
  [TestCase(10,13, 10,12)]
  [TestCase(10,25, 10,24)]
  [TestCase(20,12, 20,12)]
  [TestCase(20,13, 20,12)]
  [TestCase(20,25, 20,24)]
  public void Up(int x, int y, int expectedX, int expectedY)
  {
    _player.Spawn(x, y, 5, _playingArea);
    _player.Up();
    Assert.That(_player.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(expectedX, expectedY)));
  }

  [TestCase(10,13, 10,14)]
  [TestCase(10,96, 10,97)]
  [TestCase(10,97, 10,97)]
  [TestCase(20,13, 20,14)]
  [TestCase(20,96, 20,97)]
  [TestCase(20,97, 20,97)]
  public void Down(int x, int y, int expectedX, int expectedY)
  {
    _player.Spawn(x, y, 5, _playingArea);
    _player.Down();
    Assert.That(_player.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(expectedX, expectedY)));
  }

  [TestCase(10,12, 10,12)]
  [TestCase(11,12, 10,12)]
  [TestCase(20,12, 19,12)]
  [TestCase(10,20, 10,20)]
  [TestCase(11,20, 10,20)]
  [TestCase(20,20, 19,20)]
  public void Left(int x, int y, int expectedX, int expectedY)
  {
    _player.Spawn(x, y, 5, _playingArea);
    _player.Left();
    Assert.That(_player.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(expectedX, expectedY)));
  }

  [TestCase(15,12, 16,12)]
  [TestCase(194,12, 195,12)]
  [TestCase(195,12, 195,12)]
  [TestCase(15,20, 16,20)]
  [TestCase(194,20, 195,20)]
  [TestCase(195,20, 195,20)]
  public void Right(int x, int y, int expectedX, int expectedY)
  {
    _player.Spawn(x, y, 5, _playingArea);
    _player.Right();
    Assert.That(_player.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(expectedX, expectedY)));
  }
    
  [Test]
  public void PlayersEquality()
  {
    var player1 = new Player(5);
    var player2 = new Player(5);
    Assert.That(player1, Is.EqualTo(player2));
    Assert.True(player1.Equals(player2));
    Assert.True(player1.Equals(player1));

    player1.Spawn(12, 15, 5, _playingArea);
    player2.Spawn(12, 15, 5, _playingArea);
    Assert.That(player1, Is.EqualTo(player2));
    Assert.True(player1.Equals(player2));
    Assert.True(player1.Equals(player1));
    
    Assert.False(player1.Equals(null!));
    object obj = player1!;
    Assert.False(obj!.Equals(null!));
  }

}