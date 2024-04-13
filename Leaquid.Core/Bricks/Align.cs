using System.Drawing;
using DynamicData.Kernel;

namespace Leaquid.Core.Bricks;

public class Align(Size size) : IEquatable<Align>
{
  public Watch<Bounds> Bounds { get; } = new();
  private int? _initialX;
  private int? _initialY;

  public Size Size
  {
    get => size;
    set
    {
      size = value;
      _initialX = null;
      _initialY = null;
      Bounds.Latest = Optional<Bounds>.None;
    }
  }

  public int Left
  {
    set => SetX(value);
  }

  public int Right
  {
    set => SetX(value - Size.Width + 1);
  }

  public int HCenter
  {
    set => SetX(value - Size.Width / 2 + 1);
  }

  private void SetX(int x)
  {
    Bounds.Latest
      .IfHasValue(bounds => Bounds.Latest = new Bounds(
        bounds.Origin with { X = x },
        Size
      ))
      .Else(() => InitIfReadyAfter(() => _initialX = x));
  }

  public int Top
  {
    set => SetY(value);
  }

  public int Bottom
  {
    set => SetY(value - Size.Height + 1);
  }

  public int VCenter
  {
    set => SetY(value - Size.Height / 2 + 1);
  }

  private void SetY(int y)
  {
    Bounds.Latest
      .IfHasValue(bounds => Bounds.Latest = new Bounds(
        bounds.Origin with { Y = y },
        Size
      ))
      .Else(() => InitIfReadyAfter(() => _initialY = y));
  }

  private void InitIfReadyAfter(Action action)
  {
    action();
    if (_initialX.HasValue && _initialY.HasValue)
      Bounds.Latest = new Bounds(
        new Point(_initialX.Value, _initialY.Value),
        Size
      );
  }

  public bool Equals(Align? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Bounds.Equals(other.Bounds);
  }
}