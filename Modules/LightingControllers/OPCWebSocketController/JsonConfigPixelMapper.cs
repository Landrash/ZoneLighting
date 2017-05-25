using System;
using System.Linq;
using ZoneLighting.MEF;

namespace OPCWebSocketController
{
	public class JsonConfigPixelMapper : IPixelToOPCPixelMapper
    {
		public PixelMap PixelMap { get; }

		public int GetOPCPixelIndex(int pixelIndex)
		{
			if (PixelMap.LogicalIndices.Contains(pixelIndex))
				return PixelMap.PhysicalIndices[PixelMap.LogicalIndices.First(x => x == pixelIndex)];
			else
			{
				throw new ArgumentException("Passed in pixel index is not contained in the logical indices.");
			}
		}

        public byte GetOPCPixelChannel(IPixel pixel)
        {
            return 
        }

	    public JsonConfigPixelMapper(string configFilePath)
	    {
		    
	    }
        
    }
}