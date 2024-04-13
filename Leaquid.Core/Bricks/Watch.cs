using System.Reactive.Subjects;
using DynamicData.Kernel;

namespace Leaquid.Core.Bricks;

public class Watch<T> : IEquatable<Watch<T>>
  where T : notnull
{
  public IDisposable InitializeAndTrackWith(Action<T> tracker)
  {
    Latest.IfHasValue(tracker);
    return Updates.Subscribe(tracker);
  }
  
  public Optional<T> Latest
  {
    get => _latest;
    set
    {
      _latest = value;
      _latest.IfHasValue(v => _updates.OnNext(v));
    }
  }
  private Optional<T> _latest = Optional<T>.None;
  
  public IObservable<T> Updates => _updates;
  private readonly Subject<T> _updates = new ();
  public override string ToString() => $"Watch {_latest}";

  public bool Equals(Watch<T>? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _latest.Equals(other._latest);
  }
}