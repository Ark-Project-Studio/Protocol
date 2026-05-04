namespace Protocol.Minecraft.Level.Biome;

public class Biome
{
	public float AshDensity;
	public float BlueSporeDensity;
	public string DefinitionName;
	public float Depth;
	public float Downfall;
	public int Foliage;
	public int Grass;
	public int Id;
	public string Name;
	public float RedSporeDensity;
	public float Scale;
	public float Temperature;
	public int WaterColor;
	public float WhiteAshDensity;
}

public class BiomeUtils
{
	private const int FOREST_BIOME = 4;
	private const int SWAMPLAND_BIOME = 6;
	private const int FOREST_HILLS_BIOME = 18;
	private const int BIRCH_FOREST_BIOME = 27;
	private const int BIRCH_FOREST_HILLS_BIOME = 28;
	private const int ROOFED_FOREST_BIOME = 29;

	private const int MESA_BIOME = 37;
	private const int MESA_PLATEAU_F_BIOME = 38;
	private const int MESA_PLATEAU_BIOME = 39;

	public static Biome[] Biomes =
	{
		new()
		{
			Id = 0,
			Name = "Ocean",
			DefinitionName = "ocean",
			Temperature = 0.5f,
			Downfall = 0.5f
		},
		new()
		{
			Id = 1,
			Name = "Plains",
			DefinitionName = "plains",
			Temperature = 0.8f,
			Downfall = 0.4f
		},
		new()
		{
			Id = 2,
			Name = "Desert",
			DefinitionName = "desert",
			Temperature = 2.0f,
			Downfall = 0.0f
		}
	};


	private static readonly BiomeCorner[] grassCorners = new BiomeCorner[3]
	{
		new()
		{
			Red = 191,
			Green = 183,
			Blue = 85
		},
		new()
		{
			Red = 128,
			Green = 180,
			Blue = 151
		},
		new()
		{
			Red = 71,
			Green = 205,
			Blue = 51
		}
	};

	private static readonly BiomeCorner[] foliageCorners = new BiomeCorner[3]
	{
		new()
		{
			Red = 174,
			Green = 164,
			Blue = 42
		},
		new()
		{
			Red = 96,
			Green = 161,
			Blue = 123
		},
		new()
		{
			Red = 26,
			Green = 191,
			Blue = 0
		}
	};

	public static float Clamp(float value, float min, float max)
	{
		return value < min ? min : value > max ? max : value;
	}


	private int BiomeColor(float temperature, float rainfall, int elevation, BiomeCorner[] corners)
	{
		temperature = Clamp(temperature - elevation * 0.00166667f, 0.0f, 1.0f);

		rainfall = Clamp(rainfall, 0.0f, 1.0f);
		rainfall *= temperature;


		var lambda = new float[3];
		lambda[0] = temperature - rainfall;
		lambda[1] = 1.0f - temperature;
		lambda[2] = rainfall;

		float red = 0.0f, green = 0.0f, blue = 0.0f;
		for (var i = 0; i < 3; i++)
		{
			red += lambda[i] * corners[i].Red;
			green += lambda[i] * corners[i].Green;
			blue += lambda[i] * corners[i].Blue;
		}

		var r = (int)Clamp(red, 0.0f, 255.0f);
		var g = (int)Clamp(green, 0.0f, 255.0f);
		var b = (int)Clamp(blue, 0.0f, 255.0f);

		return (r << 16) | (g << 8) | b;
	}

	private int BiomeGrassColor(float temperature, float rainfall, int elevation)
	{
		return BiomeColor(temperature, rainfall, elevation, grassCorners);
	}

	private int BiomeFoliageColor(float temperature, float rainfall, int elevation)
	{
		return BiomeColor(temperature, rainfall, elevation, foliageCorners);
	}

	public void PrecomputeBiomeColors()
	{
		for (var biome = 0; biome < Biomes.Length; biome++)
		{
			Biomes[biome].Grass = ComputeBiomeColor(biome, 0, true);
			Biomes[biome].Foliage = ComputeBiomeColor(biome, 0, false);
		}
	}


	public int ComputeBiomeColor(int biome, int elevation, bool isGrass)
	{
		int color;

		switch (biome)
		{
			case SWAMPLAND_BIOME:


				return 0x6a7039;


			case ROOFED_FOREST_BIOME:
				if (isGrass)
				{
					color = BiomeGrassColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);


					return ((color & 0xfefefe) + 0x28340a) / 2;
				}

				return BiomeFoliageColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);

			case MESA_BIOME:
			case MESA_PLATEAU_F_BIOME:
			case MESA_PLATEAU_BIOME:

				return isGrass ? 0x90814d : 0x9e814d;

			default:
				return isGrass
					? BiomeGrassColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation)
					: BiomeFoliageColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);
		}
	}

	public static Biome GetBiome(int biomeId)
	{
		return Biomes.FirstOrDefault(biome => biome.Id == biomeId) ?? new Biome
		{
			Id = biomeId,
			Name = "" + biomeId
		};
	}

	public int BiomeSwampRiverColor(int color)
	{
		var r = (color >> 16) & 0xff;
		var g = (color >> 8) & 0xff;
		var b = color & 0xff;


		r = r * 0xE0 / 255;

		b = b * 0xAE / 255;
		color = (r << 16) | (g << 8) | b;

		return color;
	}

	private struct BiomeCorner
	{
		public int Red;
		public int Green;
		public int Blue;
	}
}