namespace Leaquid.UserInterface.ViewModels.Setup;

public class OptionsViewModel : ViewModelBase
{
  public OptionsViewModel()
  {
    Speed = new SelectIntegerInRangeViewModel(2, 1, 5);
    PlayerWidthFactor = new SelectIntegerInRangeViewModel(1, 1, 5);
    FloodWidthFactor = new SelectIntegerInRangeViewModel(3, 1, 5);
    FillFactor = new SelectIntegerInRangeViewModel(5, 1, 12);
  }

  public Core.Setup.Options Values => new (Speed.Value, PlayerWidthFactor.Value, FloodWidthFactor.Value, FillFactor.Value);

  public SelectIntegerInRangeViewModel Speed { get; }
  public SelectIntegerInRangeViewModel PlayerWidthFactor { get; }
  public SelectIntegerInRangeViewModel FloodWidthFactor { get; }
  public SelectIntegerInRangeViewModel FillFactor { get; }
  

}