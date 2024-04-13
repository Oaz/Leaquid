using Avalonia;
using Avalonia.Markup.Xaml;
using Leaquid.Core.Messages;
using Leaquid.UserInterface.Tests.Doubles;

namespace Leaquid.UserInterface.Tests;

public partial class HeadlessApp : Application
{
  public override void Initialize()
  {
    AvaloniaXamlLoader.Load(this);
  }
  
  public static FakeContext Context { get; set; } = null!;

  public static void NewContext()
  {
    Context = new FakeContext();
    Current!.DataContext = Context;
  }

  public static (string,IMessage)[] MessagesSentToPlayers => Context.TheBroker.TheTable.SentMessagesToPlayers.ToArray();
  public static IMessage[] BroadcastMessages => Context.TheBroker.TheTable.BroadcastMessages.ToArray();
  public static void SendToTable(IMessage message)
  {
    Context.TheBroker.TheTable.ReceivedMessages.OnNext(message);
  }
  public static IMessage[] MessagesSentToHost => Context.TheBroker.TheSeat.SentMessagesToHost.ToArray();
  public static void SendToSeat(IMessage message)
  {
    Context.TheBroker.TheSeat.ReceivedMessages.OnNext(message);
  }
}

