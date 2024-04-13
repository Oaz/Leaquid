using System;
using System.Diagnostics.CodeAnalysis;
using DynamicData.Binding;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Parts;

public class ZoomLevel : ViewModelBase
{
  private readonly Action _activate;

  public ZoomLevel(string name, [NotNull] Action activate, bool isActive)
  {
    _activate = activate;
    this
      .WhenPropertyChanged(x => x.IsActive)
      .Subscribe(prop =>
      {
        if (prop.Value)
          activate();
      });
    Name = name;
    IsActive = isActive;
  }
  
  public string Name { get; }
  
  public bool IsActive
  {
    get => _isActive;
    set => this.RaiseAndSetIfChanged(ref _isActive, value);
  }
  private bool _isActive;

  public void Refresh()
  {
    _activate();
  }
}