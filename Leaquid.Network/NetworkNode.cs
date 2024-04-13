using System.Reactive.Disposables;
using System.Reactive.Subjects;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;

namespace Leaquid.Network;

public class NetworkNode<TMsg> : IDisposable
{
  public NetworkNode(string id, IEnumerable<Type> messageTypes)
  {
    Id = id;
    _dispose = new CompositeDisposable(_messages, _status);
    _ec = new MessageEncoderDecoder<TMsg>(messageTypes.ToArray());
  }

  public string Id { get; }

  public IObservable<TMsg> Listen => _messages;
  private readonly Subject<TMsg> _messages = new();

  public record ConnectionStatus(bool IsConnected, string Infos);

  public IObservable<ConnectionStatus> Status => _status;
  private readonly Subject<ConnectionStatus> _status = new();

  protected Func<string, TMsg, Task> Send { get; private set; } = null!;
  protected Func<string, Task> Subscribe { get; private set; } = null!;

  private readonly CompositeDisposable _dispose;
  private MessageEncoderDecoder<TMsg> _ec;

  protected static string CreateTopic(string destination) => $"leaquid/{destination}";

  private static MqttClientOptions Tcp(string server, int port) =>
    new MqttClientOptionsBuilder().WithTcpServer(server, port).Build();

  private static MqttClientOptions Web(string url) =>
    new MqttClientOptionsBuilder()
      .WithWebSocketServer(o =>
      {
        o.Uri = url;
        if (url.StartsWith("wss://"))
        {
          o.TlsOptions.UseTls = true;
          o.TlsOptions.AllowUntrustedCertificates = true;
          o.TlsOptions.IgnoreCertificateChainErrors = true;
          o.TlsOptions.IgnoreCertificateRevocationErrors = true;
        }
      })
      .Build();

  public async Task Connect(string mqttBroker)
  {
    var uri = new Uri(mqttBroker);
    var options = uri.Scheme == "tcp"
      ? Tcp(uri.Host, uri.Port)
      : Web(mqttBroker);
    await Connect(options);
  }

  private async Task Connect(MqttClientOptions options)
  {
    try
    {
      var logger = new MqttNetEventLogger();
      logger.LogMessagePublished += (sender, args) =>
      {
        if(args.LogMessage.Level is MqttNetLogLevel.Error or MqttNetLogLevel.Warning)
          Console.WriteLine(args.LogMessage.Message);
      };
      var factory = new MqttFactory();
      var client = factory.CreateMqttClient(logger);
      _dispose.Add(client);
      Send = async (topic, message) =>
      {
        try
        {
          var payload = _ec.Payload(message);
          var mqtt = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            // .WithPayload(message.Payload())
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();
          await client.PublishAsync(mqtt);
        }
        catch (Exception)
        {
          // ignored
        }
      };
      Subscribe = async topic =>
      {
        var subscribeOptions = factory.CreateSubscribeOptionsBuilder()
          .WithTopicFilter(fb => fb
            .WithTopic(topic)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
          )
          .Build();
        var subscribeResponse = await client.SubscribeAsync(
          subscribeOptions,
          CancellationToken.None
        );
        _status.OnNext(new ConnectionStatus(true, $"listening to {topic}"));
      };
      client.ApplicationMessageReceivedAsync += e =>
      {
        var mqttMsg = e.ApplicationMessage;
        _messages.OnNext(_ec.FromMqtt(mqttMsg));
        return Task.CompletedTask;
      };
      client.ConnectedAsync += async e =>
      {
        _status.OnNext(new ConnectionStatus(true, $"connected to {options}"));
        await Subscribe(CreateTopic(Id));
      };
      client.DisconnectedAsync += async e =>
      {
        if (e.Reason == MqttClientDisconnectReason.NormalDisconnection)
        {
          await client.ConnectAsync(options, CancellationToken.None);
          return;
        }

        _status.OnNext(new ConnectionStatus(true,
          $"disconnected from {options}. Reason: {e.Reason} {e.Exception?.Message}"));
      };
      var connectResponse = await client.ConnectAsync(
        options,
        CancellationToken.None
      );
    }
    catch (Exception e)
    {
      _status.OnNext(new ConnectionStatus(false, e.Message));
    }
  }

  private void WriteLine(string s) => Console.WriteLine(s);

  public void Dispose() => _dispose.Dispose();
}