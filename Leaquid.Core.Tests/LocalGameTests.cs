namespace Leaquid.Core.Tests;

[TestFixture]
public class LocalGameTests
{
  [Test]
  public void DetailsAreCompleteAfterStart()
  {
    var localGame = new LocalGame(
      10,
      new Setup.Options(100, 2, 3, 4)
    );
    localGame.Start();
    Assert.That(localGame.CurrentPlayer.Latest.HasValue);
    Assert.That(localGame.Speed, Is.EqualTo(TimeSpan.FromMilliseconds(50)));
    Assert.That(
      localGame.Setup.Players.Select(p => p.Bounds.Latest.Value.XMin),
      Is.EquivalentTo(Enumerable.Range(0, 10).Select(i => i*24+18))
    );
  }

  [Test]
  public void SetNextPlayer()
  {
    var localGame = new LocalGame(
      10,
      new Setup.Options(100, 2, 3, 4)
    );
    localGame.Start();
    for (int i = 0; i < 100; i++)
    {
      var currentPlayer = localGame.CurrentPlayer.Latest.Value;
      localGame.NextPlayer();
      Assert.That(localGame.CurrentPlayer.Latest.Value.Index, Is.EqualTo((currentPlayer.Index + 1)%10));
    }
  }

  [Test]
  public void SetPreviousPlayer()
  {
    var localGame = new LocalGame(
      10,
      new Setup.Options(100, 2, 3, 4)
    );
    localGame.Start();
    for (int i = 0; i < 100; i++)
    {
      var currentPlayer = localGame.CurrentPlayer.Latest.Value;
      localGame.PreviousPlayer();
      Assert.That(localGame.CurrentPlayer.Latest.Value.Index, Is.EqualTo((currentPlayer.Index + 9)%10));
    }
  }
  
  [Test]
  public void NextUpdatesDrops()
  {
    var localGame = new LocalGame(
      10,
      new Setup.Options(100, 2, 3, 4)
    );
    localGame.Start();
    Assert.That(localGame.Setup.Stage.Flood.Drops.Count, Is.EqualTo(0));
    var dropsWereChecked = false;
    localGame.Setup.Stage.Flood.Drops.Connect()
      .Subscribe(dropChanges =>
      {
        Assert.That(dropChanges.Count, Is.EqualTo(37));
        dropsWereChecked = true;
      });
    localGame.Next();
    Assert.True(dropsWereChecked);
  }
}