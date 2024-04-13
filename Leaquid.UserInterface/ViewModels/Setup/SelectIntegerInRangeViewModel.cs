using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Setup;

public class SelectIntegerInRangeViewModel : ViewModelBase
{
  public SelectIntegerInRangeViewModel(int value, int minimum, int maximum)
  {
    Value = value;
    Minimum = minimum;
    Maximum = maximum;
  }

  public int Value
  {
    get => _value;
    set => this.RaiseAndSetIfChanged(ref _value, value);
  }

  private int _value;

  public int Minimum { get; }
  public int Maximum { get; }
}

internal class DesignSelectIntegerInRangeViewModel() : SelectIntegerInRangeViewModel(3, 1, 7);