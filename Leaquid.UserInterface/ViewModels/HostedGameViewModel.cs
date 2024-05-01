using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Threading;
using DynamicData.Binding;
using Leaquid.Core;
using Leaquid.Core.Bricks;
using Leaquid.UserInterface.ViewModels.Parts;

namespace Leaquid.UserInterface.ViewModels;

public class HostedGameViewModel : GameViewModel
{
  public HostedGameViewModel(Core.Setup.Options options)
  {
    IAppContext._.Broker()
      .Host(table => Dispatcher.UIThread.Post(() => Run(options, table)))
      .DisposeWith(Me);
  }

  private void Run(Core.Setup.Options options, IGameBroker.ITable table)
  {
    var waitForRegistration = new WaitForRegistrationViewModel(table.Id);
    Central = waitForRegistration;
    
    var control = new HostedGameControlViewModel();
    var isRunning = control
      .WhenPropertyChanged(x => x.IsRunning)
      .Select(r => r.Value);

    var host = new HostGame(
      options,
      isRunning,
      table.Listen, table.Say, table.Say
      );
    host.DisposeWith(Me);
    host.PlayersCount
      .InitializeAndTrackWith(playerCount => waitForRegistration.PlayerCount = playerCount)
      .DisposeWith(Me);

    waitForRegistration
      .WhenPropertyChanged(x => x.Started)
      .Where(x => x.Value)
      .SubscribeAndForget(async _ =>
      {
        var stage = new StageViewModel(host.Stage);
        stage
          .WhenPropertyChanged(x => x.Leave)
          .Subscribe(p => Exit = p.Value)
          .DisposeWith(Me);
        
        var framing = new FramingViewModel(stage);
        framing
          .WhenPropertyChanged(x => x.Size)
          .Subscribe(_ => framing.AdjustTo(framing.Framed.Size))
          .DisposeWith(Me);
        Central = framing;
        BottomRight = control;
        
        await host.Start();
        control.Play();

        IAppContext._
          .Cadence(host.Speed)
          .OnlyIf(isRunning)
          .Do(_ => host.Next())
          .Subscribe()
          .DisposeWith(Me);

      })
      .DisposeWith(Me);
  }
}
