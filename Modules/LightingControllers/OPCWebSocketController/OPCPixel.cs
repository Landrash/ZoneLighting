using System.Runtime.Serialization;
using ZoneLighting.Communication;

namespace OPCWebSocketController
{
    /// <summary>
    /// Represents a pixel that can be controlled by the Open Pixel Control protocol.
    /// http://openpixelcontrol.org/
    /// </summary>
    [DataContract]
	public abstract class OPCPixel : PhysicalRGBLight
	{
		protected OPCPixel()
		{
			
		}

		protected OPCPixel(byte channel, int physicalIndex)
		{
			Channel = channel;
			PhysicalIndex = physicalIndex;
		}

        [DataMember]
		public byte Channel { get; set; }
        public abstract int RedIndex { get; }
        public abstract int GreenIndex { get; }
        public abstract int BlueIndex { get; }
	}

	public class OPCRGBPixel : OPCPixel
	{
		public override int RedIndex => PhysicalIndex * 3;
		public override int GreenIndex => PhysicalIndex * 3 + 1;
		public override int BlueIndex => PhysicalIndex * 3 + 2;
	}

	public class OPCRBGPixel : OPCPixel
	{
		public override int RedIndex => PhysicalIndex * 3;
		public override int GreenIndex => PhysicalIndex * 3 + 2;
		public override int BlueIndex => PhysicalIndex * 3 + 1;
	}
    
    public enum OPCPixelType
	{
		None,
		OPCRGBPixel,
		OPCRBGPixel,
	}
}