#nullable enable
using Sandbox;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for a <see cref="TimeSince"/>.
/// </summary>
internal class TimeSinceConverter : JsonConverter<TimeSince>
{
	/// <inheritdoc/>
	public override TimeSince Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return reader.GetSingle();
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, TimeSince value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value );
	}
}
#nullable restore
