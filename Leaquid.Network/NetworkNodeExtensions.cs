using System.Reactive.Linq;
using Leaquid.Core.Bricks;

namespace Leaquid.Network;

public static class NetworkNodeExtensions
{
  public static IDisposable ConnectAndRun<TMsg>(
    this NetworkNode<TMsg> networkNode,
    string mqttBroker,
    Action run
  ) => DoConnectAndRun(networkNode, mqttBroker, run, r => r());

  public static IDisposable ConnectAndRun<TMsg>(
    this NetworkNode<TMsg> networkNode,
    string mqttBroker,
    Func<Task> run
  ) => DoConnectAndRun(networkNode, mqttBroker, run, r => r().StartIfNotStarted());
  
  private static IDisposable DoConnectAndRun<TMsg,TRunner>(
    this NetworkNode<TMsg> networkNode,
    string mqttBroker,
    TRunner run,
    Action<TRunner> doRun
  ) =>
    networkNode.Status
      .Prepend(new NetworkNode<TMsg>.ConnectionStatus(false, "Startup"))
      .Do(Console.WriteLine)
      .DoAndForget(async status =>
      {
        if (!status.IsConnected)
          await networkNode.Connect(mqttBroker);
      })
      .Scan((before, after) =>
      {
        if (!before.IsConnected && after.IsConnected)
          doRun(run);
        return after;
      })
      .Subscribe();
}