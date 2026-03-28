using Protocol.Minecraft.Transaction;

namespace Protocol.Network.MinecraftPacket;
public class McbeContainerRegistryCleanup : Packet
{
    public McbeContainerRegistryCleanup()
    {
        Id = 317;
        IsMcbe = true;
    }

    public FullContainerName[] RemovedContainers { get; set; } = new FullContainerName[0];

    protected override void EncodePacket()
    {
        base.EncodePacket();
        WriteUnsignedVarInt((uint)(RemovedContainers?.Length ?? 0));
        if (RemovedContainers != null)
            foreach (var containerName in RemovedContainers)
                Write(containerName);
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        var count = ReadUnsignedVarInt();
        RemovedContainers = new FullContainerName[count];
        for (var i = 0; i < count; i++)
            RemovedContainers[i] = readFullContainerName();
    }
}
