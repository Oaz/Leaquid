using Avalonia;
using Avalonia.Threading;
using Leaquid.Core;
using Leaquid.Core.Messages;
using Leaquid.UserInterface.ViewModels;
using Leaquid.UserInterface.ViewModels.Parts;

namespace Leaquid.UserInterface.Tests.ViewModels;

public class RemotePlayViewModelTests
{
  [SetUp]
  public void Init()
  {
    HeadlessApp.NewContext();
  }

  [Test]
  public void AtStartup_TheDefaultGameCodeIsProposed()
  {
    HeadlessApp.Context.DefaultGameCode = "ABCDEFGHI";
    var sut = new RemotePlayViewModel();
    var registration = (RegisterToGameViewModel) sut.Central;
    Assert.That(registration.GameCode, Is.EqualTo("ABCDEF"));
  }

  [Test]
  public void GameCodeIsUpperCasedAndTruncatedTo6UpperCaseChars()
  {
    var sut = new RemotePlayViewModel();
    var registration = (RegisterToGameViewModel) sut.Central;
    registration.GameCode = "zyx0wv1u2tsr";
    Assert.That(registration.GameCode, Is.EqualTo("ZYXWVU"));
  }

  [Test]
  public void AfterTheGameCodeIsEnteredAndTheRegistrationIsAccepted_ThePlayerIsRegistered()
  {
    var sut = new RemotePlayViewModel();
    var registration = (RegisterToGameViewModel) sut.Central;
    registration.GameCode = "ABCDEF";
    registration.Join();
    HeadlessApp.SendToSeat(new RegistrationAcceptedMessage());
    Dispatcher.UIThread.RunJobs();
    Assert.That(registration.Registered, Is.EqualTo(true));
  }

  [Test]
  public void WhenTheGameStarts_TheStageIsDisplayed()
  {
    var sut = StartGame();
    var stage = (FramingViewModel)sut.Central;
    Assert.That(stage.Framed.Size, Is.EqualTo(new Size(300, 180)));
    stage.Bounds = new Rect(20, 10, 600, 360);
    Assert.That(stage.Size, Is.EqualTo(new Size(600, 360)));
    Assert.That(stage.Transformation.Value, Is.EqualTo(
      new Matrix(2, 0, 0, 2, 150, 90)
    ));
  }

  [Test]
  public void AfterTheGameStarts_TheStageViewCanBeChanged()
  {
    var sut = StartGame();
    var stageControl = (StageControlViewModel)sut.BottomLeft;
    stageControl.ZoomLevels[0].IsActive = false;
    stageControl.ZoomLevels[1].IsActive = true;
    var stage = (FramingViewModel)sut.Central;
    Assert.That(stage.Framed.Size, Is.EqualTo(new Size(300, 180)));
    stage.Bounds = new Rect(20, 10, 600, 360);
    Assert.That(stage.Size, Is.EqualTo(new Size(600, 360)));
    Assert.That(stage.Transformation.Value, Is.EqualTo(
      new Matrix(4, 0, 0, 4, 150, 90)
    ));
  }

  [Test]
  public void AfterTheGameStarts_ThePlayerCanMove()
  {
    var sut = StartGame();
    var playerControl = (PlayerControlViewModel)sut.BottomRight;
    playerControl.Left();
    Assert.That(HeadlessApp.MessagesSentToHost.Last(), Is.EqualTo(
      new PlayerMoveMessage { PlayerId = "player666", Move = PlayerMoveMessage.Direction.Left }
    ));
    playerControl.Up();
    Assert.That(HeadlessApp.MessagesSentToHost.Last(), Is.EqualTo(
      new PlayerMoveMessage { PlayerId = "player666", Move = PlayerMoveMessage.Direction.Up }
    ));
    playerControl.Right();
    Assert.That(HeadlessApp.MessagesSentToHost.Last(), Is.EqualTo(
      new PlayerMoveMessage { PlayerId = "player666", Move = PlayerMoveMessage.Direction.Right }
    ));
    playerControl.Down();
    Assert.That(HeadlessApp.MessagesSentToHost.Last(), Is.EqualTo(
      new PlayerMoveMessage { PlayerId = "player666", Move = PlayerMoveMessage.Direction.Down }
    ));
  }

  private static RemotePlayViewModel StartGame()
  {
    var sut = new RemotePlayViewModel();
    var registration = (RegisterToGameViewModel) sut.Central;
    registration.Join();
    HeadlessApp.SendToSeat(new RegistrationAcceptedMessage());
    HeadlessApp.SendToSeat(new StartPlayingMessage
    {
      StageSize = new System.Drawing.Size(300,180),
      PlayingAreaOrigin = new System.Drawing.Point(20,10),
      PlayingAreaSize = new System.Drawing.Size(200,100),
      Options = new Setup.Options(1, 1, 1, 1),
      Players = new[]
      {
        new StartPlayingMessage.PlayerDefinition(0, "player0"),
        new StartPlayingMessage.PlayerDefinition(1, "player666"),
        new StartPlayingMessage.PlayerDefinition(2, "player2"),
      }
    });
    Dispatcher.UIThread.RunJobs();
    return sut;
  }
}