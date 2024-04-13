using IntervalTree;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Solid;

public class SolidRow
{
  public int Slope(int x)
  {
    var segments = _segments.Query(x).ToArray();
    if (segments.Length != 1)
      return 0;
    var mainSegment = segments.First();
    var segmentsR = _segments.Query(x+1).ToArray();
    if(segmentsR.Length == 1 && !Equals(mainSegment, segmentsR.First()))
      return 0;
    var segmentsL = _segments.Query(x-1).ToArray();
    if(segmentsL.Length == 1 && !Equals(mainSegment, segmentsL.First()))
      return 0;
    return mainSegment.Slope;
  }

  public void Add(ISolid solid)
  {
    var (xMin, xMax) = GetMinMax(solid.Bounds.Latest.Value);
    _segments.Add(xMin, xMax, solid);
  }

  public void Remove(ISolid solid)
  {
    _segments.Remove(solid);
  }

  private static (int xMin, int xMax) GetMinMax(Bounds bounds)
  {
    var xMin = bounds.Origin.X;
    var xMax = xMin + bounds.Size.Width - 1;
    return (xMin, xMax);
  }

  private readonly IntervalTree<int,ISolid> _segments = new();
}