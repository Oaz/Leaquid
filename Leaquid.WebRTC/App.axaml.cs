using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Leaquid.UserInterface;
using Leaquid.UserInterface.ViewModels;
using Leaquid.UserInterface.Views;

namespace Leaquid.WebRTC;

public partial class App : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
    DataContext =  new AppContext();
  }

  public override void OnFrameworkInitializationCompleted()
  {
    if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
    {
      singleViewPlatform.MainView = new MainView
      {
        DataContext = new MainViewModel()
      };
    }

    base.OnFrameworkInitializationCompleted();
  }
}

public static class ConfigureApp
{
  public static AppBuilder SetCadenceService(this AppBuilder builder, Func<TimeSpan,IObservable<long>> cadenceService) =>
    builder.AfterSetup(b => Context.Cadence = cadenceService);

  public static AppBuilder DefineFullScreenToggle(this AppBuilder builder, Action toggleFullScreen) =>
    builder.AfterSetup(b => Context.ToggleFullScreen = toggleFullScreen);
  
  public static AppBuilder SetUrlOpener(this AppBuilder builder, Action<string> openUrl) =>
    builder.AfterSetup(b => Context.OpenUrl = openUrl);

  private static IAppContext Context => IAppContext._;
}
