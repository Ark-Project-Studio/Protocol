using System.Numerics;

namespace Protocol.Network.MinecraftPacket
{
	public enum PrimitiveShapesType :
		byte
	{
		Line = 0,


		Box = 1,


		Sphere = 2,


		Circle = 3,


		Text = 4,


		Arrow = 5,


		NumShapeTypes = 6
	}


	public class DebugDrawerShape
	{
		public DebugDrawerShape()
		{
		}


		public ulong NetworkID { get; set; }


		public Optional<PrimitiveShapesType> Type { get; set; }


		public Optional<Vector3> Location { get; set; }


		public Optional<float> Scale { get; set; }


		public Optional<Vector3> Rotation { get; set; }


		public Optional<float> TotalTimeLeft { get; set; }


		public Optional<int> Colour { get; set; }


		public Optional<string> Text { get; set; }


		public Optional<Vector3> BoxBound { get; set; }


		public Optional<Vector3> LineEndLocation { get; set; }


		public Optional<float> ArrowHeadLength { get; set; }


		public Optional<float> ArrowHeadRadius { get; set; }


		public Optional<byte> Segments { get; set; }
	}


	public class McbeDebugDrawer : Packet
	{
		public McbeDebugDrawer()
		{
			Id = 328;
			IsMcbe = true;
		}

		public List<DebugDrawerShape> Shapes { get; set; } = new();

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteUnsignedVarInt((uint)Shapes.Count);
			foreach (var shape in Shapes)
			{
				Write(shape.NetworkID);

				Write(shape.Type.HasValue);
				if (shape.Type.HasValue) Write((byte)shape.Type.Value);


				Write(shape.Location.HasValue);
				if (shape.Location.HasValue) Write(shape.Location.Value);
				Write(shape.Scale.HasValue);
				if (shape.Scale.HasValue) Write(shape.Scale.Value);


				Write(shape.Rotation.HasValue);
				if (shape.Rotation.HasValue) Write(shape.Rotation.Value);


				Write(shape.TotalTimeLeft.HasValue);
				if (shape.TotalTimeLeft.HasValue) Write(shape.TotalTimeLeft.Value);


				Write(shape.Colour.HasValue);
				if (shape.Colour.HasValue)


					Write(shape.Colour.Value);


				Write(shape.Text.HasValue);
				if (shape.Text.HasValue) Write(shape.Text.Value);


				Write(shape.BoxBound.HasValue);
				if (shape.BoxBound.HasValue) Write(shape.BoxBound.Value);


				Write(shape.LineEndLocation.HasValue);
				if (shape.LineEndLocation.HasValue) Write(shape.LineEndLocation.Value);


				Write(shape.ArrowHeadLength.HasValue);
				if (shape.ArrowHeadLength.HasValue) Write(shape.ArrowHeadLength.Value);


				Write(shape.ArrowHeadRadius.HasValue);
				if (shape.ArrowHeadRadius.HasValue) Write(shape.ArrowHeadRadius.Value);


				Write(shape.Segments.HasValue);
				if (shape.Segments.HasValue) Write(shape.Segments.Value);
			}
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			var count = ReadUnsignedVarLong();
			Shapes = new List<DebugDrawerShape>();
			Shapes.Clear();
			for (ulong i = 0; i < count; i++)
			{
				var shape = new DebugDrawerShape();


				shape.NetworkID = ReadUlong();


				var hasType = ReadBool();
				if (hasType) shape.Type = new Optional<PrimitiveShapesType>((PrimitiveShapesType)ReadByte());


				var hasLocation = ReadBool();
				if (hasLocation)
					shape.Location = new Optional<Vector3>(ReadVector3());


				var hasScale = ReadBool();
				if (hasScale) shape.Scale = new Optional<float>(ReadFloat());


				var hasRotation = ReadBool();
				if (hasRotation)
					shape.Rotation = new Optional<Vector3>(ReadVector3());


				var hasTotalTimeLeft = ReadBool();
				if (hasTotalTimeLeft)
					shape.TotalTimeLeft = new Optional<float>(ReadFloat());


				var hasColour = ReadBool();
				if (hasColour)


					shape.Colour = new Optional<int>(ReadInt());


				var hasText = ReadBool();
				if (hasText) shape.Text = new Optional<string>(ReadString());


				var hasBoxBound = ReadBool();
				if (hasBoxBound)
					shape.BoxBound = new Optional<Vector3>(ReadVector3());


				var hasLineEndLocation = ReadBool();
				if (hasLineEndLocation)
					shape.LineEndLocation = new Optional<Vector3>(ReadVector3());


				var hasArrowHeadLength = ReadBool();
				if (hasArrowHeadLength)
					shape.ArrowHeadLength = new Optional<float>(ReadFloat());


				var hasArrowHeadRadius = ReadBool();
				if (hasArrowHeadRadius)
					shape.ArrowHeadRadius = new Optional<float>(ReadFloat());


				var hasSegments = ReadBool();
				if (hasSegments) shape.Segments = new Optional<byte>(ReadByte());
			}
		}
	}
}
