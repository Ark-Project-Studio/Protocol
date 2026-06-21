namespace Protocol.Minecraft.Level.Scoreboard;

public abstract class ScoreEntry
{
	public long Id { get; set; }
	public string ObjectiveName { get; set; }
	public uint Score { get; set; }
}

public class ScoreEntryRemove : ScoreEntry
{
}

public abstract class ScoreEntryChange : ScoreEntry
{
}

public class ScoreEntryChangePlayer : ScoreEntryChange
{
	public long EntityId { get; set; }
}

public class ScoreEntryChangeEntity : ScoreEntryChange
{
	public long EntityId { get; set; }
}

public class ScoreEntryChangeFakePlayer : ScoreEntryChange
{
	public string CustomName { get; set; }
}

public abstract class ScoreboardIdentityEntry
{
	public long Id { get; set; }
}

public class ScoreboardRegisterIdentityEntry : ScoreboardIdentityEntry
{
	public long EntityId { get; set; }
}

public class ScoreboardClearIdentityEntry : ScoreboardIdentityEntry
{
}