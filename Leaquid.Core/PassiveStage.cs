using System.Drawing;
using Leaquid.Core.Actors;

namespace Leaquid.Core;

public class PassiveStage: Stage
{
  public PassiveStage(Size size, int floodWidthFactor) : base(size)
  {
    Flood = Flood.Init(Flood.Create, this, Size.Width, floodWidthFactor, 0);
  }
}