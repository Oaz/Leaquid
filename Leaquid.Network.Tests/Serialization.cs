using Leaquid.Core.Messages;

namespace Leaquid.Network.Tests;

public class Serialization
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public void Encoding()
  {
    var ec = new MessageEncoderDecoder<IMessage>(Messages.AllTypes);
    Assert.That(
      ec.ToJson(new RegistrationQueryMessage { PlayerId = "Foo" }),
      Is.EqualTo("{\"$type\":\"RegistrationQueryMessage\",\"PlayerId\":\"Foo\"}")
    );
  }

  [Test]
  public void Decoding()
  {
    var ec = new MessageEncoderDecoder<IMessage>(Messages.AllTypes);
    Assert.That(
      ec.FromJson("{\"$type\":\"RegistrationQueryMessage\",\"PlayerId\":\"Foo\"}"),
      Is.EqualTo(new RegistrationQueryMessage { PlayerId = "Foo" })
    );
  }
}