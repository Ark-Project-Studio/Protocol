using Protocol.Network;
using System.Numerics;

namespace Protocol.Codec.Packets
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
		Cylinder = 6,
		Pyramid = 7,
		Ellipsoid = 8,
		Cone = 9,
		NumShapeTypes = 10
	}


	public class PrimitiveShapes
	{

		public ulong NetworkID { get; set; }


		public Optional<PrimitiveShapesType> Type { get; set; }


		public Optional<Vector3> Location { get; set; }


		public Optional<float> Scale { get; set; }


		public Optional<Vector3> Rotation { get; set; }


		public Optional<float> TotalTimeLeft { get; set; }


		public Optional<int> Colour { get; set; }


		public Optional<string> Text { get; set; }


		public bool UseRotation { get; set; }


		public Optional<int> BackgroundColor { get; set; }


		public bool DepthTest { get; set; }


		public bool ShowBackface { get; set; }


		public bool ShowTextBackface { get; set; }


		public Optional<Vector3> BoxBound { get; set; }


		public Optional<Vector3> LineEndLocation { get; set; }


		public Optional<float> ArrowHeadLength { get; set; }


		public Optional<float> ArrowHeadRadius { get; set; }


		public Optional<byte> Segments { get; set; }
		
		public Optional<float> MaxRenderDistance { get; set; }
		
		public Optional<int> DimensionId { get; set; }

		public Optional<long> AttachedToEntityId { get; set; }

		public Optional<Vector2> CylinderRadiusX { get; set; }
		public Optional<Vector2> CylinderRadiusZ { get; set; }
		public float CylinderHeight { get; set; }
		public byte CylinderNumSegments { get; set; }

		public float PyramidWidth { get; set; }
		public Optional<float> PyramidDepth { get; set; }
		public float PyramidHeight { get; set; }

		public Optional<Vector3> EllipsoidRadii { get; set; }
		public byte EllipsoidSegmentsPerAxis { get; set; }

		public Optional<Vector2> ConeRadii { get; set; }
		public float ConeHeight { get; set; }
		public byte ConeNumSegments { get; set; }
	}


	public class McbePrimitiveShapes : Packet
	{
		public McbePrimitiveShapes()
		{
			Id = 328;
			IsMcbe = true;
		}

		public List<PrimitiveShapes> Shapes { get; set; } = new();

		protected override void EncodePacket()
		{
			base.EncodePacket();
			WriteUnsignedVarInt((uint)Shapes.Count);
			foreach (var shape in Shapes)
			{
				WriteUnsignedVarLong(shape.NetworkID);

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

				Write(shape.MaxRenderDistance.HasValue);
				if (shape.MaxRenderDistance.HasValue)
				{
					Write(shape.MaxRenderDistance.Value);
				}

				Write(shape.Colour.HasValue);
				if (shape.Colour.HasValue)


					Write(shape.Colour.Value);

				Write(shape.DimensionId.HasValue);
				if (shape.DimensionId.HasValue) WriteSignedVarInt(shape.DimensionId.Value);
			Write(shape.AttachedToEntityId.HasValue);
			if (shape.AttachedToEntityId.HasValue) WriteSignedVarLong(shape.AttachedToEntityId.Value);

			WriteUnsignedVarInt(GetShapeVariantIndex(shape));

			switch (GetShapeVariantIndex(shape))
			{
				case 1:
					Write(shape.LineEndLocation.HasValue);
					if (shape.LineEndLocation.HasValue) Write(shape.LineEndLocation.Value);
					Write(shape.ArrowHeadLength.HasValue);
					if (shape.ArrowHeadLength.HasValue) Write(shape.ArrowHeadLength.Value);
					Write(shape.ArrowHeadRadius.HasValue);
					if (shape.ArrowHeadRadius.HasValue) Write(shape.ArrowHeadRadius.Value);
					Write(shape.Segments.HasValue);
					if (shape.Segments.HasValue) Write(shape.Segments.Value);
					break;
				case 2:
					Write(shape.Text.HasValue ? shape.Text.Value : string.Empty);
					Write(shape.UseRotation);
					Write(shape.BackgroundColor.HasValue);
					if (shape.BackgroundColor.HasValue) Write(shape.BackgroundColor.Value);
					Write(shape.DepthTest);
					Write(shape.ShowBackface);
					Write(shape.ShowTextBackface);
					break;
				case 3:
					Write(shape.BoxBound.HasValue ? shape.BoxBound.Value : Vector3.Zero);
					break;
				case 4:
					Write(shape.LineEndLocation.HasValue ? shape.LineEndLocation.Value : Vector3.Zero);
					break;
				case 5:
					Write(shape.Segments.HasValue ? shape.Segments.Value : (byte)0);
					break;
				case 6:
					Write(shape.CylinderRadiusX.HasValue ? shape.CylinderRadiusX.Value : Vector2.Zero);
					Write(shape.CylinderRadiusZ.HasValue ? shape.CylinderRadiusZ.Value : Vector2.Zero);
					Write(shape.CylinderHeight);
					Write(shape.CylinderNumSegments);
					break;
				case 7:
					Write(shape.PyramidWidth);
					Write(shape.PyramidDepth.HasValue);
					if (shape.PyramidDepth.HasValue) Write(shape.PyramidDepth.Value);
					Write(shape.PyramidHeight);
					break;
				case 8:
					Write(shape.EllipsoidRadii.HasValue ? shape.EllipsoidRadii.Value : Vector3.Zero);
					Write(shape.EllipsoidSegmentsPerAxis);
					break;
				case 9:
					Write(shape.ConeRadii.HasValue ? shape.ConeRadii.Value : Vector2.Zero);
					Write(shape.ConeHeight);
					Write(shape.ConeNumSegments);
					break;
			}
			}
		}

		protected override void DecodePacket()
		{
			base.DecodePacket();
			var count = ReadUnsignedVarInt();
			Shapes = new List<PrimitiveShapes>();
			for (uint i = 0; i < count; i++)
			{
				var shape = new PrimitiveShapes();


				shape.NetworkID = ReadUnsignedVarLong();


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
				var maxRenderDistance = ReadBool();
				if (maxRenderDistance)
				{
					shape.MaxRenderDistance = new Optional<float>(ReadFloat());
				}

				var hasColour = ReadBool();
				if (hasColour)


					shape.Colour = new Optional<int>(ReadInt());

				var dimsonid = ReadBool();
				if (dimsonid)
				{
					shape.DimensionId = new Optional<int>(ReadSignedVarInt());
				}
			var hasATEid = ReadBool();
			if (hasATEid) shape.AttachedToEntityId = new Optional<long>(ReadSignedVarLong());

			switch (ReadUnsignedVarInt())
			{
				case 1:
					var hasArrowEndLocation = ReadBool();
					if (hasArrowEndLocation) shape.LineEndLocation = new Optional<Vector3>(ReadVector3());
					var hasArrowHeadLength = ReadBool();
					if (hasArrowHeadLength) shape.ArrowHeadLength = new Optional<float>(ReadFloat());
					var hasArrowHeadRadius = ReadBool();
					if (hasArrowHeadRadius) shape.ArrowHeadRadius = new Optional<float>(ReadFloat());
					var hasArrowSegments = ReadBool();
					if (hasArrowSegments) shape.Segments = new Optional<byte>(ReadByte());
					break;
				case 2:
					shape.Text = new Optional<string>(ReadString());
					shape.UseRotation = ReadBool();
					var hasBackgroundColor = ReadBool();
					if (hasBackgroundColor) shape.BackgroundColor = new Optional<int>(ReadInt());
					shape.DepthTest = ReadBool();
					shape.ShowBackface = ReadBool();
					shape.ShowTextBackface = ReadBool();
					break;
				case 3:
					shape.BoxBound = new Optional<Vector3>(ReadVector3());
					break;
				case 4:
					shape.LineEndLocation = new Optional<Vector3>(ReadVector3());
					break;
				case 5:
					shape.Segments = new Optional<byte>(ReadByte());
					break;
				case 6:
					shape.CylinderRadiusX = new Optional<Vector2>(ReadVector2());
					shape.CylinderRadiusZ = new Optional<Vector2>(ReadVector2());
					shape.CylinderHeight = ReadFloat();
					shape.CylinderNumSegments = ReadByte();
					break;
				case 7:
					shape.PyramidWidth = ReadFloat();
					if (ReadBool()) shape.PyramidDepth = new Optional<float>(ReadFloat());
					shape.PyramidHeight = ReadFloat();
					break;
				case 8:
					shape.EllipsoidRadii = new Optional<Vector3>(ReadVector3());
					shape.EllipsoidSegmentsPerAxis = ReadByte();
					break;
				case 9:
					shape.ConeRadii = new Optional<Vector2>(ReadVector2());
					shape.ConeHeight = ReadFloat();
					shape.ConeNumSegments = ReadByte();
					break;
			}

				Shapes.Add(shape);
			}
		}

		private static uint GetShapeVariantIndex(PrimitiveShapes shape)
		{
			if (shape.ArrowHeadLength.HasValue || shape.ArrowHeadRadius.HasValue) return 1;
			if (shape.Text.HasValue) return 2;
			if (shape.BoxBound.HasValue) return 3;
			if (shape.LineEndLocation.HasValue && shape.Type.HasValue && shape.Type.Value == PrimitiveShapesType.Line) return 4;
			if (shape.Segments.HasValue) return 5;
			if (shape.CylinderRadiusX.HasValue || shape.CylinderRadiusZ.HasValue) return 6;
			if (shape.PyramidDepth.HasValue || shape.PyramidWidth != 0f || shape.PyramidHeight != 0f) return 7;
			if (shape.EllipsoidRadii.HasValue) return 8;
			if (shape.ConeRadii.HasValue) return 9;
			return 0;
		}
	}
}
