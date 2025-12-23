using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Leaquid.WebRTC;

[SupportedOSPlatform("browser")]
internal static partial class Interop
{
  static Interop()
  {
    Eval(@"
window.toggleFullScreen = function () {
  if(document.fullscreenElement) {
    if (document.exitFullscreen)
      document.exitFullscreen();
    else if (document.webkitExitFullscreen)
      document.webkitExitFullscreen();
    else if (document.msExitFullscreen)
      document.msExitFullscreen();
  } else {
    let doc = document.documentElement;
    if (doc.requestFullscreen)
      doc.requestFullscreen();
    else if (doc.webkitRequestFullscreen)
      doc.webkitRequestFullscreen();
    else if (doc.msRequestFullscreen)
      doc.msRequestFullscreen();
  }
}");
    Eval(@"
window.createRoom = async function (url) {
  let floor = await lobby.reachFloor(url);
  let room = await floor.createRoom({});
  return room;
};
window.joinRoom = async function (url, roomCode) {
  let floor = await lobby.reachFloor(url);
  let room = await floor.joinRoom(roomCode,{});
  return room;
};
window.onPeerConnected = function (room, callback) {
  room.onConnected(callback);
};
window.onMessage = function (room, callback) {
  room.onMessage(callback);
};
window.sendMessage = async function (room, peerId, data) {
  await room.sendMessage(peerId, data);
};
window.broadcastMessage = async function (room, data) {
  await room.broadcastMessage(data);
};
window.lockRoom = async function (room) {
  await room.lock();
};
");
  }

  [JSImport("globalThis.eval")]
  public static partial JSObject Eval(string eval);

  [JSImport("globalThis.toggleFullScreen")]
  public static partial void ToggleFullScreen();

  [JSImport("globalThis.open")]
  public static partial void OpenUrl(string url);

  [JSImport("globalThis.createRoom")]
  public static partial Task<JSObject> CreateRoom(string url);

  [JSImport("globalThis.joinRoom")]
  public static partial Task<JSObject> JoinRoom(string url, string roomCode);

  [JSImport("globalThis.onPeerConnected")]
  public static partial void OnPeerConnected(
    JSObject room,
    [JSMarshalAs<JSType.Function<JSType.String>>]
    Action<string> onConnected
  );

  [JSImport("globalThis.onMessage")]
  public static partial void OnMessage(
    JSObject room,
    [JSMarshalAs<JSType.Function<JSType.String,JSType.String>>]
    Action<string,string> onMessage
  );

  [JSImport("globalThis.lockRoom")]
  public static partial Task LockRoom(JSObject room);

  [JSImport("globalThis.sendMessage")]
  public static partial Task SendMessage(JSObject room, string peerId, string data);

  [JSImport("globalThis.broadcastMessage")]
  public static partial Task BroadcastMessage(JSObject room, string data);

}