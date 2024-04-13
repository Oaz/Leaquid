using Foundation;
using UIKit;
using Avalonia;
using Avalonia.Controls;
using Avalonia.iOS;
using Avalonia.Media;
using Avalonia.ReactiveUI;

namespace Leaquid.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
[Register("AppDelegate")]
public partial class AppDelegate : AvaloniaAppDelegate<App.App>
{
  protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
  {
    return base.CustomizeAppBuilder(builder)
      .WithInterFont()
      .UseReactiveUI();
  }
}