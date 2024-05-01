using System.Reactive.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Leaquid.App;

namespace Leaquid.Framebuffer;

class Program
{
  [STAThread]
  public static void Main(string[] args) => BuildAvaloniaApp()
    .StartLinuxFbDev(args);
  
  public static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App.App>()
      .WithInterFont()
      .With(new FontManagerOptions
      {
        DefaultFamilyName = "avares://Avalonia.Fonts.Inter/Assets#Inter"
      })
      .SetCadenceService(Observable.Interval)
      .UseMqttWithTcp()
      .SetUrlOpener(OpenUrl)
      .LogToTrace()
      .UseReactiveUI();
  
  private static void OpenUrl(string url) => Console.WriteLine($"Open {url}");
}