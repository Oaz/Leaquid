using System.Reactive.Linq;
using System.Reactive.Subjects;
using Leaquid.Core;
using Leaquid.Core.Messages;
using Leaquid.Network;

Console.WriteLine("Game code?");
var gameCode = Console.ReadLine()!.ToUpper();

var mqttBrokerUrl = "wss://mqtt.mnt.space:3426";
var numberOfRemotePlayers = 15;
Console.WriteLine($"Simulating {numberOfRemotePlayers} remote players.");
var players = new Dictionary<string, IGameBroker.ISeat>();
var rnd = new Random();

var startGame = new Subject<string>();
var countdown = numberOfRemotePlayers;
startGame.Subscribe(async seatId =>
{
  Console.WriteLine($"{seatId}: StartPlaying");
  if (--countdown > 0)
    return;
  Console.WriteLine("START GAME");
  var step = 0;
  var move = (PlayerMoveMessage.Direction)rnd.Next(0, 3);
  while (true)
  {
    step++;
    if (step % 10 == 0)
      move = (PlayerMoveMessage.Direction)rnd.Next(0, 3);
    foreach (var player in players.Values)
    {
      Console.WriteLine($"Player {player.Id} moves {move}");
      await player.Say(new PlayerMoveMessage
      {
        PlayerId = player.Id,
        Move = move
      });
    }

    Thread.Sleep(200);
  }
});

var broker = new GameBroker(mqttBrokerUrl);
for (int i = 0; i < numberOfRemotePlayers; i++)
{
  var index = i;
  broker.Sit(gameCode, seat =>
  {
    Console.WriteLine($"Registering player {index} - {seat.Id}");
    seat.Listen.OfType<RegistrationAcceptedMessage>()
      .Subscribe(m => { Console.WriteLine($"{seat.Id}: RegistrationAccepted"); });
    seat.Listen.OfType<StartPlayingMessage>()
      .Subscribe(m => startGame.OnNext(seat.Id));
    players.Add(seat.Id, seat);
  });
  Thread.Sleep(200);
}

Console.WriteLine("Players registered.");
Console.ReadLine();