using System.Reactive.Disposables;
using System.Reactive.Subjects;
using Leaquid.Core;
using Leaquid.Core.Messages;

namespace Leaquid.UserInterface.Tests.Doubles;

public class FakeBroker : IGameBroker
{
  public FakeTable TheTable { get; }
  public FakeSeat TheSeat { get; }

  public FakeBroker()
  {
    TheTable = new FakeTable();
    TheSeat = new FakeSeat();
  }
  
  public IDisposable Host(Action<IGameBroker.ITable> run)
  {
    run(TheTable);
    return new CompositeDisposable();
  }

  public IDisposable Sit(string tableId, Action<IGameBroker.ISeat> run)
  {
    run(TheSeat);
    return new CompositeDisposable();
  }

  public class FakeTable : IGameBroker.ITable
  {
    public Subject<IMessage> ReceivedMessages { get; } = new ();
    public List<(string,IMessage)> SentMessagesToPlayers { get; } = new();
    public List<IMessage> BroadcastMessages { get; } = new();
    
    public string Id { get; } = "ABCD";
    public IObservable<IMessage> Listen => ReceivedMessages;
    public Task Say(IMessage message)
    {
      BroadcastMessages.Add(message);
      return Task.CompletedTask;
    }

    public Task Say(string seatId, IMessage message)
    {
      SentMessagesToPlayers.Add((seatId,message));
      return Task.CompletedTask;
    }
  }

  public class FakeSeat : IGameBroker.ISeat
  {
    public Subject<IMessage> ReceivedMessages { get; } = new ();
    public List<IMessage> SentMessagesToHost { get; } = new();

    public string Id { get; } = "player666";
    public IObservable<IMessage> Listen => ReceivedMessages;
    public Task Say(IMessage message)
    {
      SentMessagesToHost.Add(message);
      return Task.CompletedTask;
    }
  }
}