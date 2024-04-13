using System.Drawing;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Messages;

public record StageUpdateMessage : IMessage
{
  public required IEnumerable<PlayerPosition> Players { get; set; }
  public required IEnumerable<BoundsUpdate> TankLevels { get; set; }
  public required IEnumerable<Point> AddDrops { get; set; }
  public required IEnumerable<Point> RemoveDrops { get; set; }
  
  public record PlayerPosition(string Id, Point Origin);

  public record BoundsUpdate(Point Origin, Size Size)
  {
    public static BoundsUpdate From(Bounds b) => new (b.Origin, b.Size);
    public Bounds ToBounds() => new (Origin, Size);
  }
}
