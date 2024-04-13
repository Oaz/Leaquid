using System.Drawing;
using Leaquid.Core.Bricks;
using Leaquid.Core.Solid;
using Moq;

namespace Leaquid.Core.Tests.Solid;

public class SolidRowTests
{
  private SolidRow _sut;

  [SetUp]
  public void Init()
  {
    _sut = new SolidRow();
  }
  
  private void CheckSlope(int xMin, int xMax, int expectedSlope)
  {
    for (int x = xMin; x <= xMax; x++)
      Assert.That(
        _sut.Slope(x),
        Is.EqualTo(expectedSlope),
        () => $"for x={x}"
        );
  }

  [Test]
  public void NoSlopeWhenEmpty()
  {
    CheckSlope(0,1000, 0);
  }

  [Test]
  public void SlopeRight()
  {
    _sut.Add(NewSolid(50, 60, 1));
    CheckSlope(0,49, 0);
    CheckSlope(50,60, 1);
    CheckSlope(61,1000, 0);
  }

  [Test]
  public void SlopeLeft()
  {
    _sut.Add(NewSolid(50, 60, -1));
    CheckSlope(0,49, 0);
    CheckSlope(50,60, -1);
    CheckSlope(61,1000, 0);
  }

  [Test]
  public void MultipleSlopeInSeparateX()
  {
    _sut.Add(NewSolid(50, 60, -1));
    _sut.Add(NewSolid(150, 160, 1));
    CheckSlope(0,49, 0);
    CheckSlope(50,60, -1);
    CheckSlope(61,149, 0);
    CheckSlope(150,160, 1);
    CheckSlope(161,1000, 0);
  }

  [Test]
  public void MultipleSlopeInSameX()
  {
    _sut.Add(NewSolid(50, 60, -1));
    _sut.Add(NewSolid(57, 70, -1));
    _sut.Add(NewSolid(150, 160, 1));
    _sut.Add(NewSolid(155, 170, 1));
    _sut.Add(NewSolid(165, 180, -1));
    CheckSlope(0,49, 0);
    CheckSlope(50,56, -1);
    CheckSlope(57,60, 0);
    CheckSlope(61,70, -1);
    CheckSlope(71,149, 0);
    CheckSlope(150,154, 1);
    CheckSlope(155,160, 0);
    CheckSlope(161,164, 1);
    CheckSlope(165,170, 0);
    CheckSlope(171,180, -1);
    CheckSlope(181,1000, 0);
  }
  
  
  [Test]
  public void NoSlopeOnAdjacentSolids()
  {
    _sut.Add(NewSolid(50, 60, -1));
    _sut.Add(NewSolid(61, 70, 1));
    CheckSlope(0,49, 0);
    CheckSlope(50,59, -1);
    CheckSlope(60,61, 0);
    CheckSlope(62,70, 1);
    CheckSlope(71,1000, 0);
  }


  [Test]
  public void RemoveSlope()
  {
    var s = NewSolid(50, 60, -1);
    _sut.Add(s);
    _sut.Add(NewSolid(150, 160, 1));
    _sut.Remove(s);
    CheckSlope(0,149, 0);
    CheckSlope(150,160, 1);
    CheckSlope(161,1000, 0);
  }

  private ISolid NewSolid(int xMin, int xMax, int slope)
  {
    var solid = new Mock<ISolid>();
    solid
      .Setup(s => s.Bounds)
      .Returns(new Watch<Bounds>
      {
        Latest = new Bounds(new Point(xMin, 0), new Size(xMax - xMin + 1, 0))
      });
    solid.Setup(s => s.Slope).Returns(slope);
    return solid.Object;
  }
}