using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket
{
    public class McbeServerBoundPackSettingChange : Packet
    {
        public UUID PackID;
        public PackSetting Setting;
        public McbeServerBoundPackSettingChange()
        {
            Id = 329;
            IsMcbe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();
            Write(PackID);
            WritePackSetting(Setting);
        }

        protected override void DecodePacket()
        {
            base.DecodePacket();
            PackID = ReadUUID();
            Setting = ReadPackSetting();
        }
    }
}
