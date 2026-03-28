using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol.Minecraft.Camera
{
	public enum EasingTypeEnum : int
	{
		 Linear = 0,
		 Spring = 1,
		 InQuad = 2,
		 OutQuad = 3,
		 InOutQuad = 4,
		 InCubic = 5,
		 OutCubic = 6,
		 InOutCubic = 7,
		 InQuart = 8,
		 OutQuart = 9,
		 InOutQuart = 10,
		 InQuint = 11,
		 OutQuint = 12,
		 InOutQuint = 13,
		 InSine = 14,
		 OutSine = 15,
		 InOutSine = 16,
		 InExpo = 17,
		 OutExpo = 18,
		 InOutExpo = 19,
		 InCirc = 20,
		 OutCirc = 21,
		 InOutCirc = 22,
		 InBack = 23,
		 OutBack = 24,
		 InOutBack = 25,
		 InElastic = 26,
		 OutElastic = 27,
		 InOutElastic = 28,
		 InBounce = 29,
		 OutBounce = 30,
		 InOutBounce = 31,
		 InverseLerp = 32,
	}
	public class EasingType
	{

		public static string EasingTypeToString(int easingType)
		{
			switch ((EasingTypeEnum)easingType)
			{
				case EasingTypeEnum.Linear:
					return "linear";
				case EasingTypeEnum.Spring:
					return "spring";
				case EasingTypeEnum.InQuad:
					return "in_quad";
				case EasingTypeEnum.OutQuad:
					return "out_quad";
				case EasingTypeEnum.InOutQuad:
					return "in_out_quad";
				case EasingTypeEnum.InCubic:
					return "in_cubic";
				case EasingTypeEnum.OutCubic:
					return "out_cubic";
				case EasingTypeEnum.InOutCubic:
					return "in_out_cubic";
				case EasingTypeEnum.InQuart:
					return "in_quart";
				case EasingTypeEnum.OutQuart:
					return "out_quart";
				case EasingTypeEnum.InOutQuart:
					return "in_out_quart";
				case EasingTypeEnum.InQuint:
					return "in_quint";
				case EasingTypeEnum.OutQuint:
					return "out_quint";
				case EasingTypeEnum.InOutQuint:
					return "in_out_quint";
				case EasingTypeEnum.InSine:
					return "in_sine";
				case EasingTypeEnum.OutSine:
					return "out_sine";
				case EasingTypeEnum.InOutSine:
					return "in_out_sine";
				case EasingTypeEnum.InExpo:
					return "in_expo";
				case EasingTypeEnum.OutExpo:
					return "out_expo";
				case EasingTypeEnum.InOutExpo:
					return "in_out_expo";
				case EasingTypeEnum.InCirc:
					return "in_circ";
				case EasingTypeEnum.OutCirc:
					return "out_circ";
				case EasingTypeEnum.InOutCirc:
					return "in_out_circ";
				case EasingTypeEnum.InBack:
					return "in_back";
				case EasingTypeEnum.OutBack:
					return "out_back";
				case EasingTypeEnum.InOutBack:
					return "in_out_back";
				case EasingTypeEnum.InElastic:
					return "in_elastic";
				case EasingTypeEnum.OutElastic:
					return "out_elastic";
				case EasingTypeEnum.InOutElastic:
					return "in_out_elastic";
				case EasingTypeEnum.InBounce:
					return "in_bounce";
				case EasingTypeEnum.OutBounce:
					return "out_bounce";
				case EasingTypeEnum.InOutBounce:
					return "in_out_bounce";
				case EasingTypeEnum.InverseLerp:
					return "inverse_lerp";
				default:
					throw new ArgumentException($"Unknown easing type: {easingType}");
			}
		}

		public static EasingTypeEnum EasingTypeFromString(string easingType)
		{
			switch (easingType)
			{
				case "linear":
					return EasingTypeEnum.Linear;
				case "spring":
					return EasingTypeEnum.Spring;
				case "in_quad":
					return EasingTypeEnum.InQuad;
				case "out_quad":
					return EasingTypeEnum.OutQuad;
				case "in_out_quad":
					return EasingTypeEnum.InOutQuad;
				case "in_cubic":
					return EasingTypeEnum.InCubic;
				case "out_cubic":
					return EasingTypeEnum.OutCubic;
				case "in_out_cubic":
					return EasingTypeEnum.InOutCubic;
				case "in_quart":
					return EasingTypeEnum.InQuart;
				case "out_quart":
					return EasingTypeEnum.OutQuart;
				case "in_out_quart":
					return EasingTypeEnum.InOutQuart;
				case "in_quint":
					return EasingTypeEnum.InQuint;
				case "out_quint":
					return EasingTypeEnum.OutQuint;
				case "in_out_quint":
					return EasingTypeEnum.InOutQuint;
				case "in_sine":
					return EasingTypeEnum.InSine;
				case "out_sine":
					return EasingTypeEnum.OutSine;
				case "in_out_sine":
					return EasingTypeEnum.InOutSine;
				case "in_expo":
					return EasingTypeEnum.InExpo;
				case "out_expo":
					return EasingTypeEnum.OutExpo;
				case "in_out_expo":
					return EasingTypeEnum.InOutExpo;
				case "in_circ":
					return EasingTypeEnum.InCirc;
				case "out_circ":
					return EasingTypeEnum.OutCirc;
				case "in_out_circ":
					return EasingTypeEnum.InOutCirc;
				case "in_back":
					return EasingTypeEnum.InBack;
				case "out_back":
					return EasingTypeEnum.OutBack;
				case "in_out_back":
					return EasingTypeEnum.InOutBack;
				case "in_elastic":
					return EasingTypeEnum.InElastic;
				case "out_elastic":
					return EasingTypeEnum.OutElastic;
				case "in_out_elastic":
					return EasingTypeEnum.InOutElastic;
				case "in_bounce":
					return EasingTypeEnum.InBounce;
				case "out_bounce":
					return EasingTypeEnum.OutBounce;
				case "in_out_bounce":
					return EasingTypeEnum.InOutBounce;
				case "inverse_lerp":
					return EasingTypeEnum.InverseLerp;
				default:
					throw new FormatException($"Unknown easing type string: {easingType}");
			}
		}
	}

}
