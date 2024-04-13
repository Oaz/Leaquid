using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;

namespace Leaquid.Core;

public class LocalGame
{
  public LocalGame(int numberOfPLayers, Setup.Options options)
  {
    _numberOfPLayers = numberOfPLayers;
    Setup = new Setup(options);
    for (int i = 0; i < numberOfPLayers; i++)
    {
      Setup.RegisterPlayer();
    }
    CurrentPlayer = new Watch<IPlayer>();
    Speed = TimeSpan.FromMilliseconds(800 >> options.Speed);
  }

  public void Start()
  {
    Setup.CompleteRegistration();
    _currentPlayerIndex = new Random().Next(0, _numberOfPLayers - 1);
    CurrentPlayer.Latest = Setup.Players[_currentPlayerIndex];
  }

  public Setup Setup { get; }
  public Watch<IPlayer> CurrentPlayer { get; }
  public TimeSpan Speed { get; }

  private int _currentPlayerIndex;
  private readonly int _numberOfPLayers;
  
  public void NextPlayer()
  {
    _currentPlayerIndex = (_currentPlayerIndex + 1) % _numberOfPLayers;
    CurrentPlayer.Latest = Setup.Players[_currentPlayerIndex];
  }

  public void PreviousPlayer()
  {
    _currentPlayerIndex = (_currentPlayerIndex - 1 + _numberOfPLayers) % _numberOfPLayers;
    CurrentPlayer.Latest = Setup.Players[_currentPlayerIndex];
  }

  public void Next()
  {
    Setup.Stage.Flood.Next();
  }
}