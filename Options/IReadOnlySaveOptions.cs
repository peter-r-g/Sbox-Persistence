#nullable enable
using Sandbox;
using System;
using System.Text.Json;

namespace Gooman.Persistence.Options;

/// <summary>
/// Defines an object that has options used by the <see cref="SaveManager"/>.
/// </summary>
public interface IReadOnlySaveOptions
{
	/// <summary>
	/// Whether or not the autosaving mechanism in <see cref="SaveManager"/> should be enabled.
	/// </summary>
	bool AutosaveEnabled { get; }
	/// <summary>
	/// The amount of autosaves the mechanism should maintain.
	/// </summary>
	int AutosaveCount { get; }
	/// <summary>
	/// The time in seconds between each autosave.
	/// </summary>
	int AutosaveInterval { get; }
	/// <summary>
	/// The <see cref="BaseFileSystem"/> to write to for autosaves.
	/// </summary>
	BaseFileSystem AutosaveFileSystem { get; }
	/// <summary>
	/// The file path scheme to contain autosave files. Use "./" for the root directory of <see cref="AutosaveFileSystem"/>. {num} will be replaced by the autosave number.
	/// </summary>
	string AutosavePath { get; }

	/// <summary>
	/// The <see cref="BaseFileSystem"/> to write to for general saves.
	/// </summary>
	BaseFileSystem FileSystem { get; }

	/// <summary>
	/// The <see cref="JsonSerializerOptions"/> to use in Json (de)serialization.
	/// </summary>
	JsonSerializerOptions JsonOptions { get; }

	/// <summary>
	/// Overrides the default single file load behavior of the <see cref="SaveManager"/>.
	/// </summary>
	Func<SaveManager, BaseFileSystem, string, PersistentState>? LoadOverride { get; }
	/// <summary>
	/// Overrides the default single file save behavior of the <see cref="SaveManager"/>.
	/// </summary>
	Action<SaveManager, string, bool, PersistentState>? SaveOverride { get; }

	/// <summary>
	/// Clones the <see cref="IReadOnlySaveOptions"/> to a new instance.
	/// </summary>
	/// <returns>The cloned instance of <see cref="IReadOnlySaveOptions"/>.</returns>
	IReadOnlySaveOptions Clone();
}
#nullable restore
