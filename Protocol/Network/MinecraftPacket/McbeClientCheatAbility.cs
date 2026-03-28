using Protocol.Minecraft;

namespace Protocol.Network.MinecraftPacket;
public class McbeClientCheatAbility : Packet
{
    public McbeClientCheatAbility()
    {
        Id = 197;
        IsMcbe = true;
    }

    public AbilityData AbilityData { get; set; } = new();

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteAbilityData(AbilityData);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        AbilityData = ReadAbilityData();
    }

#region 琛ュ叏鐨勬柟娉?(鍥犱负 methods.txt 涓病鏈?AbilityData 鍜?AbilityLayer 鐨勭洿鎺ヨ鍐?
    private void WriteAbilityData(AbilityData data)
    {
        if (data == null)
        {
            Write(0L);
            Write((byte)0);
            Write((byte)0);
            WriteUnsignedVarInt(0);
            return;
        }

        Write(data.EntityUniqueID);
        Write(data.PlayerPermissions);
        Write(data.CommandPermissions);
        WriteUnsignedVarInt((uint)(data.Layers?.Length ?? 0));
        if (data.Layers != null)
            foreach (var layer in data.Layers)
                Write(layer);
    }

    private AbilityData ReadAbilityData()
    {
        var entityUniqueID = ReadLong();
        var playerPermissions = ReadByte();
        var commandPermissions = ReadByte();
        var layersCount = ReadUnsignedVarInt();
        var layers = new AbilityLayer[layersCount];
        for (var i = 0; i < layersCount; i++)
            layers[i] = ReadAbilityLayer();
        return new AbilityData(entityUniqueID, playerPermissions, commandPermissions, layers);
    }
#endregion
}
