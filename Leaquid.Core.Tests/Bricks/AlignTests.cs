using System.Drawing;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Bricks;

public class AlignTests
{
  private Size _size, _newSize;

  [SetUp]
  public void Init()
  {
    _size = new Size(20, 10);
    _newSize = new Size(30, 20);
  }

  [Test]
  public void LeftAndTopPositionsDirectlySetXAndY()
  {
    var sut = new Align(_size)
    {
      Left = 15,
      Top = 25
    };
    Assert.That(sut.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(15,25)));
  }

  [Test]
  public void RightAndBottomPositionsSetOppositePoint()
  {
    var sut = new Align(_size)
    {
      Right = 100,
      Bottom = 50
    };
    Assert.That(sut.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(81,41)));
  }

  [Test]
  public void HAndVCenterPositionsAlignToCenterPoint()
  {
    var sut = new Align(_size)
    {
      HCenter = 100,
      VCenter = 50
    };
    Assert.That(sut.Bounds.Latest.Value.Origin, Is.EqualTo(new Point(91,46)));
  }

  [Test]
  public void SettingOnlyOneCoordinateDoesNotDefineAnOrigin()
  {
    Assert.False(new Align(_size) { Left = 15 }.Bounds.Latest.HasValue);
    Assert.False(new Align(_size) { Right = 15 }.Bounds.Latest.HasValue);
    Assert.False(new Align(_size) { HCenter = 15 }.Bounds.Latest.HasValue);
    Assert.False(new Align(_size) { Top = 15 }.Bounds.Latest.HasValue);
    Assert.False(new Align(_size) { Bottom = 15 }.Bounds.Latest.HasValue);
    Assert.False(new Align(_size) { VCenter = 15 }.Bounds.Latest.HasValue);
  }
  
  [Test]
  public void OriginIsInvalidatedWhenSizeChanges()
  {
    var sut = new Align(_size)
    {
      Left = 15,
      Top = 25
    };
    sut.Size = _newSize;
    Assert.That(sut.Bounds.Latest.HasValue, Is.EqualTo(false));
  }

}