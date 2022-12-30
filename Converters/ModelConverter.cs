#nullable enable
using Sandbox;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for a <see cref="Model"/>.
/// </summary>
internal class ModelConverter : JsonConverter<Model>
{
	/// <inheritdoc/>
	public override Model Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return Model.Load( reader.GetString()! );
	}

	/// <inheritdoc/>
	public override Model ReadAsPropertyName( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return Model.Load( reader.GetString()! );
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, Model value, JsonSerializerOptions options )
	{
		if ( value.IsError || value.IsProcedural )
			throw new NotSupportedException( "Error or procedural models are not supported" );

		writer.WriteStringValue( value.Name );
	}

	/// <inheritdoc/>
	public override void WriteAsPropertyName( Utf8JsonWriter writer, Model value, JsonSerializerOptions options )
	{
		if ( value.IsError || value.IsProcedural )
			throw new NotSupportedException( "Error or procedural models are not supported" );

		writer.WritePropertyName( value.Name );
	}
}
#nullable restore
