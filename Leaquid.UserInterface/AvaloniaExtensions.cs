using System;
using System.Reactive.Linq;
using Avalonia.Threading;

namespace Leaquid.UserInterface;

public static class AvaloniaExtensions
{
  public static IObservable<T> OnUserInterfaceThread<T>(this IObservable<T> obs) =>
    Observable.Create<T>(observer =>
      obs.Subscribe(t => Dispatcher.UIThread.Post(() => observer.OnNext(t)))
    );
}