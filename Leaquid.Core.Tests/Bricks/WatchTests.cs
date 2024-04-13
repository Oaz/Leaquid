using DynamicData.Kernel;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Bricks;

public class WatchTests
{
  [Test]
  public void EmptyAtInit()
  {
    var sut = new Watch<Foo>();
    Assert.That(sut.Latest, Is.EqualTo(Optional<Foo>.None));
  }

  [Test]
  public void CanBeSet()
  {
    var sut = new Watch<Foo>();
    var someFoo = new Foo();
    sut.Latest = someFoo;
    Assert.That(sut.Latest, Is.EqualTo(Optional<Foo>.Create(someFoo)));
  }

  [Test]
  public void NotifyOnSet()
  {
    var someFoo = new Foo();
    var sut = new Watch<Foo>();
    var notified = false;
    using var register = sut.Updates.Subscribe(value =>
    {
      Assert.That(value, Is.EqualTo(someFoo));
      notified = true;
    });
    sut.Latest = someFoo;
    Assert.True(notified);
  }

  [Test]
  public void DoNotifyOnSetNone()
  {
    var sut = new Watch<Foo>();
    var notified = false;
    using var register = sut.Updates.Subscribe(value =>
    {
      notified = true;
    });
    sut.Latest = Optional<Foo>.None;
    Assert.False(notified);
  }

  class Foo
  {
    
  }
}