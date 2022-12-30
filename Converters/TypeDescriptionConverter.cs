#nullable enable
using Sandbox;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for a <see cref="TypeDescription"/>.
/// </summary>
internal class TypeDescriptionConverter : JsonConverter<TypeDescription>
{
	/// <inheritdoc/>
	public override TypeDescription Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return TypeLibrary.GetType( reader.GetString()! );
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, TypeDescription value, JsonSerializerOptions options )
	{
		writer.WriteStringValue( value.TargetType.FullName ?? value.TargetType.Name );
	}
}
#nullable restore
