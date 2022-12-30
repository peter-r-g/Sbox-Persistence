#nullable enable
using System;

namespace Gooman.Persistence.Attributes;

/// <summary>
/// Marks a property inside the class to be persisted. Use this when you cannot use <see cref="PersistAttribute"/> on a property. Like on an inaccessible base class.
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = true, Inherited = true )]
public sealed class PersistPropertyAttribute : Attribute
{
	/// <summary>
	/// The name of the property to persist.
	/// </summary>
	public string PropertyName { get; }

	/// <summary>
	/// Initializes a new isntance of <see cref="PersistPropertyAttribute"/> with the name of the property to persist.
	/// </summary>
	/// <param name="propertyName">The name of the property to persist.</param>
	public PersistPropertyAttribute( string propertyName )
	{
		PropertyName = propertyName;
	}
}
#nullable restore
