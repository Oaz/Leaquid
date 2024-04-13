using System.Drawing;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Solid;

namespace Leaquid.Core;

public interface IStage
{
  public Size Size { get; }
  public SourceList<Player> Players { get; }
  public List<Wall> Walls { get; }
  public List<Sink> Sinks { get; }
  public List<Tank> Tanks { get; }
  public SolidArea SolidArea { get; }
}

public class EmptyStage : IStage
{
  public EmptyStage(Size size)
  {
    Size = size;
    SolidArea = new SolidArea(size);
  }
  
  public Size Size { get; }
  public SourceList<Player> Players { get; } = new();
  public List<Wall> Walls { get; } = new();
  public List<Sink> Sinks { get; } = new();
  public List<Tank> Tanks { get; } = new();
  public SolidArea SolidArea { get; }
}