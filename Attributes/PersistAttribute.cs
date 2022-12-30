#nullable enable
using System;

namespace Gooman.Persistence.Attributes;

/// <summary>
/// Marks a property to be persisted between sessions.
/// </summary>
[AttributeUsage( AttributeTargets.Property, AllowMultiple = false, Inherited = true )]
public sealed class PersistAttribute : Attribute
{
}
#nullable restore
