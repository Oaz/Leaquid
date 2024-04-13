using System.Drawing;
using System.Reactive.Subjects;
using Leaquid.Core.Messages;

namespace Leaquid.Core.Tests;

[TestFixture]
public class PlayRemoteTests
{

  [Test]
  public void DefaultStatusIsCreated()
  {
    Assert.That(_playRemote.CurrentStatus.Latest.Value, Is.EqualTo(PlayRemote.Status.Created));
  }
  
  [Test]
  public void JoinGameAccepted()
  {
    RegistrationAccepted();
    Assert.That(_playRemote.CurrentStatus.Latest.Value, Is.EqualTo(PlayRemote.Status.Registered));
  }
 
  [Test]
  public void GameStarts()
  {
    RegistrationAccepted();
    StartPlaying();
    Assert.That(_playRemote.CurrentStatus.Latest.Value, Is.EqualTo(PlayRemote.Status.Playing));
    Assert.That(_playRemote.Player.Latest.Value.Index, Is.EqualTo(4));
    Assert.That(_playRemote.Player.Latest.Value.Slope, Is.EqualTo(-1));
    Assert.That(_playRemote.Player.Latest.Value.Bounds.Latest.HasValue, Is.EqualTo(false));
  }
  
  [Test]
  public void GameAdvances()
  {
    RegistrationAccepted();
    StartPlaying();
    _incomingMessages.OnNext(new StageUpdateMessage
    {
      Players = Enumerable.Range(0,10)
        .Select(i => new StageUpdateMessage.PlayerPosition( "player" + i, new Point(i*2,i*3))),
      TankLevels = new StageUpdateMessage.BoundsUpdate[]
      {
        new (new Point(40, 50), new Size(10,10)),
      },
      AddDrops = Array.Empty<Point>(),
      RemoveDrops = Array.Empty<Point>()
    });
    Assert.That(_playRemote.Stage.Tanks.First().Bounds.Latest.Value.Origin, Is.EqualTo(new Point(40, 50)));
    Assert.That(_playRemote.Stage.Tanks.First().Bounds.Latest.Value.Size, Is.EqualTo(new Size(10,10)));
    Assert.That(_playRemote.Player.Latest.Value.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(8,12)));
    Assert.That(_playRemote.Player.Latest.Value.Bounds.Latest.Value.Size, Is.EqualTo(new Size(26,3)));
  }
  
  [Test]
  public void Up()
  {
    RegistrationAccepted();
    StartPlaying();
    _playRemote.Player.Latest.Value.Up();
    Assert.That(_outgoingToHostMessages.Last(), Is.EqualTo(new PlayerMoveMessage
    {
      PlayerId = _myPlayerId,
      Move = PlayerMoveMessage.Direction.Up
    }));
  }

  [Test]
  public void Down()
  {
    RegistrationAccepted();
    StartPlaying();
    _playRemote.Player.Latest.Value.Down();
    Assert.That(_outgoingToHostMessages.Last(), Is.EqualTo(new PlayerMoveMessage
    {
      PlayerId = _myPlayerId,
      Move = PlayerMoveMessage.Direction.Down
    }));
  }

  [Test]
  public void Left()
  {
    RegistrationAccepted();
    StartPlaying();
    _playRemote.Player.Latest.Value.Left();
    Assert.That(_outgoingToHostMessages.Last(), Is.EqualTo(new PlayerMoveMessage
    {
      PlayerId = _myPlayerId,
      Move = PlayerMoveMessage.Direction.Left
    }));
  }

  [Test]
  public void Right()
  {
    RegistrationAccepted();
    StartPlaying();
    _playRemote.Player.Latest.Value.Right();
    Assert.That(_outgoingToHostMessages.Last(), Is.EqualTo(new PlayerMoveMessage
    {
      PlayerId = _myPlayerId,
      Move = PlayerMoveMessage.Direction.Right
    }));
  }
  
  private void RegistrationAccepted()
  {
    _incomingMessages.OnNext(new RegistrationAcceptedMessage());
  }

  private void StartPlaying()
  {
    _incomingMessages.OnNext(new StartPlayingMessage
    {
      StageSize = new Size(300,180),
      PlayingAreaOrigin = new Point(18,10),
      PlayingAreaSize = new Size(264,98),
      Options = new Setup.Options(1,1,1,1),
      Players = Enumerable.Range(0,10)
        .Select(i => new StartPlayingMessage.PlayerDefinition(i, "player" + i))
    });
  }


  [SetUp]
  public void Init()
  {
    _incomingMessages = new Subject<IMessage>();
    _outgoingToHostMessages = new List<IMessage>();
    var sendToHost = new Func<IMessage, Task>(message =>
    {
      _outgoingToHostMessages.Add(message);
      return Task.CompletedTask;
    });

    _playRemote = new PlayRemote(_myPlayerId, _incomingMessages, sendToHost);
  }
  
  [TearDown]
  public void CleanUp()
  {
    _incomingMessages.Dispose();
  }

  private readonly string _myPlayerId = "player4";
  private PlayRemote _playRemote = null!;
  private List<IMessage> _outgoingToHostMessages = null!;
  private Subject<IMessage> _incomingMessages = null!;

}