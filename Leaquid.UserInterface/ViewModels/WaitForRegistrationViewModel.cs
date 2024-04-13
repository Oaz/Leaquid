using System.Reactive.Disposables;
using Avalonia.Media;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.QrCode;

namespace Leaquid.UserInterface.ViewModels;

public class WaitForRegistrationViewModel : ViewModelBase
{
  public WaitForRegistrationViewModel(string gameId)
  {
    PlayerCount = 0;
    GameId = gameId;
    GameUrl = IAppContext._.PlayingUrl(gameId);
    Started = false;
    CreateQrCode();
  }

  private void CreateQrCode()
  {
    var size = 512;
    using var qrCodeGenerator = new QRCodeGenerator();
    var qrCode = qrCodeGenerator.CreateQrCode(GameUrl, ECCLevel.L);
    var qrInfo = new SKImageInfo(size, size);
    using var skSurface = SKSurface.Create(qrInfo);
    skSurface.Canvas.Render(qrCode, qrInfo.Width, qrInfo.Height);
    using var image = skSurface.Snapshot();
    var bitmap = SKBitmap.FromImage(image).DisposeWith(Me);
    QrCode = bitmap.ToAvaloniaImage()!;
  }

  public void Start()
  {
    Started = true;
  }

  public string GameId { get; }

  public int PlayerCount
  {
    get => _playerCount;
    set => this.RaiseAndSetIfChanged(ref _playerCount, value);
  }

  private int _playerCount;

  public bool Started
  {
    get => _started;
    set => this.RaiseAndSetIfChanged(ref _started, value);
  }

  private bool _started;
  
  public IImage QrCode
  {
    get => _qrCode;
    set => this.RaiseAndSetIfChanged(ref _qrCode, value);
  }

  private IImage _qrCode = null!;

  public string GameUrl { get; }
}

internal class DesignWaitForRegistrationViewModel() : WaitForRegistrationViewModel("ABCDEF");