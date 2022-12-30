#nullable enable
using Sandbox;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for a <see cref="PropertyDescription"/>.
/// </summary>
internal class PropertyDescriptionConverter : JsonConverter<PropertyDescription>
{
	/// <inheritdoc/>
	public override PropertyDescription Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		var str = (ReadOnlySpan<char>)reader.GetString();
		var lastDotIndex = str.LastIndexOf( '.' );

		var typeName = str[..lastDotIndex];
		var propertyName = str[(lastDotIndex + 1)..];

		return TypeLibrary.GetType( typeName.ToString() ).GetProperty( propertyName.ToString() );
	}

	/// <inheritdoc/>
	public override PropertyDescription ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		reader.Read();
		var str = (ReadOnlySpan<char>)reader.GetString();
		var lastDotIndex = str.LastIndexOf( '.' );

		var typeName = str[..lastDotIndex];
		var propertyName = str[(lastDotIndex + 1)..];

		return TypeLibrary.GetType( typeName.ToString() ).GetProperty( propertyName.ToString() );
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, PropertyDescription value, JsonSerializerOptions options )
	{
		var typeName = value.TypeDescription.TargetType.FullName ?? value.TypeDescription.TargetType.Name;
		writer.WriteStringValue( typeName + '.' + value.Name );
	}

	/// <inheritdoc/>
	public override void WriteAsPropertyName( Utf8JsonWriter writer, PropertyDescription value, JsonSerializerOptions options )
	{
		var typeName = value.TypeDescription.TargetType.FullName ?? value.TypeDescription.TargetType.Name;
		writer.WritePropertyName( typeName + '.' + value.Name );
	}
}
#nullable restore
