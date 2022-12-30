#nullable enable
using Gooman.Persistence.Converters;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gooman.Persistence;

/// <summary>
/// Contains the type of an <see cref="Entity"/> and its persistable properties.
/// </summary>
public class PersistentEntityData
{
	/// <summary>
	/// The type of the <see cref="Entity"/>.
	/// </summary>
	public TypeDescription Type { get; }

	/// <summary>
	/// The persistable properties attached to the <see cref="Entity"/>.
	/// </summary>
	[JsonConverter( typeof(EntityPropertiesConverter) )]
	public IReadOnlyDictionary<PropertyDescription, object?> Properties { get; }

	/// <summary>
	/// Initializes a new instance of <see cref="PersistentEntityData"/> with an <see cref="Entity"/> type and its properties.
	/// </summary>
	/// <param name="type">The type of the <see cref="Entity"/>.</param>
	/// <param name="properties">The persistable properties on the <see cref="Entity"/>.</param>
	public PersistentEntityData( TypeDescription type, IReadOnlyDictionary<PropertyDescription, object?> properties )
	{
		Type = type;
		Properties = properties;
	}

	/// <summary>
	/// Gets a stored property value inside of the data.
	/// </summary>
	/// <param name="propertyName">The name of the property to search for.</param>
	/// <typeparam name="T">The type to expect the property value to be.</typeparam>
	/// <returns>The value of the property.</returns>
	/// <exception cref="ArgumentException">
	/// 1. Thrown when no persistable property is found with the <see ref="propertyName"/>.
	/// 2. Thrown when the property value is not applicable to <see ref="T"/>.
	/// </exception>
	public T GetPropertyValue<T>( string propertyName )
	{
		if ( !Properties.TryGetValue( Type.GetProperty( propertyName ), out var propertyValue ) )
			throw new ArgumentException( $"No persistable property named \"{propertyName}\" is contained in this data", nameof( propertyName ) );

		if ( propertyValue is not T expectedValue )
			throw new ArgumentException( $"The value of the \"{propertyName}\" property is not applicable to {nameof( T )}", nameof( T ) );

		return expectedValue;
	}
}
#nullable restore
