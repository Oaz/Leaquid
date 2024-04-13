namespace Leaquid.Core.Messages;

public record PlayerMoveMessage : IMessage
{
  public required string PlayerId { get; set; }
  public required Direction Move { get; set; }

  public enum Direction
  {
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
  }
}