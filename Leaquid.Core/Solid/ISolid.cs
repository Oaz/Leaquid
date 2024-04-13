using Leaquid.Core.Bricks;

namespace Leaquid.Core.Solid;

public interface ISolid
{
  Watch<Bounds> Bounds { get; }
  int Slope { get; }
}