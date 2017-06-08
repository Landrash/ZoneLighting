using System;

namespace ZoneLighting.ConfigNS
{
	[AttributeUsage(AttributeTargets.Property)]
	public class GetOnlyJsonPropertyAttribute : Attribute
	{
	}
}
