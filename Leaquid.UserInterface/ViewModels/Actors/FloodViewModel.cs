using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Media;
using DynamicData;
using Leaquid.Core;
using Leaquid.Core.Actors;
using ReactiveUI;
using SkiaSharp;
using Size = System.Drawing.Size;

namespace Leaquid.UserInterface.ViewModels.Actors;

public class FloodViewModel : ViewModelBase, IActor
{
  public FloodViewModel(Flood flood)
  {
    var bounds = flood.Bounds.Latest.Value;
    var sourceRect = new SKRect(
      bounds.Origin.X, bounds.Origin.Y,
      bounds.BottomRight.X+1, bounds.BottomRight.Y+1
    );
    var size = flood.Size;
    var buffers = Enumerable.Range(0, 2).Select(_ => new DrawingBuffer(size)).ToArray();
    var paint = new SKPaint
    {
      Color = SKColors.DarkGray,
      Style = SKPaintStyle.Fill,
    };
    flood.Drops.Connect()
      .Bind(out ReadOnlyObservableCollection<Flood.Drop> drops)
      .Subscribe()
      .DisposeWith(Me);

    var bufferIndex = 0;
    bufferIndex = Draw();
    flood.Updated.Subscribe(_ => { bufferIndex = Draw(); });

    int Draw()
    {
      bufferIndex = 1 - bufferIndex;
      buffers[bufferIndex].Clear(SKColors.Bisque);
      buffers[bufferIndex].DrawRect(sourceRect, paint);
      foreach (var drop in drops)
        buffers[bufferIndex].SetPixel(drop.Position, _waterColors[_random.Next(0,4)]);
      Image = buffers[bufferIndex].Image;
      return bufferIndex;
    }
  }

  public IImage Image
  {
    get => _image;
    set => this.RaiseAndSetIfChanged(ref _image, value);
  }
  private IImage _image = null!;

  public Rect Bounds
  {
    get => _bounds;
    set => this.RaiseAndSetIfChanged(ref _bounds, value);
  }
  private Rect _bounds;
  
  private readonly SKColor[] _waterColors = new[]
  {
    SKColors.DodgerBlue,
    SKColors.SkyBlue,
    SKColors.DodgerBlue,
    SKColors.DarkBlue,
  };

  private readonly Random _random = new ();
}


internal class DesignFloodViewModel() : FloodViewModel(DesignFlood.Instance);

internal class DesignFlood : Flood
{
  public static readonly Size StageSize = new Size(300, 200);
  public static readonly DesignFlood Instance = Init(
    (w, s, _) => new DesignFlood(w, s),
    new Stage(StageSize), StageSize.Width, 1, 0
    );

  private DesignFlood(int w, IStage s) : base(w, s) => AddDrops();

  private void AddDrops()
  {
    var rnd = new Random();
    var points = Enumerable.Range(0, 100)
      .SelectMany(_ => Block(rnd.Next(0, StageSize.Width), rnd.Next(0, StageSize.Height), rnd.Next(10, 30), rnd.Next(10, 30)))
      .Where(p => p.X < StageSize.Width && p.Y < StageSize.Height)
      .ToArray();
    Update(points, Array.Empty<System.Drawing.Point>());
  }

  private static IEnumerable<System.Drawing.Point> Block(int x0, int y0, int w, int h) =>
    from x in Enumerable.Range(x0, w)
    from y in Enumerable.Range(y0, h)
    select new System.Drawing.Point(x, y);
}