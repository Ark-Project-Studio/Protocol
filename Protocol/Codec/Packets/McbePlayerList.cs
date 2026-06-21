using Protocol.Minecraft.Actor.Player;
using Protocol.Network;

namespace Protocol.Codec.Packets;
public class McbePlayerList : Packet
{
    public enum ActionType : byte
    {
        Add = 0,
        Remove = 1,
    }

    public ActionType action;
    public PlayerListEntry[] playerEntryList = [];

    public McbePlayerList()
    {
        Id = 0x3f;
        IsMcbe = true;
    }

    protected override void EncodePacket()
    {
        base.EncodePacket();

        Write((byte)action);
        WriteUnsignedVarInt((uint)(playerEntryList?.Length ?? 0));

        if (action == ActionType.Add)
        {
            foreach (PlayerListEntry entry in playerEntryList)
            {
                Write(entry);
            }

            foreach (PlayerListEntry entry in playerEntryList)
            {
                Write(entry.skinTrusted);
            }
        }
        else
        {
            foreach (PlayerListEntry entry in playerEntryList)
            {
                Write(entry.uuid);
            }
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        action = (ActionType)ReadByte();
        uint entryCount = ReadUnsignedVarInt();
        playerEntryList = new PlayerListEntry[entryCount];

        if (action == ActionType.Add)
        {
            for (int i = 0; i < entryCount; i++)
            {
                playerEntryList[i] = ReadPlayerListEntry();
            }

            foreach (PlayerListEntry entry in playerEntryList)
            {
                entry.skinTrusted = ReadBool();
            }
        }
        else
        {
            for (int i = 0; i < entryCount; i++)
            {
                playerEntryList[i] = new PlayerListEntry
                {
                    uuid = ReadUUID(),
                };
            }
        }
    }

   
}
