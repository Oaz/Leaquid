using System;
using System.Reactive.Subjects;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Leaquid.App;
using Leaquid.Browser;

[assembly: SupportedOSPlatform("browser")]

internal partial class Program
{
  private static async Task Main(string[] args)
  {
    await BuildAvaloniaApp()
      .WithInterFont()
      .SetCadenceService(interval =>
      {
        var notifier = new Subject<long>();
        var dt = new DispatcherTimer(
          interval,
          DispatcherPriority.Input,
          (sender, eventArgs) => notifier.OnNext(DateTime.Now.Ticks)
          );
        dt.Start();
        return notifier;
      })
      .UseMqttWithWs()
      .InterpretStartupUrlArguments(args[0])
      .DefineFullScreenToggle(Interop.ToggleFullScreen)
      .SetUrlOpener(Interop.OpenUrl)
      .UseReactiveUI()
      .StartBrowserAppAsync("out");
  }


  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>();

}