using System.Reactive.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Net;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Leaquid.App;

namespace Leaquid.Android;

[Activity(
  Label = "Leaquid.Android",
  Theme = "@style/MyTheme.NoActionBar",
  Icon = "@drawable/icon",
  MainLauncher = true,
  ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App.App>
{
  protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
  {
    return base.CustomizeAppBuilder(builder)
      .WithInterFont()
      .SetCadenceService(Observable.Interval)
      .UseMqttWithTcp()
      .SetUrlOpener(OpenUrl)
      .UseReactiveUI();
  }
  
  private void OpenUrl(string url)
  {
    var uri = Uri.Parse (url);
    var intent = new Intent (Intent.ActionView, uri);
    StartActivity(intent);
  }

}