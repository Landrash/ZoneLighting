using ZoneLighting.Communication;

namespace OPCWebSocketController
{
	public interface IOPCPixelContainer : ILightingControllerPixel
	{
		OPCPixel OPCPixel { get; set; }
	}
}