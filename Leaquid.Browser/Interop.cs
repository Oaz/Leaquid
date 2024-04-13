using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Leaquid.Browser;

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
  }

  [JSImport("globalThis.eval")]
  public static partial JSObject Eval(string eval);

  [JSImport("globalThis.toggleFullScreen")]
  public static partial void ToggleFullScreen();

  [JSImport("globalThis.open")]
  public static partial void OpenUrl(string url);

}