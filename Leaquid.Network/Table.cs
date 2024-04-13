using Leaquid.Core;
using Leaquid.Core.Messages;

namespace Leaquid.Network;

internal class Table : NetworkNode<IMessage>, IGameBroker.ITable
{
  public async Task Say(IMessage message) => await Send.Invoke(_broadcast, message);

  public async Task Say(string seatId, IMessage message) => await Send.Invoke(CreateTopic(seatId), message);

  internal Table()
    : base(CreateId(new Random()), Network.Messages.AllTypes) =>
    _broadcast = BroadcastTopic(Id);

  private readonly string _broadcast;

  internal static string BroadcastTopic(string gameId) => CreateTopic($"b{gameId}");

  private static string CreateId(Random rnd)
  {
    var chars = Enumerable
      .Range(0, 6)
      .Select(_ => rnd.Next('A', 'Z'))
      .Select(i => (char)i)
      .ToArray();
    return new string(chars);
  }

}