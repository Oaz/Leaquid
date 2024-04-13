using System.Reactive.Subjects;
using DynamicData.Kernel;
using Leaquid.Core;

namespace Leaquid.UserInterface.Tests.Doubles;

public class FakeContext : IAppContext
{
  public FakeContext()
  {
    TheBroker = new FakeBroker();
    Broker = () => TheBroker;
    var cadence = new Subject<long>();
    Cadence = _ => cadence;
    SendCadence = () => cadence.OnNext(0);
  }
  
  public FakeBroker TheBroker { get; }
  public Action SendCadence { get; }

  public string BaseUrl { get; } = "";
  public string ProjectUrl { get; } = "";
  public string PlayingUrl(string gameCode) => $"https://my.site/{gameCode}";

  public string DefaultGameCode { get; set; } = "";
  public string StartupUrlArguments { get; set; } = null!;
  public Action<string> OpenUrl { get; set; } = null!;
  public Optional<Action> ToggleFullScreen { get; set; }
  public Func<TimeSpan, IObservable<long>> Cadence { get; set; }
  public void UseTcp() => throw new NotImplementedException();
  public void UseWs() => throw new NotImplementedException();

  public Func<IGameBroker> Broker { get; }
}