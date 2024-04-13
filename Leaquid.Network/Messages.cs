using Leaquid.Core.Messages;

namespace Leaquid.Network;

public class Messages
{
  public static Type[] AllTypes = new[]
  {
    typeof(PlayerMoveMessage),
    typeof(RegistrationAcceptedMessage),
    typeof(RegistrationQueryMessage),
    typeof(StageUpdateMessage),
    typeof(StartPlayingMessage),
  };
}