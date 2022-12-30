#nullable enable
using Sandbox;
using System.Collections.Generic;

namespace Gooman.Persistence.Extensions;

/// <summary>
/// A utility class containing extensions for <see cref="TypeDescription"/>.
/// </summary>
public static class TypeDescriptionExtensions
{
	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.GetPersistableProperties(TypeDescription)"/>.
	/// </summary>
	/// <param name="typeDescription">The description to find properties on.</param>
	/// <returns>A list of persistable properties on the type.</returns>
	public static IReadOnlyList<PropertyDescription> GetPersistableProperties( this TypeDescription typeDescription )
	{
		return PersistenceManager.GetPersistableProperties( typeDescription );
	}

	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.PersistProperty(TypeDescription, string)"/>.
	/// </summary>
	/// <param name="typeDescription">The description to use.</param>
	/// <param name="propertyName">The name of the property to persist.</param>
	public static void PersistProperty( this TypeDescription typeDescription, string propertyName )
	{
		PersistenceManager.PersistProperty( typeDescription, propertyName );
	}
}
#nullable restore
