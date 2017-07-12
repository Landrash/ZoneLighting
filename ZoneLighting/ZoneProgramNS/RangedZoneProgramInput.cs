using System;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using ZoneLighting.ZoneNS;

namespace ZoneLighting.ZoneProgramNS
{
	[DataContract]
	public class RangedZoneProgramInput : ZoneProgramInput
	{
		public RangedZoneProgramInput(string name, Type type, double min, double max) : base(name, type)
		{
			Min = min;
			Max = max;
		}

		[DataMember]
		public double Min { get; set; }
		public double Max { get; set; }
	}
}
