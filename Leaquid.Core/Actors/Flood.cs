using System.Drawing;
using System.Reactive.Subjects;
using DynamicData;

namespace Leaquid.Core.Actors;

public class Flood : Actor
{
  public static Flood Create(int width, IStage stage, int fillFactor) => new (width, stage);

  public static T Init<T>(
    Func<int, IStage, int, T> creator,
    IStage stage, int width, int floodWidthFactor, int fillFactor
    ) where T:Flood
  {
    var centerX = (width - 1) / 2;
    var floodWidth = width / (64>>floodWidthFactor);
    var flood = creator(floodWidth, stage, fillFactor);
    flood.Align.Top = 0;
    flood.Align.HCenter = centerX;
    return flood;
  }
  
  protected Flood(int width, IStage stage) : base(new Size(width, 1))
  {
    Stage = stage;
    Drops = new(x => x.Position);
  }

  public virtual void Next() => throw new NotSupportedException("No movement on inactive flood");
  
  public void Update(IEnumerable<Point> added, IEnumerable<Point> removed)
  {
    Drops.Edit(drops =>
    {
      drops.AddOrUpdate(added.Select(p => new Drop(p.X, p.Y)));
      drops.Remove(removed);
    });
    _updated.OnNext(true);
  }
  
  public record Drop
  {
    public Drop(int x, int y) => Position = new Point(x, y);
    public Point Position { get; }
  }

  public IObservable<bool> Updated => _updated;
  private readonly Subject<bool> _updated = new();

  public SourceCache<Drop, Point> Drops { get; }
  public Size Size => Stage.Size;
  protected readonly IStage Stage;
}