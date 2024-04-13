using System.Drawing;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Leaquid.Core.Messages;

namespace Leaquid.Core.Tests;

[TestFixture]
public class HostGameTests
{
  private HostGame _hostGame = null!;
  private List<IMessage> _broadcastMessages = null!;
  private List<(string, IMessage)> _outgoingPlayerMessages = null!;
  private List<IMessage> _incomingPlayerMessages = null!;
  private Subject<IMessage> _incomingMessages = null!;

  [Test]
  public void HostInit()
  {
    Assert.That(_hostGame.Speed, Is.EqualTo(TimeSpan.FromMilliseconds(400)));
    Assert.That(_hostGame.PlayersCount.Latest.HasValue, Is.EqualTo(false));

  }
  
  [Test]
  public void ConfirmPlayersRegistrations()
  {
    PLayersRegisterToGame();
    Assert.That(_outgoingPlayerMessages.Count, Is.EqualTo(10));
    for (int i = 0; i < 10; i++)
    {
      var accepted = new RegistrationAcceptedMessage();
      Assert.That(_outgoingPlayerMessages[i], Is.EqualTo(("player" + i,accepted)));
    }
  }
  
  [Test]
  public async Task BroadcastStartPlayingMessageOnGameStart()
  {
    PLayersRegisterToGame();
    await _hostGame.Start();
    Assert.That(_broadcastMessages.Count, Is.EqualTo(1));
    var bm = (StartPlayingMessage) _broadcastMessages.First();
    Assert.That(bm.StageSize, Is.EqualTo(new Size(300,180)));
    Assert.That(bm.PlayingAreaOrigin, Is.EqualTo(new Point(18,10)));
    Assert.That(bm.PlayingAreaSize, Is.EqualTo(new Size(264,98)));
    Assert.That(bm.Options, Is.EqualTo(new Setup.Options(1,1,1,1)));
    Assert.That(bm.Players, Is.EqualTo(Enumerable.Range(0,10)
      .Select(i => new StartPlayingMessage.PlayerDefinition(i, "player" + i))));
  }
  
  [Test]
  public async Task BroadcastStageUpdateMessageOnCadence()
  {
    PLayersRegisterToGame();
    await _hostGame.Start();
    for (int i = 0; i < 100; i++)
    {
      _hostGame.Next();
      Assert.That(_broadcastMessages.Count, Is.EqualTo(i+2));
      var bm = (StageUpdateMessage) _broadcastMessages.Last();
      Assert.That(bm.AddDrops.Count(), Is.GreaterThan(0));
      Assert.That(bm.RemoveDrops.Count(), Is.EqualTo(0));
      Assert.That(bm.TankLevels.Count(), Is.EqualTo(1));
      Assert.That(bm.Players.Count(), Is.EqualTo(10));
    }
  }
  
  [Test]
  public async Task IfPlayerMovesDropsMightBeRemoved()
  {
    PLayersRegisterToGame();
    await _hostGame.Start();
    for (int i = 0; i < 250; i++) _hostGame.Next();
    _incomingMessages.OnNext(new PlayerMoveMessage
    {
      PlayerId = "player5",
      Move = PlayerMoveMessage.Direction.Left
    });
    _hostGame.Next();
    _hostGame.Next();
    var bm = (StageUpdateMessage) _broadcastMessages.Last();
    Assert.That(bm.RemoveDrops.Count(), Is.GreaterThan(0));
    Assert.That(bm.TankLevels.Count(), Is.EqualTo(1));
    Assert.That(bm.Players.Count(), Is.EqualTo(10));
  }
  
  [Test]
  public async Task AtSomePointWhenNobodyMovesNoMoreAddedOrRemovedDrops()
  {
    PLayersRegisterToGame();
    await _hostGame.Start();
    for (int i = 0; i < 250; i++)
    {
      _hostGame.Next();
    }
    for (int i = 0; i < 1000; i++)
    {
      _hostGame.Next();
      var bm = (StageUpdateMessage) _broadcastMessages.Last();
      Assert.That(bm.AddDrops.Count(), Is.EqualTo(0));
      Assert.That(bm.RemoveDrops.Count(), Is.EqualTo(0));
    }
  }
  
  [Test]
  public async Task PlayersCanMove()
  {
    PLayersRegisterToGame();
    await _hostGame.Start();
    _hostGame.Next();
    var beforeMove = (StageUpdateMessage) _broadcastMessages.Last();
    for (int i = 0; i < 10; i++)
      _incomingMessages.OnNext(new PlayerMoveMessage
      {
        PlayerId = "player" + i,
        Move = (PlayerMoveMessage.Direction) (i % 4)
      });
    _hostGame.Next();
    var afterMove = (StageUpdateMessage) _broadcastMessages.Last();
    Assert.That(
      Shifts(afterMove, beforeMove, p => p.X),
      Is.EqualTo(new [] { 0, 0, -1, 1, 0, 0, -1, 1, 0, 0 })
    );
    Assert.That(
      Shifts(afterMove, beforeMove, p => p.Y),
      Is.EqualTo(new [] { -1, 1, 0, 0, -1, 1, 0, 0, -1, 1 })
    );
  }

  private static IEnumerable<int> Shifts(
    StageUpdateMessage afterMove,
    StageUpdateMessage beforeMove,
    Func<Point, int> coordinate) => afterMove
      .Players.Select(p => coordinate(p.Origin))
      .Zip(beforeMove.Players.Select(p => coordinate(p.Origin)),
        (after, before) => after - before);


  private void PLayersRegisterToGame()
  {
    for (int i = 0; i < 10; i++)
      _incomingMessages.OnNext(new RegistrationQueryMessage { PlayerId = "player" + i });
  }

  [SetUp]
  public void Init()
  {
    var options = new Setup.Options(1,1,1,1);
    var isRunning = Observable.Return(true);
    _incomingMessages = new Subject<IMessage>();
    _incomingPlayerMessages = new List<IMessage>();
    _incomingMessages.Subscribe(m => _incomingPlayerMessages.Add(m));
    _outgoingPlayerMessages = new List<(string,IMessage)>();
    var sendToPlayer = new Func<string, IMessage, Task>((playerId, message) =>
    {
      _outgoingPlayerMessages.Add((playerId,message));
      return Task.CompletedTask;
    });
    _broadcastMessages = new List<IMessage>();
    var broadcastToAllPlayers = new Func<IMessage, Task>(message =>
    {
      _broadcastMessages.Add(message);
      return Task.CompletedTask;
    });

    _hostGame = new HostGame(options, isRunning, _incomingMessages, sendToPlayer, broadcastToAllPlayers);
  }

  [TearDown]
  public void CleanUp()
  {
    _incomingMessages.Dispose();
    _hostGame.Dispose();
  }
}