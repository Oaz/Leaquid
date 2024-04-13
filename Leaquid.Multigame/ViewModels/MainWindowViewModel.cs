using AvaloniaGraphControl;

namespace Leaquid.Multigame.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
  public Graph TheGraph { get; } = new GameGraph();
}