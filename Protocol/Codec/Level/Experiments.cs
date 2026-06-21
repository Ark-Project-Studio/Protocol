namespace Protocol.Minecraft.Level
{
	public class Experiments
	{
		public class Experiment
		{
			public string Name { get; }
			public bool Enabled { get; }

			public Experiment(string name, bool enabled)
			{
				Name = name;
				Enabled = enabled;
			}
		}
	}
}