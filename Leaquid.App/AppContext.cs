using DynamicData.Kernel;
using Leaquid.Core;
using Leaquid.Network;
using Leaquid.UserInterface;

namespace Leaquid.App;


public class AppContext : IAppContext
{
  public AppContext()
  {
    Broker = () => new GameBroker(_mqttBroker);
  }
  
  public string BaseUrl => "https://leaquid.mnt.space/";
  public string ProjectUrl => "https://github.com/Oaz/Leaquid";
  public string PrivacyUrl => "https://leaquid.mnt.space/privacy/";
  public string PlayingUrl(string gameCode) => $"{BaseUrl}?JoinGame={gameCode}";
  public string DefaultGameCode { get; private set; } = string.Empty;
  
  public Action<string> OpenUrl { get; set; } = url => { };
  public Optional<Action> ToggleFullScreen { get; set; } = Optional<Action>.None;

  public Func<TimeSpan, IObservable<long>> Cadence { get; set; } =
    ts => throw new NotSupportedException("No cadence service was defined");

  public Func<IGameBroker> Broker { get; }
  private string _mqttBroker = string.Empty;
  public void UseTcp() => _mqttBroker = TcpUrl;
  public void UseWs() => _mqttBroker = WebsocketUrl;
  private const string TcpUrl = "tcp://mqtt.mnt.space:3423";
  private const string WebsocketUrl = "wss://mqtt.mnt.space:3426";
  
  public string StartupUrlArguments
  {
    set
    {
      if (ParseSearch(value).TryGetValue("JoinGame", out var defaultGameCode))
        DefaultGameCode = defaultGameCode;
    }
  }

  private static Dictionary<string, string> ParseSearch(string search) =>
    search.Length == 0
      ? new Dictionary<string, string>()
      : search.Substring(1)
        .Split('&')
        .Select(p => p.Split('='))
        .ToDictionary(t => t[0], t => t.Length > 1 ? t[1] : "");

}