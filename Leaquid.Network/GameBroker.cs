using Leaquid.Core;

namespace Leaquid.Network;

public class GameBroker : IGameBroker
{
  private readonly string _mqttBroker;

  public GameBroker(string mqttBroker) => _mqttBroker = mqttBroker;

  public IDisposable Host(Action<IGameBroker.ITable> run)
  {
    var table = new Table();
    return table.ConnectAndRun(_mqttBroker, () => run(table));
  }
  
  public IDisposable Sit(string tableId, Action<IGameBroker.ISeat> run)
  {
    var seat = new Seat(tableId);
    return seat.ConnectAndRun(_mqttBroker, async () =>
    {
      await seat.Initialize();
      run(seat);
    });
  }
}