using Leaquid.Core.Messages;

namespace Leaquid.WebRTC;

public class Node() : MessageEncoderDecoder<IMessage>(AllTypes)
{
  private static readonly Type[] AllTypes =
  [
    typeof(PlayerMoveMessage),
    typeof(RegistrationAcceptedMessage),
    typeof(RegistrationQueryMessage),
    typeof(StageUpdateMessage),
    typeof(StartPlayingMessage)
  ];
}