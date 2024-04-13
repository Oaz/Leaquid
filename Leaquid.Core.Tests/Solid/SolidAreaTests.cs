using System.Drawing;
using Leaquid.Core.Bricks;
using Leaquid.Core.Solid;
using Moq;

namespace Leaquid.Core.Tests.Solid;

public class SolidAreaTests
{
  [Test]
  public void NoSlopeWhenEmpty()
  {
    CheckSlope(0, 100,0, 100, 0);
  }

  [Test]
  public void SlopeRight()
  {
    _sut.AddOrUpdate(NewSolid(35, 50, 60, 1));
    CheckSlope(0, 100,0, 34, 0);
    CheckSlope(0, 49,35, 35, 0);
    CheckSlope(50, 60,35, 35, 1);
    CheckSlope(61, 100,35, 35, 0);
    CheckSlope(0, 100,36, 100, 0);
  }

  [Test]
  public void SlopeLeft()
  {
    _sut.AddOrUpdate(NewSolid(35, 50, 60, -1));
    CheckSlope(0, 100,0, 34, 0);
    CheckSlope(0, 49,35, 35, 0);
    CheckSlope(50, 60,35, 35, -1);
    CheckSlope(61, 100,35, 35, 0);
    CheckSlope(0, 100,36, 100, 0);
  }

  [Test]
  public void RemoveSlope()
  {
    var s = NewSolid(35, 50, 60, -1);
    _sut.AddOrUpdate(s);
    _sut.AddOrUpdate(NewSolid(45, 50, 60, 1));
    _sut.Remove(s);
    CheckSlope(0, 100,0, 44, 0);
    CheckSlope(0, 49,45, 45, 0);
    CheckSlope(50, 60,45, 45, 1);
    CheckSlope(61, 100,45, 45, 0);
    CheckSlope(0, 100,46, 100, 0);
  }

  [Test]
  public void UpdateSolidVertically()
  {
    var s = NewSolid(45, 50, 60, 1);
    _sut.AddOrUpdate(s);
    CheckSlope(0, 100,0, 44, 0);
    CheckSlope(0, 49,45, 45, 0);
    CheckSlope(50, 60,45, 45, 1);
    CheckSlope(61, 100,45, 45, 0);
    CheckSlope(0, 100,46, 100, 0);
    s.Bounds.Latest = new Bounds(new Point(50, 46), new Size(11, 1));
    _sut.AddOrUpdate(s);
    CheckSlope(0, 100,0, 45, 0);
    CheckSlope(0, 49,46, 46, 0);
    CheckSlope(50, 60,46, 46, 1);
    CheckSlope(61, 100,46, 46, 0);
    CheckSlope(0, 100,47, 100, 0);
  }

  [Test]
  public void UpdateSolidHorizontally()
  {
    var s = NewSolid(45, 50, 60, 1);
    _sut.AddOrUpdate(s);
    CheckSlope(0, 100,0, 44, 0);
    CheckSlope(0, 49,45, 45, 0);
    CheckSlope(50, 60,45, 45, 1);
    CheckSlope(61, 100,45, 45, 0);
    CheckSlope(0, 100,46, 100, 0);
    s.Bounds.Latest = new Bounds(new Point(51, 45), new Size(11, 1));
    _sut.AddOrUpdate(s);
    CheckSlope(0, 100,0, 44, 0);
    CheckSlope(0, 50,45, 45, 0);
    CheckSlope(51, 61, 45, 45, 1);
    CheckSlope(62, 100,45, 45, 0);
    CheckSlope(0, 100,46, 100, 0);
  }

  private SolidArea _sut = null!;

  [SetUp]
  public void Init()
  {
    _sut = new SolidArea(new Size(1000, 1000));
  }

  private void CheckSlope(int xMin, int xMax, int yMin, int yMax, int expectedSlope)
  {
    for (int x = xMin; x <= xMax; x++)
    for (int y = yMin; y <= yMax; y++)
      Assert.That(
        _sut.Slope(x, y),
        Is.EqualTo(expectedSlope),
        () => $"for x={x} y={y}"
      );
  }

  private ISolid NewSolid(int y, int xMin, int xMax, int slope)
  {
    var solid = new Mock<ISolid>();
    var bounds = new Watch<Bounds>
    {
      Latest = new Bounds(new Point(xMin, y), new Size(xMax - xMin + 1, 1))
    };
    solid.Setup(s => s.Bounds).Returns(bounds);
    solid.Setup(s => s.Slope).Returns(slope);
    return solid.Object;
  }
}