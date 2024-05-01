using System;
using System.Linq;
using DynamicData.Binding;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels;

public class RegisterToGameViewModel : ViewModelBase
{
  public RegisterToGameViewModel()
  {
    GameCode = IAppContext._.DefaultGameCode;
    Entered = false;
    this
      .WhenPropertyChanged(x => x.GameCode)
      .Subscribe(p => GameCode = ValidGameCode(p.Value!));
  }
  
  private static string ValidGameCode(string input) => new (input.ToUpper().Where(char.IsUpper).Take(6).ToArray());
  
  public string GameCode
  {
    get => _gameCode;
    set => this.RaiseAndSetIfChanged(ref _gameCode, value);
  }
  private string _gameCode = null!;
  
  public bool Entered
  {
    get => _entered;
    set => this.RaiseAndSetIfChanged(ref _entered, value);
  }
  private bool _entered;

  public bool Registered
  {
    get => _registered;
    set => this.RaiseAndSetIfChanged(ref _registered, value);
  }
  private bool _registered;

  public void Join() => Entered = true;

}