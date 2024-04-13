using System.Collections.ObjectModel;
using System.Drawing;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Actors;

public class ActiveFloodTests
{
  private EmptyStage _stage = null!;
  private ReadOnlyObservableCollection<Flood.Drop> _actualDrops = null!;
  private int _adds;
  private int _removes;
  private IDisposable _sub = null!;

  [SetUp]
  public void Init()
  {
    _stage = new EmptyStage(new Size(100,100));
    _adds = 0;
    _removes = 0;
  }

  [TearDown]
  public void Cleanup()
  {
    _sub.Dispose();
  }

  private ActiveFlood DefineSut(ActiveFlood f)
  {
    _sub = f.Drops.Connect()
      .OnItemAdded(_ => _adds++)
      .OnItemRemoved(_ => _removes++)
      .Bind(out _actualDrops)
      .Subscribe();
    return f;
  }

  private ActiveFlood CreateActiveFlood(int width)
  {
    var flood = ActiveFlood.Create(width, _stage, 1);
    flood.Align.Top = 7;
    flood.Align.Left = 50;
    return flood;
  }

  private void CheckActualDrops(IEnumerable<Flood.Drop> expected, int expectedAdds, int expectedRemoves)
  {
    Assert.That(_actualDrops, Is.EqualTo(expected));
    Assert.That(_adds, Is.EqualTo(expectedAdds));
    Assert.That(_removes, Is.EqualTo(expectedRemoves));
  }

  [Test]
  public void FloodingSingleDrop()
  {
    var sut = DefineSut(CreateActiveFlood(1));
    sut.Next();
    CheckActualDrops(new[] {new Flood.Drop(50,8)},1,0);
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(50, 9),
    },2,0);
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(50,10),
    },3,0);
  }

  [Test]
  public void FloodingMultipleDrops()
  {
    var sut = DefineSut(CreateActiveFlood(2));
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
    },2,0);
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(51, 9),
    },4,0);
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(51, 9),
      new Flood.Drop(50, 10),
      new Flood.Drop(51, 10),
    },6,0);
  }

  [Test]
  public void DropIntoSink()
  {
    _stage.Sinks.Add(new Sink(100,90) {Align = { Top = 10, Left = 0}});
    var sut = DefineSut(CreateActiveFlood(2));
    sut.Next();
    sut.Next();
    sut.Next();
    sut.Next();
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(51, 9),
    },4,0);
  }

  [Test]
  public void DropIntoTank()
  {
    var tank = new Tank(2) {Align = { Top = 11, Left = 50}};
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(50,11), new Size(2,1))));
    _stage.Tanks.Add(tank);
    var sut = DefineSut(CreateActiveFlood(2));
    sut.Next();
    sut.Next();
    sut.Next();
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(51, 9),
      new Flood.Drop(50, 10),
      new Flood.Drop(51, 10),
    },6,0);
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(50, 10), new Size(2,2))));
  }

  [Test]
  public void DropOntoSolid()
  {
    var wall = new Wall(20, 1) {Align = { Top = 11, Left = 50}};
    Assert.That(wall.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(50,11), new Size(10,20))));
    _stage.SolidArea.AddOrUpdate(wall);
    var sut = DefineSut(CreateActiveFlood(2));
    sut.Next();
    sut.Next();
    sut.Next();
    sut.Next();
    CheckActualDrops(new[]
    {
      new Flood.Drop(50,8),
      new Flood.Drop(51,8),
      new Flood.Drop(50, 9),
      new Flood.Drop(51, 9),
      new Flood.Drop(50, 10),
      new Flood.Drop(51, 10),
      // new Flood.Drop(51, 10),
      new Flood.Drop(52, 10),
    },7,0);
  }
  
  [Test]
  public void NoMovementOnInactiveFlood()
  {
    var flood = Flood.Create(10, _stage, 1);
    Assert.Throws<NotSupportedException>(() => flood.Next());
  }
}