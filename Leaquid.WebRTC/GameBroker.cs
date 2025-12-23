using Leaquid.Core;

namespace Leaquid.WebRTC;

public class GameBroker(string lobbyServerUrl) : IGameBroker
{
  public IDisposable Host(Action<IGameBroker.ITable> run)
  {
    return Task.Run(async () =>
    {
      var room = await Interop.CreateRoom(lobbyServerUrl);
      run(new Table(room));
    });
  }
  
  public IDisposable Sit(string tableId, Action<IGameBroker.ISeat> run)
  {
    return Task.Run(async () =>
    {
      var room = await Interop.JoinRoom(lobbyServerUrl, tableId);
      run(new Seat(room));
    });
  }
}