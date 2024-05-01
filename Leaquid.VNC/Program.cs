using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Leaquid.App;

namespace Leaquid.VNC;

class Program
{
  [STAThread]
  public static void Main(string[] args) => BuildAvaloniaApp()
    .StartWithHeadlessVncPlatform("0.0.0.0",5900, args, ShutdownMode.OnMainWindowClose);
  
  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App.App>()
      .UseSkia()
      .WithInterFont()
      .SetCadenceService(Observable.Interval)
      .UseMqttWithTcp()
      .SetUrlOpener(OpenUrl)
      .LogToTrace()
      .UseReactiveUI()
      .AfterSetup(a =>
      {
        Console.WriteLine("Starting VNC server...");
        Console.CancelKeyPress += (sender, args) => Console.WriteLine("Shutting down VNC server.");
      });
  
  private static void OpenUrl(string url) => Console.WriteLine($"Open {url}");
}