using System;
using System.Runtime.Serialization;

namespace ZoneLighting.ZoneProgramNS.Input
{
	[DataContract]
	public class RangedZoneProgramInput<T> : ZoneProgramInput
	{
		public RangedZoneProgramInput(string name, Type type, T min, T max) : base(name, type)
		{
			Min = min;
			Max = max;
		}

		[DataMember]
		public T Min { get; set; }
		[DataMember]
		public T Max { get; set; }
	}
}
