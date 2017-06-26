using System.Collections.Generic;

namespace OPCWebSocketController
{
	public class NodeMCUControllerConfig
	{
		public string Name { get; set; }
		public string ServerURL { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public OPCPixelType OPCPixelType { get; set; }
		public byte Channel { get; set; }
		public List<Pixel> Pixels { get; set; }
		public PixelMapperType? PixelMapperType { get; set; }
	}

	public enum PixelMapperType
	{
		OneToOne,
		Static
	}
}
