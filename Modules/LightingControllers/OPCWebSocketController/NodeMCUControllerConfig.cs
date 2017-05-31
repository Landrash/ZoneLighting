﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCWebSocketController
{
	public class NodeMCUControllerConfig
	{
		public string Name { get; set; }
		public string ServerURL { get; set; }
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
