using System.Drawing;
using Leaquid.Core.Bricks;
using Leaquid.Core.Solid;

namespace Leaquid.Core.Actors;

public interface IPlayer : ISolid
{
  void Up();
  void Down();
  void Left();
  void Right();
  int Index { get; }
}

public class Player : IPlayer, IEquatable<Player>
{
  // public const int Height = 5;
  public const int Height = 3;

  public Player(int index)
  {
    Index = index;
    Slope = (index % 2) * 2 - 1;
    Bounds = new Watch<Bounds>();
  }

  public void Spawn(int x, int y, int width, Bounds playingArea)
  {
    _playingArea = playingArea;
    Bounds.Latest = AdjustBounds(
      new Point(x, y),
      new Size(width, Height),
      _playingArea
    );
  }

  public void Up() => Move(p => new Point(p.X, p.Y - 1));
  public void Down() => Move(p => new Point(p.X, p.Y + 1));
  public void Left() => Move(p => new Point(p.X - 1, p.Y));
  public void Right() => Move(p => new Point(p.X + 1, p.Y));

  private void Move(Func<Point, Point> move) =>
    Bounds.Latest = AdjustBounds(
      move(CurrentPosition.Origin),
      CurrentPosition.Size,
      _playingArea
    );

  private static Bounds AdjustBounds(Point origin, Size size, Bounds playingArea) =>
    new(StayInPlayingArea(origin, size, playingArea), size);

  private static Point StayInPlayingArea(Point position, Size size, Bounds playingArea) =>
    new(
      StayIn(position.X, playingArea.XMin, playingArea.XMax - size.Width + 1),
      StayIn(position.Y, playingArea.YMin, playingArea.YMax - size.Height + 1)
    );

  private static int StayIn(int value, int min, int max) => Math.Max(min, Math.Min(max, value));

  public int Index { get; }
  public int Slope { get; }
  public Watch<Bounds> Bounds { get; }
  private Bounds _playingArea;
  private Bounds CurrentPosition => Bounds.Latest.Value;

  public bool Equals(Player? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return Index == other.Index && Bounds.Latest.Equals(other.Bounds.Latest);
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(null, obj)) return false;
    if (ReferenceEquals(this, obj)) return true;
    return Equals((Player)obj);
  }

  public override int GetHashCode() => HashCode.Combine(Index, Bounds);
}