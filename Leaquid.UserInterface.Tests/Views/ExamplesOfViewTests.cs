using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Headless;
using Leaquid.UserInterface.ViewModels;
using Leaquid.UserInterface.Views;
using Leaquid.UserInterface.Views.Setup;

namespace Leaquid.UserInterface.Tests.Views;

public class ExamplesOfViewTests
{
  [Test]
  public void ExampleOfScreenCapture()
  {
    HeadlessApp.NewContext();
    var window = new MainWindow
    {
      DataContext = new MainViewModel()
    };
    window.Show();

    var toggle = window.Find().Match<HomeView, Grid, ToggleButton>().Exactly();
    window.Click(toggle!.At(0.5,0.5));
    window.CaptureRenderedFrame()?.Save("/tmp/screen1.png");

    var speedOptionSlider = window.Find().Match<OptionsView, Slider>().Loosely(0);
    window.Click(speedOptionSlider!.At(0.75,0.5));
    window.CaptureRenderedFrame()?.Save("/tmp/screen2.png");

    var fillOptionSlider = window.Find().Match<OptionsView, Slider>().Loosely(3);
    window.Click(fillOptionSlider!.At(0.55,0.5));
    window.CaptureRenderedFrame()?.Save("/tmp/screen3.png");
  }
  
}