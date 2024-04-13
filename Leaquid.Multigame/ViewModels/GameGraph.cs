using AvaloniaGraphControl;

namespace Leaquid.Multigame.ViewModels;

public class GameGraph : Graph
{
  public GameGraph()
  {
    Orientation = Orientations.Horizontal;
    var options = new Core.Setup.Options(1, 3, 1, 1);
    var a = new EmbedGameViewModel(options, 5);
    var b = new EmbedGameViewModel(options, 10);
    var c = new EmbedGameViewModel(options, 15);
    var d = new EmbedGameViewModel(options, 20);
    var e = new EmbedGameViewModel(options, 25);
    Edges.Add(new Edge(a, b));
    Edges.Add(new Edge(a, d));
    Edges.Add(new Edge(a, e));
    Edges.Add(new Edge(b, c));
    Edges.Add(new Edge(b, d));
    Edges.Add(new Edge(d, a));
    Edges.Add(new Edge(d, e));
    Edges.Add(new Edge("Hello, world!", e));
    Edges.Add(new Edge(a, "This is the end"));
    var s1 = "Here";
    Edges.Add(new Edge(s1, c));
    Edges.Add(new Edge(b, s1));
    var s2 = "And there";
    Edges.Add(new Edge(s2, e));
    Edges.Add(new Edge(c, s2));
  }
}