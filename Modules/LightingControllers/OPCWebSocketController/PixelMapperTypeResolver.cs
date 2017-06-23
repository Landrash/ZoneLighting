using System;

namespace OPCWebSocketController
{
	public class PixelMapperTypeResolver
	{
		public Type Resolve(PixelMapperType type)
		{
			switch (type)
			{
				case PixelMapperType.Static:
					return typeof(StaticPixelMapper);
				case PixelMapperType.OneToOne:
					return typeof(OneToOnePixelMapper);
				default:
					throw new Exception("Unrecognized PixelMapperType");
			}
		}
	}
}