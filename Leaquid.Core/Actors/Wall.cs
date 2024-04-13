using System.Drawing;
using Leaquid.Core.Solid;

namespace Leaquid.Core.Actors;

public class Wall : Actor, ISolid, IEquatable<Wall>
{
  public Wall(int height, int slope) : base(new Size(Width, height))
  {
    Slope = slope;
  }

  public Wall(int x, int y, int height, int slope) : this(height, slope)
  {
    Align.Left = x;
    Align.Top = y;
  }

  public int Slope { get; }
  public const int Width = 10;
  public override string ToString() => $"Wall {Bounds.Latest}";

  public bool Equals(Wall? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Bounds.Equals(other.Bounds);
  }
}