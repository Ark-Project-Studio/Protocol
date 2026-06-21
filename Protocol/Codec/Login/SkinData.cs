using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Protocol.Minecraft.Login
{
	public class SkinDatas
	{
		[JsonPropertyName("AnimatedImageData")]
		public List<AnimatedImageData> AnimatedImageData { get; set; }

		[JsonPropertyName("ArmSize")]
		public string ArmSize { get; set; }

		[JsonPropertyName("CapeData")]
		public string CapeData { get; set; }

		[JsonPropertyName("CapeId")]
		public string CapeId { get; set; }

		[JsonPropertyName("CapeImageHeight")]
		public int CapeImageHeight { get; set; }

		[JsonPropertyName("CapeImageWidth")]
		public int CapeImageWidth { get; set; }

		[JsonPropertyName("CapeOnClassicSkin")]
		public bool CapeOnClassicSkin { get; set; }

		[JsonPropertyName("ClientRandomId")]
		public long ClientRandomId { get; set; }

		[JsonPropertyName("CompatibleWithClientSideChunkGen")]
		public bool CompatibleWithClientSideChunkGen { get; set; }

		[JsonPropertyName("CurrentInputMode")]
		public int CurrentInputMode { get; set; }

		[JsonPropertyName("DefaultInputMode")]
		public int DefaultInputMode { get; set; }

		[JsonPropertyName("DeviceId")]
		public string DeviceId { get; set; }

		[JsonPropertyName("DeviceModel")]
		public string DeviceModel { get; set; }

		[JsonPropertyName("DeviceOS")]
		public int DeviceOS { get; set; }

		[JsonPropertyName("GameVersion")]
		public string GameVersion { get; set; }

		[JsonPropertyName("GraphicsMode")]
		public int GraphicsMode { get; set; }

		[JsonPropertyName("GuiScale")]
		public int GuiScale { get; set; }

		[JsonPropertyName("IsEditorMode")]
		public bool IsEditorMode { get; set; }

		[JsonPropertyName("LanguageCode")]
		public string LanguageCode { get; set; }

		[JsonPropertyName("MaxViewDistance")]
		public int MaxViewDistance { get; set; }

		[JsonPropertyName("MemoryTier")]
		public int MemoryTier { get; set; }

		[JsonPropertyName("OverrideSkin")]
		public bool OverrideSkin { get; set; }

		[JsonPropertyName("PersonaPieces")]
		public List<PersonaPiece> PersonaPieces { get; set; }

		[JsonPropertyName("PersonaSkin")]
		public bool PersonaSkin { get; set; }

		[JsonPropertyName("PieceTintColors")]
		public List<PieceTintColor> PieceTintColors { get; set; }

		[JsonPropertyName("PlatformOfflineId")]
		public string PlatformOfflineId { get; set; }

		[JsonPropertyName("PlatformOnlineId")]
		public string PlatformOnlineId { get; set; }

		[JsonPropertyName("PlatformType")]
		public int PlatformType { get; set; }

		[JsonPropertyName("PremiumSkin")]
		public bool PremiumSkin { get; set; }

		[JsonPropertyName("SelfSignedId")]
		public string SelfSignedId { get; set; }

		[JsonPropertyName("ServerAddress")]
		public string ServerAddress { get; set; }

		[JsonPropertyName("SkinAnimationData")]
		public string SkinAnimationData { get; set; }

		[JsonPropertyName("SkinColor")]
		public string SkinColor { get; set; }

		[JsonPropertyName("SkinData")]
		public string SkinData { get; set; }

		[JsonPropertyName("SkinGeometryData")]
		public string SkinGeometryData { get; set; }

		[JsonPropertyName("SkinGeometryDataEngineVersion")]
		public string SkinGeometryDataEngineVersion { get; set; }

		[JsonPropertyName("SkinId")]
		public string SkinId { get; set; }

		[JsonPropertyName("SkinImageHeight")]
		public int SkinImageHeight { get; set; }

		[JsonPropertyName("SkinImageWidth")]
		public int SkinImageWidth { get; set; }

		[JsonPropertyName("SkinResourcePatch")]
		public string SkinResourcePatch { get; set; }

		[JsonPropertyName("ThirdPartyName")]
		public string ThirdPartyName { get; set; }

		[JsonPropertyName("TrustedSkin")]
		public bool TrustedSkin { get; set; }

		[JsonPropertyName("UIProfile")]
		public int UIProfile { get; set; }
	}

	/// <summary>
	/// 动画图片数据
	/// </summary>
	public class AnimatedImageData
	{
		[JsonPropertyName("AnimationExpression")]
		public int AnimationExpression { get; set; }

		[JsonPropertyName("Frames")]
		public double Frames { get; set; }

		[JsonPropertyName("Image")]
		public string Image { get; set; }

		[JsonPropertyName("ImageHeight")]
		public int ImageHeight { get; set; }

		[JsonPropertyName("ImageWidth")]
		public int ImageWidth { get; set; }

		[JsonPropertyName("Type")]
		public int Type { get; set; }
	}

	/// <summary>
	/// Persona 部件定义
	/// </summary>
	public class PersonaPiece
	{
		[JsonPropertyName("IsDefault")]
		public bool IsDefault { get; set; }

		[JsonPropertyName("PackId")]
		public string PackId { get; set; }

		[JsonPropertyName("PieceId")]
		public string PieceId { get; set; }

		[JsonPropertyName("PieceType")]
		public string PieceType { get; set; }

		[JsonPropertyName("ProductId")]
		public string ProductId { get; set; }
	}

	/// <summary>
	/// 部件着色颜色
	/// </summary>
	public class PieceTintColor
	{
		[JsonPropertyName("Colors")]
		public List<string> Colors { get; set; }

		[JsonPropertyName("PieceType")]
		public string PieceType { get; set; }
	}
}
