using System.Numerics;

namespace Protocol.Network.MinecraftPacket;
public enum ClientBoundDebugRendererType : byte
{
    Invalid = 0,
    ClearDebugMarkers = 1,
    AddDebugMarkerCube = 2
}

public class McbeClientBoundDebugRenderer : Packet
{
    public McbeClientBoundDebugRenderer()
    {
        Id = 164;
        IsMcbe = true;
    }

    public ClientBoundDebugRendererType Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public Vector3 Position { get; set; }
    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }
    public float Alpha { get; set; }
    public ulong Duration { get; set; }

    protected override void EncodePacket()
    {
        base.EncodePacket();
        Write((byte)Type);
        if (Type == ClientBoundDebugRendererType.AddDebugMarkerCube)
        {
            Write(Text);
            Write(Position);
            Write(Red);
            Write(Green);
            Write(Blue);
            Write(Alpha);
            Write(Duration);
        }
    }

    protected override void DecodePacket()
    {
        base.DecodePacket();
        Type = (ClientBoundDebugRendererType)ReadByte();
        if (Type == ClientBoundDebugRendererType.AddDebugMarkerCube)
        {
            Text = ReadString();
            Position = ReadVector3();
            Red = ReadFloat();
            Green = ReadFloat();
            Blue = ReadFloat();
            Alpha = ReadFloat();
            Duration = ReadUlong();
        }
    }
}
