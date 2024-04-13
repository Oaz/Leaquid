using Avalonia;
using Avalonia.Threading;
using DynamicData.Binding;
using Leaquid.Core;
using Leaquid.Core.Messages;
using Leaquid.UserInterface.ViewModels;
using Leaquid.UserInterface.ViewModels.Parts;
using Point = System.Drawing.Point;

namespace Leaquid.UserInterface.Tests.ViewModels;

public class HostedGameViewModelTests
{
  [SetUp]
  public void Init()
  {
    HeadlessApp.NewContext();
  }

  [Test]
  public void AfterTheHostIsCreated_WaitingForPlayersRegistration()
  {
    ViewModelBase? central = null!;
    var sut = new HostedGameViewModel(new Setup.Options(1, 1, 1, 1));
    sut
      .WhenPropertyChanged(x => x.Central)
      .Subscribe(p => central = p.Value);
    Dispatcher.UIThread.RunJobs();
    Assert.True(central is WaitForRegistrationViewModel);
    var wfr = (WaitForRegistrationViewModel)central;
    Assert.That(wfr.GameId, Is.EqualTo("ABCD"));
    Assert.That(wfr.GameUrl, Is.EqualTo("https://my.site/ABCD"));
    Assert.That(wfr.QrCode.Size, Is.EqualTo(new Size(512, 512)));
    Assert.That(wfr.PlayerCount, Is.EqualTo(0));
  }

  [Test]
  public void ForEachPlayerRegistration_PlayerCountIsIncremented()
  {
    var sut = new HostedGameViewModel(new Setup.Options(1, 1, 1, 1));
    Dispatcher.UIThread.RunJobs();
    var wfr = (WaitForRegistrationViewModel)sut.Central;
    Assert.That(wfr.PlayerCount, Is.EqualTo(0));
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player1" });
    Assert.That(wfr.PlayerCount, Is.EqualTo(1));
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player2" });
    Assert.That(wfr.PlayerCount, Is.EqualTo(2));
  }

  [Test]
  public void ForEachPlayerRegistration_ConfirmationIsSentBackToPlayer()
  {
    var sut = new HostedGameViewModel(new Setup.Options(1, 1, 1, 1));
    Dispatcher.UIThread.RunJobs();
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player1" });
    Assert.That(HeadlessApp.MessagesSentToPlayers, Is.EqualTo(new[]
    {
      ("player1", new RegistrationAcceptedMessage())
    }));
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player2" });
    Assert.That(HeadlessApp.MessagesSentToPlayers, Is.EqualTo(new[]
    {
      ("player1", new RegistrationAcceptedMessage()),
      ("player2", new RegistrationAcceptedMessage())
    }));
  }

  [Test]
  public void WhenGameStarts_StageIsDisplayed()
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
  public void WhenGameStarts_AllRegisteredPlayersAreNotified()
  {
    var sut = StartGame();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(1));
    var startPlaying = (StartPlayingMessage)HeadlessApp.BroadcastMessages[0];
    Assert.That(startPlaying.Players, Is.EqualTo(new[]
    {
      new StartPlayingMessage.PlayerDefinition(0, "player1"),
      new StartPlayingMessage.PlayerDefinition(1, "player2"),
      new StartPlayingMessage.PlayerDefinition(2, "player3"),
    }));
  }

  [Test]
  public void WhenGameIsRunning_AllRegisteredPlayersAreUpdatedOnCadence()
  {
    var sut = StartGame();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(2));
    var update0 = (StageUpdateMessage)HeadlessApp.BroadcastMessages.Last();
    Assert.That(update0.AddDrops, Is.EqualTo(Enumerable.Range(146, 9).Select(x => new Point(x, 1))));
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(3));
    var update1 = (StageUpdateMessage)HeadlessApp.BroadcastMessages.Last();
    Assert.That(update1.AddDrops, Is.EqualTo(Enumerable.Range(146, 9).Select(x => new Point(x, 2))));
  }

  [Test]
  public void WhenGameIsPaused_AllRegisteredPlayersAreNoLongerUpdatedOnCadence()
  {
    var sut = StartGame();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(2));
    ((HostedGameControlViewModel)sut.BottomRight).Pause();
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(2));
  }

  [Test]
  public void WhenGameIsUnPaused_AllRegisteredPlayersAreUpdatedOnCadenceAgain()
  {
    var sut = StartGame();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(2));
    ((HostedGameControlViewModel)sut.BottomRight).Pause();
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(2));
    ((HostedGameControlViewModel)sut.BottomRight).Play();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(3));
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    HeadlessApp.Context.SendCadence();
    Assert.That(HeadlessApp.BroadcastMessages.Length, Is.EqualTo(6));
  }

  private static HostedGameViewModel StartGame()
  {
    var sut = new HostedGameViewModel(new Setup.Options(1, 1, 1, 1));
    Dispatcher.UIThread.RunJobs();
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player1" });
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player2" });
    HeadlessApp.SendToTable(new RegistrationQueryMessage { PlayerId = "player3" });
    ((WaitForRegistrationViewModel)sut.Central).Start();
    return sut;
  }
}