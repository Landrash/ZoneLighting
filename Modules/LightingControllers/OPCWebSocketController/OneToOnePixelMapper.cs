using System;
using ZoneLighting.MEF;

namespace OPCWebSocketController
{
    public class OneToOnePixelMapper : IPixelToOPCPixelMapper
    {
        public int GetOPCPixelIndex(int pixelIndex)
        {
            return pixelIndex;
        }

        public byte GetOPCPixelChannel(int pixelIndex)
        {
            //this is where per-pixel mapping of the channel would happen
            throw new NotImplementedException();
        }

    }
}