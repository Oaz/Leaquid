using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Leaquid.UserInterface.ViewModels;

namespace Leaquid.UserInterface;

public class ViewLocator : IDataTemplate
{
  public Control? Build(object? data) => data is null ? null : FindView(data.GetType());

  private Control? FindView(Type viewModelType)
  {
    try
    {
      var name = viewModelType.FullName!.Replace("ViewModel", "View");
      var type = Type.GetType(name);
      if (type != null)
        return (Control)Activator.CreateInstance(type)!;

      var parent = viewModelType.BaseType;
      if(parent == null)
        return new TextBlock { Text = name };

      return FindView(parent);
    }
    catch (Exception e)
    {
      Console.WriteLine($"Cannot find view for {viewModelType.FullName}");
      Console.WriteLine(e);
      throw;
    }
  }

  public bool Match(object? data)
  {
    return data is ViewModelBase;
  }
}