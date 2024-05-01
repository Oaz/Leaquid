using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using Leaquid.UserInterface.ViewModels.Setup;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels;

public class MainViewModel : ViewModelBase
{
  public MainViewModel()
  {
    _home = new HomeViewModel();
    _home.UserChoices
      .WhenPropertyChanged(x => x.Selection)
      .Subscribe(p =>
      {
        if (p.Value == null)
        {
          Game = _home;
          return;
        }

        var game = (GameViewModel)p.Value;
        game
          .WhenPropertyChanged(x => x.Exit)
          .Where(x => x.Value)
          .Subscribe(x => BackToHome());
        Game = game;
      }).DisposeWith(Me);
    BackToHome();
  }

  public void BackToHome()
  {
    if (Game is GameViewModel game)
      game.Dispose();
    Game = _home;
  }

  public ViewModelBase Game
  {
    get => _game;
    set => this.RaiseAndSetIfChanged(ref _game, value);
  }

  private ViewModelBase _game = null!;
  private readonly HomeViewModel _home;
}