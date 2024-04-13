using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Leaquid.UserInterface.Views.Setup;

public partial class FullScreenButtonView : UserControl
{
  public FullScreenButtonView()
  {
    InitializeComponent();
    IsVisible = IAppContext._.ToggleFullScreen.HasValue;
  }

  private void Button_OnClick(object? sender, RoutedEventArgs e) => IAppContext._.ToggleFullScreen.Value();
}