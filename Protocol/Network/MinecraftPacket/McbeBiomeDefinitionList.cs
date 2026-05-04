using Protocol.Minecraft.Level.Biome;

namespace Protocol.Network.MinecraftPacket;
public class McbeBiomeDefinitionList : Packet
{
    public string[] biomeNames = new string[0];
    public BiomeDefinition[] biomes;
    public McbeBiomeDefinitionList()
    {
        Id = 0x7a;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteVarInt(biomes.Length);
        foreach (var biome in biomes)
            Write(biome);
        WriteUnsignedVarInt((uint)biomeNames.Length);
        foreach (var biomeName in biomeNames)
            Write(biomeName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var biomeCount = ReadUnsignedVarInt();
        biomes = new BiomeDefinition[biomeCount];
        for (var i = 0; i < biomeCount; i++)
            biomes[i] = ReadBiomeDefinition();
        var biomeNameCount = ReadUnsignedVarInt();
        biomeNames = new string[biomeNameCount];
        for (var i = 0; i < biomeNameCount; i++)
            biomeNames[i] = ReadString();
    }
}
