namespace Protocol.Minecraft
{
	public abstract class PlayerRecords
	{
		public Player[] Players { get; set; } = [];
	}

	public class PlayerAddRecords : PlayerRecords
	{
	}

	public class PlayerRemoveRecords : PlayerRecords
	{
	}
}