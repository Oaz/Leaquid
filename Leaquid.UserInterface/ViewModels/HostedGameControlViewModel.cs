using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels;

public class HostedGameControlViewModel : ViewModelBase
{
  public void Play() => IsRunning = true;
  public void Pause() => IsRunning = false;

  public bool IsRunning
  {
    get => _isRunning;
    set => this.RaiseAndSetIfChanged(ref _isRunning, value);
  }
  private bool _isRunning;

}
