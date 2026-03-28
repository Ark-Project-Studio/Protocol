using System.Text.Json.Serialization;

namespace Protocol.Minecraft.Geometry;

public class GeometryModel : ICloneable
{
	[JsonPropertyName("format_version")] public string FormatVersion { get; set; } = "1.12.0";

	[JsonPropertyName("minecraft:geometry")]
	public List<Geometry> Geometry { get; set; } = new();

	public object Clone()
	{
		var model = (GeometryModel)MemberwiseClone();
		model.Geometry = new List<Geometry>();
		foreach (var records in Geometry) model.Geometry.Add((Geometry)records.Clone());

		return model;
	}

	public Geometry FindGeometry(string geometryName, bool matchPartial = true)
	{
		var fullName = Geometry.FirstOrDefault(g => matchPartial
				? g.Description.Identifier.StartsWith(geometryName, StringComparison.InvariantCultureIgnoreCase)
				: g.Description.Identifier.Equals(geometryName, StringComparison.InvariantCultureIgnoreCase))
			?.Description.Identifier;

		if (fullName == null) return null;

		var geometry = Geometry.First(g => g.Description.Identifier == fullName);
		geometry.Name = fullName;


		return geometry;
	}

	public Geometry CollapseToDerived(Geometry derived)
	{
		if (derived == null) throw new ArgumentNullException(nameof(derived));

		return derived;
	}
}