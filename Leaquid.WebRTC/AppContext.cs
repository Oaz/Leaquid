using System.Text.RegularExpressions;
using DynamicData.Kernel;
using Leaquid.Core;
using Leaquid.UserInterface;

namespace Leaquid.WebRTC;


public class AppContext : IAppContext
{
  public AppContext()
  {
    Broker = () => new GameBroker("https://lobby.mnt.space");
    if (ParseUrlArguments().TryGetValue("JoinGame", out var defaultGameCode))
      DefaultGameCode = defaultGameCode;
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
  public void UseTcp() {}
  public void UseWs() {}
  
  private static Dictionary<string, string> ParseUrlArguments()
  {
    var result = new Dictionary<string, string>();
    var args = Environment.GetCommandLineArgs();
    if (args.Length < 2) return result;
    var index = args[1].IndexOf('?');
    if (index < 0) return result;
    var query = args[1].Substring(index + 1);
    var matches = Regex.Matches(query, @"([^=&]+)(?:=([^&]+))?");
    foreach (Match match in matches)
    {
      var key = match.Groups[1].Value;
      var value = match.Groups[2].Success ? match.Groups[2].Value : string.Empty;
      result[key] = value;
    }

    return result;
  }

}