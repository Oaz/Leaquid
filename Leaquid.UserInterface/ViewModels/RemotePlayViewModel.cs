using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using Leaquid.Core;
using Leaquid.UserInterface.ViewModels.Parts;

namespace Leaquid.UserInterface.ViewModels;

public class RemotePlayViewModel : GameViewModel
{
  public RemotePlayViewModel()
  {
    var register = new RegisterToGameViewModel();
    Central = register;

    register
      .WhenPropertyChanged(x => x.Entered)
      .Where(x => x.Value)
      .Subscribe(_ =>
      {
        IAppContext._.Broker().Sit(
          register.GameCode, seat => Run(seat, register)
        ).DisposeWith(Me);
      })
      .DisposeWith(Me);
  }

  private void Run(IGameBroker.ISeat seat, RegisterToGameViewModel register)
  {
    var pr = new PlayRemote(seat.Id, seat.Listen.OnUserInterfaceThread(), seat.Say);

    pr.CurrentStatus.InitializeAndTrackWith(status =>
      {
        if (status == PlayRemote.Status.Registered)
          register.Registered = true;
        else if (status == PlayRemote.Status.Playing)
        {
          var stageViewModel = new LocalStageViewModel(pr.Stage, pr.Player);
          var framing = new FramingViewModel(stageViewModel);
          framing
            .WhenPropertyChanged(x => x.Size)
            .Subscribe(_ => framing.AdjustTo(framing.Framed.Size))
            .DisposeWith(Me);
          BottomLeft = new StageControlViewModel(framing, pr.Player);
          BottomRight = new PlayerControlViewModel(pr.Player);
          Central = framing;
        }
      })
      .DisposeWith(Me);
  }
}