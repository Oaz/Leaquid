using System.Drawing;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Bricks;

public class BoundsTests
{
  [TestCase(0,0,20,10,19,9)]
  [TestCase(15,5,30,21,44,25)]
  public void Coordinates(int x, int y, int width, int height, int brx, int bry)
  {
    var sut = new Bounds(new Point(x, y), new Size(width, height));
    Assert.That(sut.Origin, Is.EqualTo(new Point(x, y)));
    Assert.That(sut.Size, Is.EqualTo(new Size(width, height)));
    Assert.That(sut.BottomRight, Is.EqualTo(new Point(brx, bry)));
  }
}