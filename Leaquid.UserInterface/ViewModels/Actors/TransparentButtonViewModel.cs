using System;
using Avalonia;
using Leaquid.Core.Bricks;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Actors;

public class TransparentButtonViewModel : ViewModelBase, IActor
{
  private readonly Action _activate;

  public TransparentButtonViewModel(Align align, Action activate)
  {
    _activate = activate;
    align.Bounds.InitializeAndTrackWith(bounds =>
    {
      Bounds = new Rect(
        new Point(bounds.Origin.X, bounds.Origin.Y), 
        new Size(bounds.Size.Width, bounds.Size.Height)
      );
    });
  }

  public Rect Bounds
  {
    get => _bounds;
    set => this.RaiseAndSetIfChanged(ref _bounds, value);
  }
  private Rect _bounds;
  
  public void Activate() => _activate();
}

internal class DesignTransparentButtonViewModel() : TransparentButtonViewModel(
  new Align(new System.Drawing.Size(100, 50))
  {
    HCenter = 100,
    Bottom = 100
  },
  () => Console.WriteLine("CLICKED")
);