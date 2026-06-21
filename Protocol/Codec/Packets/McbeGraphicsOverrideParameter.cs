using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Protocol.Minecraft.Graphic;
using Protocol.Network;

namespace Protocol.Codec.Packets
{
    public class McbeGraphicsOverrideParameter : Packet
    {
        public ParameterKeyframeValue[] Values { get; set; }
        public Optional<float> FloatValue { get; set; } = new();
        public Optional<Vector3> Vec3Value { get; set; } = new();
        public string BiomeIdentifier { get; set; }
        public Optional<string> PlayerIdentifier { get; set; } = new();
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
            if (ReadBool())
            {
                FloatValue = new Optional<float>(ReadFloat());
            }
            if (ReadBool())
            {
                Vec3Value = new Optional<Vector3>(ReadVector3());
            }
            BiomeIdentifier = ReadString();
            if (ReadBool())
            {
                PlayerIdentifier = new Optional<string>(ReadString());
            }
            ParameterType = ReadByte();
            Reset = ReadBool();
        }

        protected override void EncodePacket()
        {
            base.EncodePacket();
            WriteSlice(Values, Write);
            Write(FloatValue.HasValue);
            if (FloatValue.HasValue)
            {
                Write(FloatValue.Value);
            }
            Write(Vec3Value.HasValue);
            if (Vec3Value.HasValue)
            {
                Write(Vec3Value.Value);
            }
            Write(BiomeIdentifier);
            Write(PlayerIdentifier.HasValue);
            if (PlayerIdentifier.HasValue)
            {
                Write(PlayerIdentifier.Value);
            }
            Write(ParameterType);
            Write(Reset);
        }
    }
}
