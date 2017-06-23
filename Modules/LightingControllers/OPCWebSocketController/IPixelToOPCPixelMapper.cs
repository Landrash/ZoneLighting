namespace OPCWebSocketController
{
    public interface IPixelToOPCPixelMapper
    {
        int GetOPCPixelIndex(int pixelIndex);
	    byte GetOPCPixelChannel(int pixelIndex);
    }
}