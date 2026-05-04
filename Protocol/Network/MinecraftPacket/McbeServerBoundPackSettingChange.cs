using System;
using System.Collections.Generic;
using System.Text;
using Protocol.Minecraft.Level.ResourcePacks;
using Protocol.Utils;

namespace Protocol.Network.MinecraftPacket
{
    public class McbeServerBoundPackSettingChange : Packet
    {
        public UUID PackID;
        public PackSetting Setting;
        public byte PackSettingDataType;
        public bool BoolValue;
        public float FloatValue;
        public string StringValue;
        public McbeServerBoundPackSettingChange()
        {
            Id = 329;
            IsMcbe = true;
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();
            Write(PackID);
            if (Setting != null)
            {
                Write(Setting.Value switch
                {
                    float => (byte)PackSettingType.Float,
                    bool => (byte)PackSettingType.Bool,
                    string => (byte)PackSettingType.String,
                    _ => PackSettingDataType
                });
                Write(Setting.Name ?? string.Empty);
            }
            else
            {
                Write(PackSettingDataType);
                Write(string.Empty);
            }

            Write(BoolValue);
            Write(FloatValue);
            Write(StringValue ?? string.Empty);
        }

        protected override void DecodePacket()
        {
            base.DecodePacket();
            PackID = ReadUUID();
            PackSettingDataType = ReadByte();
            Setting = new PackSetting { Name = ReadString() };
            BoolValue = ReadBool();
            FloatValue = ReadFloat();
            StringValue = ReadString();

            Setting.Value = (PackSettingType)PackSettingDataType switch
            {
                PackSettingType.Float => FloatValue,
                PackSettingType.Bool => BoolValue,
                PackSettingType.String => StringValue,
                _ => null
            };
        }
    }
}
