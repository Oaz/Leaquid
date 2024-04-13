using System.Drawing;
using System.Reactive.Disposables;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core;


public class Stage : EmptyStage, IDisposable
{
  public Stage(Size size) : base(size)
  {
    var disposables = new List<IDisposable>();
    var wallSpace = size.Width / 16;
    var wallHeight = (int)(Size.Height * 0.4);
    var centerX = (Size.Width - 1) / 2;
    AddWalls(wallHeight, wallSpace);
    Tanks.Add(new Tank(Size.Width - 2 * (wallSpace + Wall.Width))
    {
      Align = { Bottom = Size.Height - 1, HCenter = centerX }
    });
    AddSinks(wallHeight, wallSpace);

    PlayingArea = new Bounds(
      new Point(wallSpace, 10),
      new Size(Size.Width - 2 * wallSpace, Size.Height - 10 - wallHeight)
    );
    
    foreach (var wall in Walls) SolidArea.AddOrUpdate(wall);
    var trackPlayers = Players.Connect()
      .OnItemAdded(player =>
      {
        disposables.Add(
          player.Bounds.Updates.Subscribe(_ => SolidArea.AddOrUpdate(player))
        );
      })
      .Subscribe();
    disposables.Add(trackPlayers);
    Subs = new CompositeDisposable(disposables);
  }

  private void AddSinks(int wallHeight, int wallSpace)
  {
    var sinkHeight = wallHeight * 7 / 8;
    Sinks.Add(new Sink(wallSpace, sinkHeight)
    {
      Align = { Bottom = Size.Height - 1, Left = 0 }
    });
    Sinks.Add(new Sink(wallSpace, sinkHeight)
    {
      Align = { Bottom = Size.Height - 1, Right = Size.Width - 1 }
    });
  }

  private void AddWalls(int wallHeight, int wallSpace)
  {
    var wall1 = new Wall(wallHeight, -1)
    {
      Align = { Bottom = Size.Height - 1, Left = wallSpace }
    };
    Walls.Add(wall1);
    var wall2 = new Wall(wallHeight, 1)
    {
      Align = { Bottom = Size.Height - 1, Right = Size.Width - wallSpace - 1 }
    };
    Walls.Add(wall2);
  }
  public Flood Flood { get; protected init; } = null!;

  public Bounds PlayingArea { get; }
  protected readonly CompositeDisposable Subs;

  public void Dispose() => Subs.Dispose();
}