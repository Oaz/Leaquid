using System.Drawing;
using Leaquid.Core.Actors;

namespace Leaquid.Core;

public class ActiveStage : Stage
{
  public ActiveStage(Size size, int floodWidthFactor, int fillFactor) : base(size)
  {
    var flood = Flood.Init(ActiveFlood.Create, this, Size.Width, floodWidthFactor, fillFactor);
    Subs.Add(flood);
    Flood = flood;
  }
}