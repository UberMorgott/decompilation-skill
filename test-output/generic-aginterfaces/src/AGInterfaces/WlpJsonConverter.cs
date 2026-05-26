using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AGInterfaces;

internal class WlpJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(List<Param>);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		if (reader.TokenType == JsonToken.StartObject)
		{
			return null;
		}
		return serializer.Deserialize<List<Param>>(reader);
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}
