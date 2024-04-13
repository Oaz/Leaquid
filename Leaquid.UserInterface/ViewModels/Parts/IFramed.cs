using Avalonia;
using ReactiveUI;

namespace Leaquid.UserInterface.ViewModels.Parts;

public interface IFramed : IReactiveObject
{
  Size Size { get; }
}