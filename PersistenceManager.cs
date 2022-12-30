#nullable enable
using Gooman.Persistence.Attributes;
using Gooman.Persistence.Extensions;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gooman.Persistence;

/// <summary>
/// Handles (de)serialization of data in a game.
/// </summary>
public static class PersistenceManager
{
	/// <summary>
	/// A dictionary containing all entity types that have a set of persistable properties.
	/// </summary>
	private static readonly Dictionary<TypeDescription, List<PropertyDescription>> persistenceMap = new();

	/// <summary>
	/// Whether or not the <see cref="PersistenceManager"/> has been initialized.
	/// </summary>
	private static bool initialized = false;

	/// <summary>
	/// Initializes the persistence system. Finds all entity types and searches for persistable properties on them.
	/// </summary>
	internal static void Initialize()
	{
		persistenceMap.Clear();

		var types = TypeLibrary.GetTypes<Entity>();
		foreach ( var type in types )
		{
			var attributes = type.GetAttributes<PersistPropertyAttribute>();
			var propertyHashSet = new HashSet<string>();
			foreach ( var attribute in attributes )
				propertyHashSet.Add( attribute.PropertyName );

			var propertyMap = new List<PropertyDescription>();
			var properties = type.Properties;
			foreach ( var property in properties )
			{
				if ( property.GetCustomAttribute<PersistAttribute>() is not null )
					propertyMap.Add( property );

				if ( propertyHashSet.Contains( property.Name ) )
					propertyMap.Add( property );
			}

			if ( propertyMap.Count == 0 )
				continue;

			persistenceMap.Add( type, propertyMap );
		}

		initialized = true;
	}

	/// <summary>
	/// Gets a list of persistable properties on the type provided.
	/// </summary>
	/// <param name="typeDescription">The description to find properties on.</param>
	/// <returns>A list of persistable properties on the type.</returns>
	/// <exception cref="ArgumentException">Thrown when the type description provided does not derive from <see cref="Entity"/>.</exception>
	public static IReadOnlyList<PropertyDescription> GetPersistableProperties( TypeDescription typeDescription )
	{
		if ( !initialized )
			Initialize();

		if ( !typeDescription.TargetType.IsAssignableTo( typeof( Entity ) ) )
			throw new ArgumentException( $"Persistable properties are only supported on {nameof( Entity )}", nameof( typeDescription ) );

		if ( !persistenceMap.TryGetValue( typeDescription, out var properties ) )
			return Array.Empty<PropertyDescription>();

		return properties;
	}

	/// <summary>
	/// Adds a property to be persisted on the entity type.
	/// </summary>
	/// <param name="type">The entity type to look at.</param>
	/// <param name="propertyName">The name of the property to persist.</param>
	/// <exception cref="ArgumentException">
	/// 1. Thrown when the type is not derived from <see cref="Entity"/>.
	/// 2. Thrown when the property does not exist on the type provided.
	/// </exception>
	public static void PersistProperty( TypeDescription type, string propertyName )
	{
		if ( !initialized )
			Initialize();

		if ( !type.TargetType.IsAssignableTo( typeof( Entity ) ) )
			throw new ArgumentException( $"Persistence is only supported on types that are derived from {nameof( Entity )}", nameof( type ) );

		var property = type.GetProperty( propertyName );
		if ( property is null )
			throw new ArgumentException( null, nameof( propertyName ) );

		if ( !persistenceMap.ContainsKey( type ) )
			persistenceMap.Add( type, new List<PropertyDescription>() );

		if ( !persistenceMap[type].Contains( property ) )
			persistenceMap[type].Add( property );
	}

	/// <summary>
	/// Gets all entity instances that have persistable data and builds a save file.
	/// </summary>
	/// <returns>A save file representing all persitable data now.</returns>
	public static PersistentState GetCurrentState()
	{
		if ( !initialized )
			Initialize();

		var entities = new List<PersistentEntityData>();
		foreach ( var (type, propertyDescriptions) in persistenceMap )
		{
			if ( type.GetAttribute<ManuallyPersistAttribute>() is not null )
				continue;

			// FIXME: Don't use Type.Name, use Type.Equals( Type ) when it gets whitelisted. https://github.com/sboxgame/issues/issues/2612
			foreach ( var entity in Entity.All.Where( entity => entity.GetType().Name == type.TargetType.Name ) )
				entities.Add( entity.GetPersisitentData() );
		}

		return new PersistentState( entities );
	}

	/// <summary>
	/// Refreshes the <see cref="persistenceMap"/> on hotload.
	/// </summary>
	[Event.Hotload]
	private static void Hotload()
	{
		initialized = false;
	}
}
#nullable restore
