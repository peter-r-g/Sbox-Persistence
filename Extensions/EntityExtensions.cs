#nullable enable
using Sandbox;
using System.Collections.Generic;

namespace Gooman.Persistence.Extensions;

/// <summary>
/// A utility class containing extensions for <see cref="Entity"/>.
/// </summary>
public static class EntityExtensions
{
	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.GetPersistableProperties(TypeDescription)"/>.
	/// </summary>
	/// <param name="entity">The entity whose type to use.</param>
	/// <returns>A list of persistable properties on the entity type.</returns>
	public static IReadOnlyList<PropertyDescription> GetPersistableProperties( this Entity entity )
	{
		return PersistenceManager.GetPersistableProperties( TypeLibrary.GetType( entity.GetType() ) );
	}

	/// <summary>
	/// Creates a <see cref="PersistentEntityData"/> from the <see cref="Entity"/>.
	/// </summary>
	/// <param name="entity">The entity to get persistent data from.</param>
	/// <returns>The <see cref="PersistentEntityData"/> of the <see cref="Entity"/>.</returns>
	public static PersistentEntityData GetPersisitentData( this Entity entity )
	{
		var properties = new Dictionary<PropertyDescription, object?>();
		foreach ( var property in entity.GetPersistableProperties() )
			properties.Add( property, property.GetValue( entity ) );

		return new PersistentEntityData( TypeLibrary.GetType( entity.GetType() ), properties );
	}

	/// <summary>
	/// A short-hand method to <see cref="PersistenceManager.PersistProperty(TypeDescription, string)"/>.
	/// </summary>
	/// <param name="entity">The entity whose type to use.</param>
	/// <param name="propertyName">The name of the property to persist.</param>
	public static void PersistProperty( this Entity entity, string propertyName )
	{
		PersistenceManager.PersistProperty( TypeLibrary.GetType( entity.GetType() ), propertyName );
	}
}
#nullable restore
