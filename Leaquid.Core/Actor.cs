using System.Drawing;
using Leaquid.Core.Bricks;

namespace Leaquid.Core;

public class Actor(Size size)
{
  public Align Align { get; } = new(size);
  public Watch<Bounds> Bounds => Align.Bounds;
}