#nullable enable
using Sandbox;
using System;
using System.Collections.Generic;

namespace Gooman.Persistence.Extensions;

/// <summary>
/// A utility class containing extensions for <see cref="Type"/>.
/// </summary>
public static class TypeExtensions
{
	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.GetPersistableProperties(TypeDescription)"/>.
	/// </summary>
	/// <param name="type">The type to find properties on.</param>
	/// <returns>A list of persistable properties on the type.</returns>
	public static IReadOnlyList<PropertyDescription> GetPersistableProperties( this Type type )
	{
		return PersistenceManager.GetPersistableProperties( TypeLibrary.GetType( type ) );
	}

	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.PersistProperty(TypeDescription, string)"/>.
	/// </summary>
	/// <param name="type">The type to use.</param>
	/// <param name="propertyName">The name of the property to persist.</param>
	public static void PersistProperty( this Type type, string propertyName )
	{
		PersistenceManager.PersistProperty( TypeLibrary.GetType( type ), propertyName );
	}
}
#nullable restore
