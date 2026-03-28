using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Protocol.Minecraft.Graphic;

namespace Protocol.Network.MinecraftPacket
{
    public class McbeGraphicsOverrideParameter : Packet
    {
        public ParameterKeyframeValue[] Values { get; set; }
        public float FloatValue { get; set; }
        public System.Numerics.Vector3 Vec3Value { get; set; }
        public string BiomeIdentifier { get; set; }
        public byte ParameterType { get; set; }
        public bool Reset { get; set; }

        public McbeGraphicsOverrideParameter()
        {
            IsMcbe = true;
            Id = 331;
        }

        protected override void DecodePacket()
        {
            base.DecodePacket();
            Values = ReadSlice(ReadParameterKeyframeValue);
            FloatValue = ReadFloat();
            Vec3Value = ReadVector3();
            BiomeIdentifier = ReadString();
            ParameterType = ReadByte();
            Reset = ReadBool();
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();
            WriteSlice(Values, Write);
            Write(FloatValue);
            Write(Vec3Value);
            Write(BiomeIdentifier);
            Write(ParameterType);
            Write(Reset);
        }
    }
}
