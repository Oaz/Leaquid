using System;

namespace Leaquid.UserInterface.ViewModels.Setup;

public class UserChoiceViewModel : ViewModelBase
{
  public UserChoiceViewModel(string text, Action action)
  {
    _action = action;
    Text = text;
  }
  public string Text { get; }
  private readonly Action _action;

  public void Execute()
  {
    _action();
  }
}

internal class DesignUserChoiceViewModel() : UserChoiceViewModel("Lorem Ipsum", () =>
{
  Console.WriteLine("Yo!");
});