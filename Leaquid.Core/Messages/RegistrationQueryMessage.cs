
namespace Leaquid.Core.Messages;

public record RegistrationQueryMessage : IMessage
{
  public required string PlayerId { get; set; }
}