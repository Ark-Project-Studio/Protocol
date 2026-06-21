using Protocol.Codec.IO;
using Protocol.Codec.Packets;
using Protocol.Network;
using Protocol.Utils.IO;

var tests = new (string Name, Action Run)[]
{
	("McbeLogin round-trip", TestMcbeLoginRoundTrip),
	("McbeAnimate optional round-trip", TestMcbeAnimateRoundTrip),
	("McbePlayStatus round-trip", TestMcbePlayStatusRoundTrip),
	("McbeText round-trip", TestMcbeTextRoundTrip),
	("McbeMovePlayer round-trip", TestMcbeMovePlayerRoundTrip),
	("Packet compression round-trip", TestPacketCompressionRoundTrip),
	("Encode reflects field mutation", TestEncodeReflectsFieldMutation),
	("Truncated string throws", TestTruncatedStringThrows),
	("PacketFactory preserves known dispatch", TestPacketFactoryKnownDispatch)
};

var failures = 0;
foreach (var (name, run) in tests)
{
    try
    {
        run();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception ex)
    {
        failures++;
        Console.Error.WriteLine($"FAIL {name}: {ex.GetType().Name}: {ex.Message}");
    }
}

return failures == 0 ? 0 : 1;

static void TestMcbeLoginRoundTrip()
{
    var packet = new McbeLogin
    {
        protocolVersion = ProtocolInfo.McProtocol,
        payload = [0x01, 0x02, 0x03, 0x04]
    };

    var decoded = RequireType<McbeLogin>(RoundTrip(packet));

    Equal(packet.protocolVersion, decoded.protocolVersion);
    SequenceEqual(packet.payload, decoded.payload);
    SequenceEqual(packet.Encode(), decoded.Encode());
}

static void TestMcbeAnimateRoundTrip()
{
    var packet = new McbeAnimate
    {
        actionId = 3,
        runtimeEntityId = 123456789,
        Data = 1.25f,
        swingSource = new Optional<string>("main_hand")
    };

    var decoded = RequireType<McbeAnimate>(RoundTrip(packet));

    Equal(packet.actionId, decoded.actionId);
    Equal(packet.runtimeEntityId, decoded.runtimeEntityId);
    Equal(packet.Data, decoded.Data);
    Equal(true, decoded.swingSource.HasValue);
    Equal(packet.swingSource.Value, decoded.swingSource.Value);
	SequenceEqual(packet.Encode(), decoded.Encode());
}

static void TestMcbePlayStatusRoundTrip()
{
	var packet = new McbePlayStatus
	{
		status = PlayStatus.PlayerSpawn
	};

	var decoded = RequireType<McbePlayStatus>(RoundTrip(packet));

	Equal(packet.status, decoded.status);
	SequenceEqual(packet.Encode(), decoded.Encode());
}

static void TestMcbeTextRoundTrip()
{
	var packet = new McbeText
	{
		TextType = TextType.Chat,
		NeedsTranslation = false,
		SourceName = "Steve",
		Message = "hello",
		XUID = "xuid-1",
		PlatformChatID = "platform-1",
		FilteredMessage = new Optional<string>("filtered")
	};

	var decoded = RequireType<McbeText>(RoundTrip(packet));

	Equal(packet.TextType, decoded.TextType);
	Equal(packet.NeedsTranslation, decoded.NeedsTranslation);
	Equal(packet.SourceName, decoded.SourceName);
	Equal(packet.Message, decoded.Message);
	Equal(packet.XUID, decoded.XUID);
	Equal(packet.PlatformChatID, decoded.PlatformChatID);
	Equal(true, decoded.FilteredMessage.HasValue);
	Equal(packet.FilteredMessage.Value, decoded.FilteredMessage.Value);
	SequenceEqual(packet.Encode(), decoded.Encode());
}

static void TestMcbeMovePlayerRoundTrip()
{
	var packet = new McbeMovePlayer
	{
		runtimeEntityId = 77,
		x = 1.5f,
		y = 64.25f,
		z = -2.75f,
		pitch = 10,
		yaw = 20,
		headYaw = 30,
		mode = PositionMode.Teleport,
		onGround = true,
		otherRuntimeEntityId = 88,
		tick = 99
	};

	var decoded = RequireType<McbeMovePlayer>(RoundTrip(packet));

	Equal(packet.runtimeEntityId, decoded.runtimeEntityId);
	Equal(packet.x, decoded.x);
	Equal(packet.y, decoded.y);
	Equal(packet.z, decoded.z);
	Equal(packet.pitch, decoded.pitch);
	Equal(packet.yaw, decoded.yaw);
	Equal(packet.headYaw, decoded.headYaw);
	Equal(packet.mode, decoded.mode);
	Equal(packet.onGround, decoded.onGround);
	Equal(packet.otherRuntimeEntityId, decoded.otherRuntimeEntityId);
	Equal(packet.tick, decoded.tick);
	SequenceEqual(packet.Encode(), decoded.Encode());
}

static void TestPacketCompressionRoundTrip()
{
	var packets = new List<Packet>
	{
		new McbePlayStatus { status = PlayStatus.LoginSuccess },
		new McbeAnimate
		{
			actionId = 2,
			runtimeEntityId = 12,
			Data = 0.5f,
			swingSource = new Optional<string>()
		}
	};

	var compressed = ZLibHelper.Compress(packets, CompressionAlgorithm.None, enable: false);
	var decompressed = ZLibHelper.Decompress(compressed, CompressionAlgorithm.None);

	Equal(packets.Count, decompressed.Count);
	RequireType<McbePlayStatus>(decompressed[0]);
	RequireType<McbeAnimate>(decompressed[1]);
}

static void TestEncodeReflectsFieldMutation()
{
	var packet = new McbePlayStatus { status = PlayStatus.LoginSuccess };
	var first = packet.Encode();

	packet.status = PlayStatus.LoginFailedClientOld;
	var second = packet.Encode();

	if (first.SequenceEqual(second))
	{
		throw new InvalidOperationException("Encode returned stale bytes after packet mutation.");
	}
}

static void TestTruncatedStringThrows()
{
    var packet = new StringReadPacket();
    ReadOnlyMemory<byte> truncatedPayload = new byte[] { 0x01 };
    Throws<EndOfStreamException>(() => packet.Decode(truncatedPayload));
}

static void TestPacketFactoryKnownDispatch()
{
    var login = RequireType<McbeLogin>(PacketFactory.translatePacket(0x01, new McbeLogin
    {
        protocolVersion = ProtocolInfo.McProtocol,
        payload = []
    }.Encode()));

    Equal(0x01, login.Id);

    var animate = RequireType<McbeAnimate>(PacketFactory.translatePacket(0x2c, new McbeAnimate
    {
        actionId = 1,
        runtimeEntityId = 42,
        Data = 0,
        swingSource = new Optional<string>()
    }.Encode()));

    Equal(0x2c, animate.Id);
}

static Packet RoundTrip(Packet packet)
{
    return PacketFactory.translatePacket(packet.Id, packet.Encode());
}

static T RequireType<T>(Packet packet) where T : Packet
{
    if (packet is T typed)
    {
        return typed;
    }

    throw new InvalidOperationException($"Expected {typeof(T).Name}, got {packet.GetType().Name}.");
}

static void Equal<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected {expected}, got {actual}.");
    }
}

static void SequenceEqual(byte[] expected, byte[] actual)
{
    if (!expected.SequenceEqual(actual))
    {
        throw new InvalidOperationException("Byte sequences differ.");
    }
}

static void Throws<TException>(Action action) where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}

internal sealed class StringReadPacket : Packet
{
    public StringReadPacket()
    {
        Id = 1;
        IsMcbe = true;
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        ReadString();
    }
}
