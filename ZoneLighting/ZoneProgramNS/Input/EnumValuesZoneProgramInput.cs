using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ZoneLighting.ZoneProgramNS.Input
{
	[DataContract]
	public class EnumValuesZoneProgramInput<T> : ZoneProgramInput
	{
		public EnumValuesZoneProgramInput(string name, Type type, Type enumType) : base(name, type)
		{
			if (type.IsEnum)
			{
				EnumValues = Enum.GetValues(enumType).Cast<T>();
			}
		}

		[DataMember]
		public IEnumerable<T> EnumValues { get; set; }
	}
}
