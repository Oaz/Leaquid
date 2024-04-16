using System;
using System.Reactive.Linq;
using DynamicData.Kernel;
using Leaquid.Core;
using Leaquid.UserInterface;

namespace Leaquid.Multigame;

public class AppContext : IAppContext
{
  public string BaseUrl { get; } = "";
  public string ProjectUrl { get; } = "";
  public string PrivacyUrl { get; } = "";
  public string PlayingUrl(string gameCode)
  {
    throw new NotImplementedException();
  }

  public string DefaultGameCode { get; } = "";
  public string StartupUrlArguments { get; set; } = null!;
  public Action<string> OpenUrl { get; set; } = null!;
  public Optional<Action> ToggleFullScreen { get; set; }
  public Func<TimeSpan, IObservable<long>> Cadence { get; set; } = Observable.Interval;

  public void UseTcp()
  {
    throw new NotImplementedException();
  }

  public void UseWs()
  {
    throw new NotImplementedException();
  }

  public Func<IGameBroker> Broker { get; } = null!;
}