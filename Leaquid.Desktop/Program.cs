using System;
using System.Diagnostics;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.ReactiveUI;
using Leaquid.App;

namespace Leaquid.Desktop;

class Program
{
  // Initialization code. Don't use any Avalonia, third-party APIs or any
  // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
  // yet and stuff might break.
  [STAThread]
  public static void Main(string[] args) => BuildAvaloniaApp()
    .StartWithClassicDesktopLifetime(args);

  // Avalonia configuration, don't remove; also used by visual designer.
  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App.App>()
      .UsePlatformDetect()
      .WithInterFont()
      .SetCadenceService(Observable.Interval)
      .UseMqttWithTcp()
      .SetUrlOpener(OpenUrl)
      .LogToTrace()
      .UseReactiveUI();
  
  private static void OpenUrl(string url)
  {
    using var proc = new Process();
    proc.StartInfo.UseShellExecute = true;
    proc.StartInfo.FileName = url;
    proc.Start();
  }
}