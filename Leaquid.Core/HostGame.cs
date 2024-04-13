using System.Drawing;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using Leaquid.Core.Actors;
using Leaquid.Core.Bricks;
using Leaquid.Core.Messages;

namespace Leaquid.Core;

public class HostGame : IDisposable
{
  public HostGame(Setup.Options options,
    IObservable<bool> isRunning,
    IObservable<IMessage> incomingMessages,
    Func<string, IMessage, Task> sendToPlayer,
    Func<IMessage, Task> broadcastToAllPlayers)
  {
    var setup = new Setup(options);
    Stage = setup.Stage;
    Dictionary<string, Player> players = new();
    var playersIds = new Dictionary<int, string>();
    Speed = TimeSpan.FromMilliseconds(800 >> options.Speed);
    
    var sub1 = incomingMessages
      .OfType<RegistrationQueryMessage>()
      .SubscribeAndWait(async registration =>
      {
        var player = setup.RegisterPlayer();
        players[registration.PlayerId] = player;
        playersIds[player.Index] = registration.PlayerId;
        PlayersCount.Latest = players.Count;
        await sendToPlayer(registration.PlayerId, new RegistrationAcceptedMessage());
      });
    
    var moves = new Dictionary<PlayerMoveMessage.Direction, Action<Player>>
    {
      [PlayerMoveMessage.Direction.Up] = p => p.Up(),
      [PlayerMoveMessage.Direction.Down] = p => p.Down(),
      [PlayerMoveMessage.Direction.Left] = p => p.Left(),
      [PlayerMoveMessage.Direction.Right] = p => p.Right(),
    };
    
    var sub2 = incomingMessages
      .OfType<PlayerMoveMessage>()
      .OnlyIf(isRunning)
      .Subscribe(msg => moves[msg.Move](players[msg.PlayerId]));
    
    async Task UpdateStage(IEnumerable<Point> addedDrops, IEnumerable<Point> removedDrops)
    {
      var tankLevels = Stage.Tanks
        .Select(t => StageUpdateMessage.BoundsUpdate.From(t.Bounds.Latest.Value));
      await broadcastToAllPlayers(new StageUpdateMessage
      {
        Players = setup.Players
          .Select(p => new StageUpdateMessage.PlayerPosition(playersIds[p.Index], p.Bounds.Latest.Value.Origin))
          .ToArray(),
        TankLevels = tankLevels,
        AddDrops = addedDrops,
        RemoveDrops = removedDrops
      });
    }

    var dropUpdate = false;
    
    var sub3 = Stage.Flood.Drops.Connect().SubscribeAndForget(async dc =>
    {
      var addDrops = new List<Point>();
      var removeDrops = new List<Point>();
      foreach (var change in dc)
      {
        if (change.Reason == ChangeReason.Add)
          addDrops.Add(change.Key);
        if (change.Reason == ChangeReason.Remove)
          removeDrops.Add(change.Key);
      }
      dropUpdate = true;
      await UpdateStage(addDrops, removeDrops);
    });

    var sub4 = Stage.Flood.Updated
      .SubscribeAndForget(async _ =>
      {
        if (!dropUpdate) await UpdateStage(Array.Empty<Point>(), Array.Empty<Point>());
        dropUpdate = false;
      });

    _onStart = async () =>
    {
      setup.CompleteRegistration();
      await broadcastToAllPlayers(new StartPlayingMessage
      {
        StageSize = Stage.Size,
        PlayingAreaOrigin = Stage.PlayingArea.Origin,
        PlayingAreaSize = Stage.PlayingArea.Size,
        Options = options,
        Players = playersIds.Select(
          p => new StartPlayingMessage.PlayerDefinition(p.Key, p.Value)
        )
      });
    };

    _subs = new CompositeDisposable(setup, sub1, sub2, sub3, sub4);
  }

  public TimeSpan Speed { get; }
  public Watch<int> PlayersCount { get; } = new ();
  public Stage Stage { get; }
  public async Task Start() => await _onStart();
  public void Next() => Stage.Flood.Next();
  public void Dispose() => _subs.Dispose();
  
  private readonly Func<Task> _onStart;
  private readonly CompositeDisposable _subs;

}