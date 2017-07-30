using System;
using System.Linq;
using Newtonsoft.Json;
using ZoneLighting.ZoneProgramNS;

namespace ZoneLighting
{
	public class InputBagConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(InputBag);
		}

		public override bool CanWrite => true;

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var result = new InputBag();
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
			var inputBag = (InputBag)value;

			writer.WriteStartObject();
			inputBag.Keys.ToList().ForEach(key =>
			{
				writer.WritePropertyName(key.ToPascalCase());
				serializer.Serialize(writer, inputBag[key]);
			});
			writer.WriteEndObject();
		}
	}
}