using Avalonia;
using Avalonia.Headless;

namespace Leaquid.UserInterface.Tests;

[SetUpFixture]
public class HeadlessAppFixture
{
  [OneTimeSetUp]
  public void Init()
  {
    AppBuilder
      .Configure<HeadlessApp>()
      .UseSkia()
      .UseHeadless(new AvaloniaHeadlessPlatformOptions
      {
        UseHeadlessDrawing = false
      })
      .SetupWithoutStarting();
  }
}