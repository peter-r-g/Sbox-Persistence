#nullable enable
using Sandbox;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for <see cref="PersistentEntityData.Properties"/>.
/// </summary>
internal class EntityPropertiesConverter : JsonConverter<IReadOnlyDictionary<PropertyDescription, object?>>
{
	/// <inheritdoc/>
	public override IReadOnlyDictionary<PropertyDescription, object?> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		var dictionary = new Dictionary<PropertyDescription, object?>();
		var converter = new PropertyDescriptionConverter();

		reader.Read();
		while ( reader.TokenType != JsonTokenType.EndObject )
		{
			var property = converter.Read( ref reader, typeof( PropertyDescription ), options );

			reader.Read();
			var value = JsonSerializer.Deserialize( ref reader, property.PropertyType, options );

			dictionary.Add( property, value );
			reader.Read();
		}

		return dictionary;
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, IReadOnlyDictionary<PropertyDescription, object?> value, JsonSerializerOptions options )
	{
		JsonSerializer.SerializeToElement( value, options ).WriteTo( writer );
	}
}
#nullable restore
