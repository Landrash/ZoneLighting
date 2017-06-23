using System;
using System.Linq;

namespace OPCWebSocketController
{
    public class StaticPixelMapper : IPixelToOPCPixelMapper
    {
        public PixelMap PixelMap { get; }

        public int GetOPCPixelIndex(int pixelIndex)
        {
	        if (PixelMap.LogicalIndices.Contains(pixelIndex))
                return PixelMap.PhysicalIndices[PixelMap.LogicalIndices.First(x => x == pixelIndex)];
	        throw new ArgumentException("Passed in pixel index is not contained in the logical indices.");
        }

        public byte GetOPCPixelChannel(int pixelIndex)
        {
	        if (PixelMap.LogicalIndices.Contains(pixelIndex))
		        return PixelMap.Channels[PixelMap.LogicalIndices.First(x => x == pixelIndex)];
	        throw new ArgumentException("Passed in pixel index is not contained in the logical indices.");
        }

        public StaticPixelMapper(PixelMap pixelMap)
        {
	        PixelMap = pixelMap;
        }

    }
}