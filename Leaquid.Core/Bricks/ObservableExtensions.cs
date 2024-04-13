using System.Reactive.Linq;

namespace Leaquid.Core.Bricks;

public static class ObservableExtensions
{
  public static IObservable<T> OnlyIf<T>(this IObservable<T> @this, IObservable<bool> condition) =>
    @this.CombineLatest(condition)
      .Where(x => x.Second)
      .Select(x => x.First);
  
  public static IDisposable SubscribeAndWait<T>(this IObservable<T> @this, Func<T,Task> action) =>
    @this
      .Select(action)
      .Subscribe(t => t.Wait());
  
  public static IDisposable SubscribeAndForget<T>(this IObservable<T> @this, Func<T,Task> action) =>
    @this
      .Select(action)
      .Subscribe(t => t.StartIfNotStarted());
  
  public static IObservable<T> DoAndWait<T>(this IObservable<T> @this, Func<T,Task> action) =>
    @this
      .Select(t => (t, action(t)))
      .Do(x => x.Item2.Wait())
      .Select(x => x.t);
  
  public static IObservable<T> DoAndForget<T>(this IObservable<T> @this, Func<T,Task> action) =>
    @this
      .Select(t => (t, action(t)))
      .Do(x => x.Item2.StartIfNotStarted())
      .Select(x => x.t);

  public static void StartIfNotStarted(this Task task)
  {
    if(task.Status == TaskStatus.Created)
      task.Start();
  }
}