using System.Drawing;
using System.Reactive.Disposables;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Actors;

public class ActiveFlood : Flood, IDisposable
{
  public new static ActiveFlood Create(int width, IStage stage, int fillFactor) => new (width, stage, fillFactor);

  private ActiveFlood(int width, IStage stage, int fillFactor) : base(width, stage)
  {
    _fillFactor = fillFactor;
    _sinks = stage.Sinks
      .SelectMany(sink => GetSurface(sink.Bounds.Latest.Value)).ToHashSet();
    _tanks = stage.Tanks
      .SelectMany(tank => GetSurface(tank.Bounds.Latest.Value).Select(p => (p, tank)))
      .ToDictionary(pt => pt.p, pt => pt.tank);
    _subs = new CompositeDisposable(
      Bounds.InitializeAndTrackWith(bounds =>
        _sources = GetSurface(Bounds.Latest.Value)
          .Select(p => p with { Y = p.Y + 1 }).ToArray()
      )
    );
  }

  private IEnumerable<Point> GetSurface(Bounds bounds) =>
    Enumerable
      .Range(bounds.Origin.X, bounds.Size.Width)
      .Select(x => bounds.Origin with { X = x });

  private readonly IDisposable _subs;
  private readonly int _fillFactor;
  private IEnumerable<Point> _sources = null!;
  private readonly HashSet<Point> _sinks;
  private readonly Dictionary<Point, Tank> _tanks;

  public override void Next()
  {
    var adding = new HashSet<Point>(_sources);
    foreach (var pointDrop in Drops.KeyValues)
    {
      var point = pointDrop.Key;
      var newPoint = point with { Y = point.Y + 1 };
      if (_sinks.Contains(newPoint))
        continue;
      if (_tanks.TryGetValue(newPoint, out var tank))
      {
        tank.Fill(_fillFactor);
        continue;
      }

      var slope = Stage.SolidArea.Slope(newPoint.X, newPoint.Y);
      var addedPoint = slope == 0 ? newPoint : point with { X = point.X + slope };
      adding.Add(addedPoint);
    }

    var added = adding.Except(Drops.Keys);
    var removed = Drops.Keys.Except(adding);

    Update(added, removed);
  }
  
  public void Dispose() => _subs.Dispose();
}