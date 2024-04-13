using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Leaquid.Core.Bricks;
using ReactiveUI;
using Size = System.Drawing.Size;

namespace Leaquid.UserInterface.ViewModels.Actors;

public class DecorViewModel : ViewModelBase, IActor
{
  public DecorViewModel(string name)
  {
    var uri = new Uri($"avares://Leaquid.UserInterface/Assets/{name}.png");
    Image = new Bitmap(AssetLoader.Open(uri));
    var size = new Size(Image.PixelSize.Width,Image.PixelSize.Height);
    Align = new Align(size);
    Align.Bounds.InitializeAndTrackWith(bounds =>
    {
      Bounds = new Rect(
        new Point(bounds.Origin.X, bounds.Origin.Y), 
        new Avalonia.Size(bounds.Size.Width, bounds.Size.Height)
        );
    });
  }

  public Bitmap Image { get; }
  public Align Align { get; }

  public Rect Bounds
  {
    get => _bounds;
    set => this.RaiseAndSetIfChanged(ref _bounds, value);
  }
  private Rect _bounds;

  private static int Choose(int? before, int? after, int delta)
  {
    if (before.HasValue)
      return before.Value;
    if (after.HasValue)
      return after.Value - delta + 1;
    return default;
  }
}

internal class DesignDecorViewModel() : DecorViewModel("castle_pxl")
{
}