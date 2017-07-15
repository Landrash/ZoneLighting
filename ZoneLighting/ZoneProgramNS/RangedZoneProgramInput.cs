using System;
using System.Reactive.Subjects;
using System.Runtime.Serialization;
using ZoneLighting.ZoneNS;

namespace ZoneLighting.ZoneProgramNS
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
