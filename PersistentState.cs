#nullable enable
using Sandbox;
using System.Collections.Generic;

namespace Gooman.Persistence;

/// <summary>
/// Contains a capture of all persistable entities and their property data.
/// </summary>
public class PersistentState
{
	/// <summary>
	/// A list of persistent entity data.
	/// </summary>
	public IReadOnlyList<PersistentEntityData> Entities { get; }

	/// <summary>
	/// Initializes a new instance of <see cref="PersistentState"/> with the list of persistent entity data to contain.
	/// </summary>
	/// <param name="entities">The persistent entity data to contain.</param>
	public PersistentState( IReadOnlyList<PersistentEntityData> entities )
	{
		Entities = entities;
	}

	/// <summary>
	/// Returns an iterator containing all <see cref="PersistentEntityData"/> that points to an <see cref="Entity"/> of type <see ref="T"/>.
	/// </summary>
	/// <param name="exact">Whether or not to check for entities with the exact provided type.</param>
	/// <typeparam name="T">The type of <see cref="Entity"/> to get.</typeparam>
	/// <returns>An iterator containing all <see cref="PersistentEntityData"/> that points to an <see cref="Entity"/> of type <see ref="T"/>.</returns>
	public IEnumerable<PersistentEntityData> EntitiesOfType<T>( bool exact = false ) where T : Entity
	{
		var tType = typeof( T );
		foreach ( var entity in Entities )
		{
			var entityType = entity.Type.TargetType;
			if ( exact )
			{
				// FIXME: Don't use Type.Name, use Type.Equals( Type ) when it gets whitelisted. https://github.com/sboxgame/issues/issues/2612
				if ( (entityType.FullName ?? entityType.Name) == (tType.FullName ?? tType.Name) )
					yield return entity;

				continue;
			}
			
			if ( entityType.IsAssignableTo( tType ) )
				yield return entity;
		}
	}
}
#nullable restore
