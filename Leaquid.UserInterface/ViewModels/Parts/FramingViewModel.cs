using System;
using Avalonia;
using Avalonia.Media;
using DynamicData.Binding;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class FramingViewModel : ViewModelBase
{
  public FramingViewModel(IFramed framed)
  {
    Framed = framed;
    this
      .WhenPropertyChanged(x => x.Bounds)
      .Subscribe(bounds =>
      {
        var width = (float)bounds.Value.Width;
        var height = (float)bounds.Value.Height;
        if (width == 0 || height == 0)
          return;
        Size = new Size(width, height);
      });
  }

  public void AdjustTo(Size visibleSize, Point translate = default)
  {
    if (Size.Width == 0 || Size.Height == 0)
      return;
    var scale = (float)Math.Min(Size.Width / visibleSize.Width, Size.Height / visibleSize.Height);
    var baseTranslation = new Rect(Size).Center - new Rect(Framed.Size).Center;
    var translation = baseTranslation + translate * scale;
    Transformation = new MatrixTransform(
      Matrix.CreateScale(scale, scale) * Matrix.CreateTranslation(translation.X, translation.Y)
    );
  }

  public IFramed Framed
  {
    get => _framed;
    set => this.RaiseAndSetIfChanged(ref _framed, value);
  }

  private IFramed _framed = null!;

  public ITransform Transformation
  {
    get => _transformation;
    set => this.RaiseAndSetIfChanged(ref _transformation, value);
  }

  private ITransform _transformation = null!;

  public Rect Bounds
  {
    get => _bounds;
    set => this.RaiseAndSetIfChanged(ref _bounds, value);
  }

  private Rect _bounds;

  public Size Size
  {
    get => _size;
    private set => this.RaiseAndSetIfChanged(ref _size, value);
  }

  private Size _size;
}

internal class DesignFramingViewModel : FramingViewModel
{
  public DesignFramingViewModel() : base(new DesignStageViewModel())
  {
    this
      .WhenPropertyChanged(x => x.Size)
      .Subscribe(_ => AdjustTo(Framed.Size));
  }
}
