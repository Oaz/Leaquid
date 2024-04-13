using System.Drawing;

namespace Leaquid.Core.Bricks;

public readonly struct Bounds(Point origin, Size size)
{
  public Point Origin { get; } = origin;
  public int XMin => Origin.X;
  public int YMin => Origin.Y;
  public Point Center => Origin + (Size*0.5f).ToSize();
  public Size Size { get; } = size;
  public int Width => Size.Width;
  public int Height => Size.Height;
  public Point BottomRight { get; } = origin + size - new Size(1,1);
  public int XMax => BottomRight.X;
  public int YMax => BottomRight.Y;
  public override string ToString() => $"{Origin}-{Size}";
}