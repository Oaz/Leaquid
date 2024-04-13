using System.Drawing;

namespace Leaquid.Core.Messages;

public record StartPlayingMessage : IMessage
{
  public required Size StageSize { get; set; }
  public required Point PlayingAreaOrigin { get; set; }
  public required Size PlayingAreaSize { get; set; }
  public required Setup.Options Options { get; set; }
  public required IEnumerable<PlayerDefinition> Players { get; set; }
  
  public record PlayerDefinition(int Index, string Id);

}