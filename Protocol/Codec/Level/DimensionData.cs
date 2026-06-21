namespace Protocol.Minecraft.Level
{
	public class DimensionData
	{
		public string Identifier { get; set; } = string.Empty;
		public int MaxHeight { get; set; }
		public int MinHeight { get; set; }
		public int Generator { get; set; }
		public int Dimension { get; set; }
	}
}