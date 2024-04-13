using Leaquid.Core;
using Leaquid.Core.Messages;

namespace Leaquid.Network;

internal class Seat : NetworkNode<IMessage>, IGameBroker.ISeat
{
  public async Task Say(IMessage message) => await Send.Invoke(_gameTopic, message);
  
  private readonly string _gameTopic;
  private readonly string _broadcastTopic;

  public Seat(string gameCode)
    : base(Guid.NewGuid().ToString("N"), Network.Messages.AllTypes)
  {
    _gameTopic = CreateTopic(gameCode);
    _broadcastTopic = Table.BroadcastTopic(gameCode);
  }

  public async Task Initialize()
  {
    await Subscribe.Invoke(_broadcastTopic);
    await Send.Invoke(_gameTopic, new RegistrationQueryMessage { PlayerId = Id });
  }

}