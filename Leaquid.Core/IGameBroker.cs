using Leaquid.Core.Messages;

namespace Leaquid.Core;

public interface IGameBroker
{
  IDisposable Host(Action<ITable> run);

  public interface ITable
  {
    string Id { get; }
    IObservable<IMessage> Listen { get; }
    Task Say(IMessage message);
    Task Say(string seatId, IMessage message);
  }
  
  IDisposable Sit(string tableId, Action<ISeat> run);

  public interface ISeat
  {
    string Id { get; }
    IObservable<IMessage> Listen { get; }
    Task Say(IMessage message);
  }
}