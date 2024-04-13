using System.Drawing;
using System.Reactive.Linq;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;
using Leaquid.Core.Messages;

namespace Leaquid.Core;

public class PlayRemote
{
  public PlayRemote(string playerId, IObservable<IMessage> incomingMessages, Func<IMessage, Task> sendToHost)
  {
    CurrentStatus.Latest = Status.Created;
    
    incomingMessages
      .OfType<RegistrationAcceptedMessage>()
      .Subscribe(msg => CurrentStatus.Latest = Status.Registered);

    incomingMessages
      .OfType<StartPlayingMessage>()
      .Subscribe(msg =>
      {
        Stage = new PassiveStage(msg.StageSize, msg.Options.FloodWidthFactor);
        Dictionary<string, Player> players = msg.Players
          .ToDictionary(d => d.Id, d => new Player(d.Index));
        foreach (var p in players.Values) Stage.Players.Add(p);
        Player.Latest = new PlayerProxy(players[playerId], move => sendToHost(new PlayerMoveMessage
        {
          PlayerId = playerId,
          Move = move
        }));
        
        var (playerWidth, _) =
          Setup.ComputePlayerWidthAndSpacing(Stage.PlayingArea, msg.Options.PlayerWidthFactor, players.Count);
        var playerSize = new Size(playerWidth, Actors.Player.Height);

        _updateStage = stageUpdateMessage =>
        {
          Stage.Flood.Update(stageUpdateMessage.AddDrops, stageUpdateMessage.RemoveDrops);
          foreach (var tankUpdate in Stage.Tanks.Zip(stageUpdateMessage.TankLevels))
            tankUpdate.First.Bounds.Latest = tankUpdate.Second.ToBounds();
          foreach (var position in stageUpdateMessage.Players)
          {
            var player = players[position.Id];
            player.Bounds.Latest =  new Bounds(position.Origin, playerSize);
          }
        };

        CurrentStatus.Latest = Status.Playing;
      });
    
    incomingMessages
      .OfType<StageUpdateMessage>()
      .Subscribe(msg => _updateStage!(msg));
  }

  public enum Status
  {
    Created,
    Registered,
    Playing
  }
  
  public Stage Stage { get; private set; } = null!;
  public Watch<Status> CurrentStatus { get; } = new ();
  public Watch<IPlayer> Player { get; } = new ();

  private Action<StageUpdateMessage> _updateStage = default!;

  private class PlayerProxy(IPlayer player, Func<PlayerMoveMessage.Direction, Task> move) : IPlayer
  {
    public Watch<Bounds> Bounds { get; } = player.Bounds;
    public int Slope { get; } = player.Slope;
    public int Index { get; } = player.Index;
    public async void Up() => await move(PlayerMoveMessage.Direction.Up);
    public async void Down() => await move(PlayerMoveMessage.Direction.Down);
    public async void Left() => await move(PlayerMoveMessage.Direction.Left);
    public async void Right() => await move(PlayerMoveMessage.Direction.Right);
  }

}