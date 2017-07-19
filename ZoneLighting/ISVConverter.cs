using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZoneLighting.ZoneProgramNS;

namespace ZoneLighting
{
	public class ISVConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ISV);
		}

		public override bool CanWrite => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var result = new ISV();
			reader.Read();

			while (reader.TokenType == JsonToken.PropertyName)
			{
				var propertyName = ((string)reader.Value).ToPascalCase();
				reader.Read();

				var value = reader.TokenType == JsonToken.Integer ? Convert.ToInt32(reader.Value) : serializer.Deserialize(reader);
				result.Add(propertyName, value);
				reader.Read();
			}

			return result;
		}

		/// <summary>
		/// Suppresses camel casing for input property names
		/// </summary>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var isv = (ISV)value;

			writer.WriteStartObject();
			isv.Keys.ToList().ForEach(key =>
			{
				writer.WritePropertyName(key.ToPascalCase());
				serializer.Serialize(writer, isv[key]);
			});
			writer.WriteEndObject();
		}
	}
}