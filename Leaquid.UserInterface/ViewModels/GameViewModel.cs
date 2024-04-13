using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels;

public abstract class GameViewModel : ViewModelBase
{
  public ViewModelBase Central
  {
    get => _central;
    set => this.RaiseAndSetIfChanged(ref _central, value);
  }

  private ViewModelBase _central = null!;

  public ViewModelBase TopLeft
  {
    get => _topLeft;
    set => this.RaiseAndSetIfChanged(ref _topLeft, value);
  }

  private ViewModelBase _topLeft = null!;

  public ViewModelBase BottomLeft
  {
    get => _bottomLeft;
    set => this.RaiseAndSetIfChanged(ref _bottomLeft, value);
  }

  private ViewModelBase _bottomLeft = null!;
  
  public ViewModelBase TopRight
  {
    get => _topRight;
    set => this.RaiseAndSetIfChanged(ref _topRight, value);
  }

  private ViewModelBase _topRight = null!;

  public ViewModelBase BottomRight
  {
    get => _bottomRight;
    set => this.RaiseAndSetIfChanged(ref _bottomRight, value);
  }

  private ViewModelBase _bottomRight = null!;

}