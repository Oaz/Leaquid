using System;
using Avalonia;
using DynamicData.Kernel;
using Leaquid.Core;

namespace Leaquid.UserInterface;

public interface IAppContext
{
  string BaseUrl { get; }
  string ProjectUrl { get; }
  string PlayingUrl(string gameCode);
  string DefaultGameCode { get; }
  string StartupUrlArguments { set; }
  Action<string> OpenUrl { get; set; }
  Optional<Action> ToggleFullScreen { get; set; }
  Func<TimeSpan, IObservable<long>> Cadence { get; set; }
  
  void UseTcp();
  void UseWs();
  Func<IGameBroker> Broker { get; }

  public static IAppContext _ => (IAppContext)Application.Current!.DataContext!;
}
