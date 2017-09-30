using System.Collections.Generic;
using Anshul.Utilities;
using ZoneLighting.Usables;
using ZoneLighting.ZoneProgramNS;
using ZoneLighting.ZoneProgramNS.Input;

namespace WebRemote.Models
{
	public class ZoneJsonModel : IBetterListType
	{
		public string Name { get; set; }
		
		public string ProgramName { get; set; }
		
		public double Brightness { get; set; }

		public bool Running { get; set; }

		public int LightCount { get; set; }

		public InputInfo Inputs { get; set; }
	}
}