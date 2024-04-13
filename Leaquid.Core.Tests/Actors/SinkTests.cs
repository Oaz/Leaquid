using Leaquid.Core.Actors;

namespace Leaquid.Core.Tests.Actors;

public class SinkTests
{
  [Test]
  public void SinksEquality()
  {
    var sink1 = new Sink(50, 100) { Align = { Left = 7, Top = 13 } };
    var sink2 = new Sink(50, 100) { Align = { Left = 7, Top = 13 } };
    Assert.That(sink1, Is.EqualTo(sink2));
    Assert.True(sink1.Equals(sink2));
    Assert.True(sink1.Equals(sink1));
    Assert.False(sink1.Equals(null!));
  }
  
  [Test]
  public void SinkToString()
  {
    var sink1 = new Sink(50, 100) { Align = { Left = 7, Top = 13 } };
    Assert.That(sink1.ToString(), Is.EqualTo("Sink {X=7,Y=13}-{Width=50, Height=100}"));
  }

}