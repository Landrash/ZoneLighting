using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ZoneLighting.ZoneProgramNS.Input
{
	[DataContract]
	public class EnumeratedValuesZoneProgramInput<T> : ZoneProgramInput
	{
		public EnumeratedValuesZoneProgramInput(string name, Type type, List<T> enumeratedValues) : base(name, type)
		{
			EnumeratedValues = enumeratedValues;
		}

		[DataMember]
		public List<T> EnumeratedValues { get; set; }
	}
}
