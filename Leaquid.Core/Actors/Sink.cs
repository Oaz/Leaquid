using System.Drawing;

namespace Leaquid.Core.Actors;

public class Sink : Actor, IEquatable<Sink>
{
  public Sink(int width, int height) : base(new Size(width, height))
  {
  }

  public bool Equals(Sink? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return this.Align.Equals(other.Align);
  }

  public override string ToString() => $"Sink {Bounds.Latest}";

}