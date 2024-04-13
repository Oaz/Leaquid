using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Input;
using Avalonia.LogicalTree;

namespace Leaquid.UserInterface.Tests.Views;

public static class ViewsExtensions
{
  public static void Click(this Window window, Point p, MouseButton button = MouseButton.Left)
  {
    window.MouseDown(p, button);
    window.MouseUp(p, button);
  }

  public static Rect GlobalBounds(this Visual visual)
  {
    if (visual.Parent == null)
      return visual.Bounds;
    var parentOrigin = ((Visual)visual.Parent).GlobalBounds();
    return new Rect(visual.Bounds.Position + parentOrigin.Position, visual.Bounds.Size);
  }

  public static Point At(this Visual visual, double xPercent, double yPercent)
  {
    var bounds = visual.GlobalBounds();
    return new Point(bounds.X + bounds.Width * xPercent, bounds.Y + bounds.Height * yPercent);
  }

  public static Search Find(this ILogical logical) => new(logical);

  public class Search(ILogical root)
  {
    public Search<T1> Match<T2, T1>() => new(root, [typeof(T1), typeof(T2)]);
    public Search<T1> Match<T3, T2, T1>() => new(root, [typeof(T1), typeof(T2), typeof(T3)]);
    public Search<T1> Match<T4, T3, T2, T1>() => new(root, [typeof(T1), typeof(T2), typeof(T3), typeof(T4)]);
    public Search<T1> Match<T5, T4, T3, T2, T1>() =>
      new(root, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5)]);
    public Search<T1> Match<T6, T5, T4, T3, T2, T1>() =>
      new(root, [typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6)]);
  }

  public class Search<T>(ILogical root, Type[] pattern)
  {
    public T? Exactly(int n = 0) => (T?) FindLogical(root, ancestry =>
    {
      var slice = ancestry.Take(pattern.Length).ToArray();
      return slice.Length == pattern.Length && pattern.SequenceEqual(slice.Select(x => x.GetType()));
    }, n);
    
    public T? Loosely(int n = 0) => (T?) FindLogical(root, ancestry =>
    {
      var haystack = ancestry.Select(a => a.GetType()).ToList();
      var needle = pattern.ToList();
      while (haystack.Count > 0 && needle.Count > 0)
      {
        if (needle.First() == haystack.First())
          needle.RemoveAt(0);
        haystack.RemoveAt(0);
      }
      return needle.Count == 0;
    }, n);
  }

  private static ILogical? FindLogical(ILogical logical, Predicate<Stack<ILogical>> condition, int n)
  {
    var ancestry = new Stack<ILogical>();
    return DepthFirstSearch(logical, a =>
    {
      if (!condition(a))
        return false;
      if (n == 0)
        return true;
      n--;
      return false;
    }, ancestry);
  }

  private static ILogical? DepthFirstSearch(ILogical logical, Predicate<Stack<ILogical>> condition,
    Stack<ILogical> ancestry)
  {
    ancestry.Push(logical);
    if (condition(ancestry))
      return logical;
    foreach (var child in logical.GetLogicalChildren())
    {
      var descendant = DepthFirstSearch(child, condition, ancestry);
      if (descendant != null)
        return descendant;
    }

    ancestry.Pop();
    return default;
  }
}