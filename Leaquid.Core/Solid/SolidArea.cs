using System.Drawing;

namespace Leaquid.Core.Solid;

public class SolidArea
{
  private readonly SolidRow[] _rows;
  private Dictionary<ISolid, int> _rowOfSolid = new();

  public SolidArea(Size size)
  {
    _rows = Enumerable.Range(0, size.Height).Select(_ => new SolidRow()).ToArray();
  }

  public int Slope(int x, int y) => _rows[y].Slope(x);

  public void AddOrUpdate(ISolid solid)
  {
    if(_rowOfSolid.TryGetValue(solid, out var row))
      _rows[row].Remove(solid);
    var newRow = solid.Bounds.Latest.Value.YMin;
    _rowOfSolid[solid] = newRow;
    _rows[newRow].Add(solid);
  }

  public void Remove(ISolid solid)
  {
    _rows[solid.Bounds.Latest.Value.YMin].Remove(solid);
  }
}