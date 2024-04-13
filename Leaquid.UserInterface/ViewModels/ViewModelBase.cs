using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels;

public class ViewModelBase : ReactiveObject, IDisposable
{
  public void OpenUrl(string url) => IAppContext._.OpenUrl(url);
  public string ProjectUrl => IAppContext._.ProjectUrl;

  protected readonly CompositeDisposable Me = new();

  public void Dispose() => Me.Dispose();
}