using System.Reactive.Subjects;
using System.Runtime.InteropServices.JavaScript;
using Leaquid.Core;
using Leaquid.Core.Messages;

namespace Leaquid.WebRTC;

public class Table : Node, IGameBroker.ITable
{
  public Table(JSObject room)
  {
    _room = room;
    Id = room.GetPropertyAsString("roomCode") ?? throw new Exception("Room code not found");
    Interop.OnMessage(room, (_, json) => _messages.OnNext(FromJson(json)));
  }

  public string Id { get; }
  private readonly JSObject _room;

  public IObservable<IMessage> Listen => _messages;
  private readonly Subject<IMessage> _messages = new();

  public async Task Say(IMessage message)
  {
    if (message is StartPlayingMessage)
      await Interop.LockRoom(_room);
    await Interop.BroadcastMessage(_room, ToJson(message));
  }

  public async Task Say(string seatId, IMessage message) =>
    await Interop.SendMessage(_room, seatId, ToJson(message));
}