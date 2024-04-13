using System.Drawing;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core.Tests.Actors;

public class TankTests
{
  [TestCase(0)]
  [TestCase(1)]
  [TestCase(2)]
  [TestCase(3)]
  [TestCase(4)]
  public void FillDoesNotRaiseLevelIfWidthNotReached(int fill)
  {
    var tank = new Tank(5) { Align = { Left = 7, Top = 13 } };
    tank.Fill(fill);
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(7,13), new Size(5,1))));

  }

  [TestCase(5)]
  [TestCase(6)]
  [TestCase(7)]
  [TestCase(8)]
  [TestCase(9)]
  public void FillRaiseLevelIfWidthReached(int fill)
  {
    var tank = new Tank(5) { Align = { Left = 7, Top = 13 } };
    tank.Fill(fill);
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(7,12), new Size(5,2))));
  }

  [TestCase(10)]
  [TestCase(11)]
  [TestCase(12)]
  [TestCase(13)]
  [TestCase(14)]
  public void FillRaiseLevelMultipleTimesIfWidthReachedMultipleTimes(int fill)
  {
    var tank = new Tank(5) { Align = { Left = 7, Top = 13 } };
    tank.Fill(fill);
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(7,11), new Size(5,3))));
  }

  [TestCase(3, 3, 12, 2)]
  [TestCase(3, 8, 11, 3)]
  public void FillInMultipleSteps(int fill1, int fill2, int expectedY, int expectedH)
  {
    var tank = new Tank(5) { Align = { Left = 7, Top = 13 } };
    tank.Fill(fill1);
    tank.Fill(fill2);
    Assert.That(tank.Bounds.Latest.Value,
      Is.EqualTo(new Bounds(new Point(7, expectedY), new Size(5, expectedH))));
  }
  
  [Test]
  public void TanksEquality()
  {
    var tank1 = new Tank(5) { Align = { Left = 7, Top = 13 } };
    var tank2 = new Tank(5) { Align = { Left = 7, Top = 13 } };
    Assert.That(tank1, Is.EqualTo(tank2));
    Assert.True(tank1.Equals(tank2));
    Assert.True(tank1.Equals(tank1));
    Assert.False(tank1.Equals(null));
  }
  
  [Test]
  public void TanksToString()
  {
    var tank1 = new Tank(5) { Align = { Left = 7, Top = 13 } };
    Assert.That(tank1.ToString(), Is.EqualTo("Tank {X=7,Y=13}-{Width=5, Height=1}"));
  }
  
}