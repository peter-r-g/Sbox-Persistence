#nullable enable
using Sandbox;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gooman.Persistence.Converters;

/// <summary>
/// A <see cref="JsonConverter"/> for a <see cref="TimeUntil"/>.
/// </summary>
internal class TimeUntilConverter : JsonConverter<TimeUntil>
{
	/// <inheritdoc/>
	public override TimeUntil Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		return reader.GetSingle();
	}

	/// <inheritdoc/>
	public override void Write( Utf8JsonWriter writer, TimeUntil value, JsonSerializerOptions options )
	{
		writer.WriteNumberValue( value.Relative );
	}
}
#nullable restore
