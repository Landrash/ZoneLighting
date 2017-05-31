using System.Collections.Generic;
using System.Linq;
using Anshul.Utilities;

namespace OPCWebSocketController
{
	public class PixelMap
	{
        public List<Pixel> Pixels { get; set; }

	    public int[] LogicalIndices
	    {
	        get { return Pixels.Select(x => x.LogicalIndex).ToArray(); }
	    }

	    public int[] PhysicalIndices
	    {
	        get { return Pixels.Select(x => x.PhysicalIndex).ToArray(); }
	    }

        public byte[] Channels
        {
            get { return Pixels.Select(x => x.Channel).ToArray(); }
        }
    }
}