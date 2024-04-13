using System.Drawing;
using Avalonia.Media;
using SkiaSharp;

namespace Leaquid.UserInterface.ViewModels.Actors;

public class DrawingBuffer
{
  public DrawingBuffer(Size size)
  {
    _bitmap = new SKBitmap(size.Width, size.Height, SKColorType.Rgba8888, SKAlphaType.Opaque);
    _canvas = new SKCanvas(_bitmap);
  }

  public IImage Image => _bitmap.ToAvaloniaImage()!;
  public void Clear(SKColor color) => _canvas.Clear(color);
  public void DrawRect(SKRect rect, SKPaint paint) => _canvas.DrawRect(rect, paint);
  public void SetPixel(Point p, SKColor color) => _bitmap.SetPixel(p.X, p.Y, color);
  
  private readonly SKBitmap _bitmap;
  private readonly SKCanvas _canvas;
}