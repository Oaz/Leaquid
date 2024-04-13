using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using DynamicData;
using MQTTnet;

namespace Leaquid.Network;

public class MessageEncoderDecoder<TMsg>
{
  public MessageEncoderDecoder(params Type[] messageTypes)
  {
    _options = new JsonSerializerOptions()
    {
      TypeInfoResolver = new TypeResolver(messageTypes),
    };
  }

  private readonly JsonSerializerOptions _options;

  public IEnumerable<byte> Payload(TMsg msg) => Encoding.UTF8.GetBytes(ToJson(msg));
  internal string ToJson(TMsg msg) => JsonSerializer.Serialize(msg, _options);

  public TMsg FromMqtt(MqttApplicationMessage mqtt) => FromJson(Encoding.UTF8.GetString(mqtt.Payload));
  internal TMsg FromJson(string json) => JsonSerializer.Deserialize<TMsg>(json, _options)!;

  class TypeResolver : DefaultJsonTypeInfoResolver
  {
    private readonly JsonPolymorphismOptions _polymorphism;

    public TypeResolver(IEnumerable<Type> messageTypes)
    {
      _polymorphism = new JsonPolymorphismOptions
      {
        IgnoreUnrecognizedTypeDiscriminators = true,
        UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
      };
      _polymorphism.DerivedTypes.AddRange(messageTypes.Select(mt => new JsonDerivedType(mt, mt.Name)));
    }

    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
      var jsonTypeInfo = base.GetTypeInfo(type, options);
      if (type == typeof(TMsg))
        jsonTypeInfo.PolymorphismOptions = _polymorphism;
      return jsonTypeInfo;
    }
  }
}