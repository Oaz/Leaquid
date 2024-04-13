using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Leaquid.Multigame.ViewModels;

namespace Leaquid.Multigame;

public class ViewLocator : IDataTemplate
{ 
  private readonly Leaquid.UserInterface.ViewLocator _viewLocator = new();
  
  public Control Build(object? data)
  {
    var name = data!.GetType().FullName!.Replace("ViewModel", "View");
    var type = Type.GetType(name);

    if (type != null)
    {
      return (Control)Activator.CreateInstance(type)!;
    }

    return _viewLocator.Build(data)!;
  }

  public bool Match(object? data)
  {
    return data is ViewModelBase || _viewLocator.Match(data);
  }
}