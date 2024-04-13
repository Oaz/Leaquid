using Leaquid.Core.Actors;

namespace Leaquid.Core.Tests.Actors;

public class WallTests
{
    
  [Test]
  public void WallsEquality()
  {
    var wall1 = new Wall(100, 1) { Align = { Left = 7, Top = 13 } };
    var wall2 = new Wall(100, 1) { Align = { Left = 7, Top = 13 } };
    Assert.That(wall1, Is.EqualTo(wall2));
    Assert.True(wall1.Equals(wall2));
    Assert.True(wall1.Equals(wall1));
    Assert.False(wall1.Equals(null!));
  }
  
  [Test]
  public void WallToString()
  {
    var wall1 = new Wall(100, 1) { Align = { Left = 7, Top = 13 } };
    Assert.That(wall1.ToString(), Is.EqualTo("Wall {X=7,Y=13}-{Width=10, Height=100}"));
  }

}