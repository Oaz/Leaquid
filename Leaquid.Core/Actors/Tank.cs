using System.Drawing;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Actors;

public class Tank : Actor, IEquatable<Tank>
{
  public Tank(int x, int width, int minLevel) : this(width)
  {
    Align.Left = x;
    Align.Top = minLevel;
  }
  public Tank(int width) : base(new Size(width, 1))
  {
    _filled = 0;
  }

  public void Fill(int count = 1)
  {
    var bounds = Bounds.Latest.Value;
    var width = bounds.Size.Width;
    _filled += count;
    var raise = _filled / width;
    if (raise > 0)
    {
      _filled -= raise * width;
      Bounds.Latest = new Bounds(
        bounds.Origin with { Y = bounds.Origin.Y-raise },
        bounds.Size with { Height = bounds.Size.Height+raise }
      );
    }
  }

  private int _filled;
  public override string ToString() => $"Tank {Bounds.Latest}";
  
  public bool Equals(Tank? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Bounds.Equals(other.Bounds);
  }
}