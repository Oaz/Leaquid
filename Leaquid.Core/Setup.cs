using System.Drawing;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core;

public class Setup : IDisposable
{
  public record Options(int Speed, int PlayerWidthFactor, int FloodWidthFactor, int FillFactor);

  public Setup(Options options)
  {
    _playerWidthFactor = options.PlayerWidthFactor;
    Stage = new ActiveStage(new Size(300, 180), options.FloodWidthFactor, options.FillFactor);
  }
  public List<Player> Players { get; } = new();
  public ActiveStage Stage { get; }
  private readonly float _playerWidthFactor;

  public Player RegisterPlayer()
  {
    var newPlayer = new Player(Players.Count);
    Players.Add(newPlayer);
    Stage.Players.Add(newPlayer);
    return newPlayer;
  }

  public void CompleteRegistration()
  {
    var playingArea = Stage.PlayingArea;
    var wallSpace = playingArea.Origin.X;
    var (playerWidth, playerStep) = ComputePlayerWidthAndSpacing(playingArea, _playerWidthFactor, Players.Count);
    foreach (var player in Players)
    {
      var x = player.Index * playerStep + wallSpace;
      var y = new Random().Next(playingArea.Origin.Y, playingArea.Size.Height);
      player.Spawn((int)x, y, (int)playerWidth, Stage.PlayingArea);
    }
  }

  public static (int playerWidth, int playerStep) ComputePlayerWidthAndSpacing(
    Bounds playingArea, 
    float playerWidthFactor,
    int playerCount)
  {
    var playingWidth = playingArea.Size.Width;
    var factor = (playerWidthFactor-1f)*(playerCount / 100f)+1f;
    var playerWidth = float.Min(playingWidth * factor / playerCount, playingWidth);
    var playerStep = (playingWidth - playerWidth / 2) / playerCount;
    return ((int)playerWidth, (int)playerStep);
  }

  public void Dispose() => Stage.Dispose();
}