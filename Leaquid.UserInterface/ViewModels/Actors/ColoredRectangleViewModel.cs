using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Media;
using Leaquid.Core.Bricks;
using ReactiveUI;

// ReSharper disable NonReadonlyMemberInGetHashCode

namespace Leaquid.UserInterface.ViewModels.Actors;

public class ColoredRectangleViewModel : ViewModelBase, IActor, IEquatable<ColoredRectangleViewModel>
{
  public ColoredRectangleViewModel(Watch<Bounds> boundsWatcher)
  {
    boundsWatcher.InitializeAndTrackWith(bounds =>
    {
      Bounds = new Rect(bounds.Origin.X, bounds.Origin.Y, bounds.Size.Width, bounds.Size.Height);
    }).DisposeWith(Me);
  }

  public Rect Bounds
  {
    get => _bounds;
    set => this.RaiseAndSetIfChanged(ref _bounds, value);
  }

  private Rect _bounds;

  public IBrush Brush
  {
    get => _brush;
    set => this.RaiseAndSetIfChanged(ref _brush, value);
  }

  private IBrush _brush = null!;

  public override string ToString()
  {
    return $"ColoredRectangleViewModel {_bounds} {_brush}";
  }

  public bool Equals(ColoredRectangleViewModel? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _bounds.Equals(other._bounds) && _brush.Equals(other._brush);
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    if (obj.GetType() != this.GetType()) return false;
    return Equals((ColoredRectangleViewModel)obj);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(_bounds, _brush);
  }
}

internal class DesignColoredRectangleViewModel : ColoredRectangleViewModel
{
  public DesignColoredRectangleViewModel()
    : base(new Watch<Bounds>
    {
      Latest = new Bounds(
        new System.Drawing.Point(0,0),
        new System.Drawing.Size(100,50))
    })
  {
    Brush = Brushes.Salmon;
  }
}