using System.Reactive.Subjects;
using System.Runtime.InteropServices.JavaScript;
using Leaquid.Core;
using Leaquid.Core.Messages;

namespace Leaquid.WebRTC;

public class Seat : Node, IGameBroker.ISeat
{
  public Seat(JSObject room)
  {
    _room = room;
    Id = room.GetPropertyAsString("guestId") ?? throw new Exception("Guest ID not found");
    _hostId = room.GetPropertyAsString("hostId") ?? throw new Exception("Host ID not found");
    Interop.OnPeerConnected(
      room, async hostId =>
        await Interop.SendMessage(room, hostId, ToJson(new RegistrationQueryMessage { PlayerId = Id })));
    Interop.OnMessage(
      room, async (_, json) => _messages.OnNext(FromJson(json)));
    var roomCode = room.GetPropertyAsString("roomCode") ?? throw new Exception("Room code not found");
  }

  public string Id { get; }
  private readonly JSObject _room;
  private readonly string _hostId;

  public IObservable<IMessage> Listen => _messages;
  private readonly Subject<IMessage> _messages = new();

  public async Task Say(IMessage message) => await Interop.SendMessage(_room, _hostId, ToJson(message));
}