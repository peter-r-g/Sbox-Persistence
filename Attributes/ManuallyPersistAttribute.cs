#nullable enable
using System;

namespace Gooman.Persistence.Attributes;

/// <summary>
/// Marks an entity class to not be serialized automatically by the <see cref="PersistenceManager"/>.
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
public sealed class ManuallyPersistAttribute : Attribute
{
}
#nullable restore
