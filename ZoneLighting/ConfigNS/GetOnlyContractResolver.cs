using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ZoneLighting.ConfigNS
{
	public class GetOnlyContractResolver : DefaultContractResolver
	{
		protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
		{
			var property = base.CreateProperty(member, memberSerialization);
			if (property != null && property.Writable)
			{
				var attributes = property.AttributeProvider.GetAttributes(typeof(GetOnlyJsonPropertyAttribute), true);
				if (attributes != null && attributes.Count > 0)
					property.Writable = false;
			}
			return property;
		}
	}
}