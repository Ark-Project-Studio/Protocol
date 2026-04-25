using System.Net;
using Protocol.Minecraft;
using Protocol.Minecraft.Skins;
using Protocol.Network.MinecraftPacket;
using Protocol.Network.RaknetPacket;
using Protocol.Utils.Crypto;
using Protocol.Utils;

namespace Protocol.Minecraft;

public class Player : Entity
{
	private Dictionary<ChunkCoordinates, McbeWrapper> _chunksUsed = new();
	private ChunkCoordinates _currentChunkPosition;

	internal IInventory _openInventory = null;


	public Player(string entityTypeId, Level level) : base(entityTypeId, level)
	{
	}

	public IPEndPoint EndPoint { get; private set; }

	public PlayerLocation SpawnPosition { get; set; }
	public bool IsSleeping { get; set; } = false;

	public int MaxViewDistance { get; set; } = 22;
	public int MoveRenderDistance { get; set; } = 1;

	public GameMode GameMode { get; set; }
	public bool UseCreativeInventory { get; set; } = true;
	public bool IsConnected { get; set; }
	public CertificateData CertificateData { get; set; }
	public string Username { get; set; }
	public string DisplayName { get; set; }
	public long ClientId { get; set; }
	public long CurrentTick { get; set; }
	public UUID ClientUuid { get; set; }
	public string ServerAddress { get; set; }
	public PlayerInfo PlayerInfo { get; set; }

	public Skin Skin { get; set; }

	public float MovementSpeed { get; set; } = 0.1f;
	public bool IsFalling { get; set; }
	public bool IsFlyingHorizontally { get; set; }
	public Entity LastAttackTarget { get; set; }
}