#nullable enable
using Gooman.Persistence.Converters;
using Sandbox;
using Sandbox.Diagnostics;
using System;
using System.Text.Json;

namespace Gooman.Persistence.Options;

/// <summary>
/// Contains a set of options for the <see cref="SaveManager"/>.
/// </summary>
public sealed class SaveOptions : IReadOnlySaveOptions
{
	/// <inheritdoc/>
	public bool AutosaveEnabled { get; set; } = true;
	/// <inheritdoc/>
	public int AutosaveCount { get; set; } = 3;
	/// <inheritdoc/>
	public int AutosaveInterval { get; set; } = 60;
	/// <inheritdoc/>
	public BaseFileSystem AutosaveFileSystem
	{
		get => autosaveFileSystem ?? FileSystem;
		set => autosaveFileSystem = value;
	}
	/// <summary>
	/// The backing field to <see cref="AutosaveFileSystem"/>.
	/// </summary>
	private BaseFileSystem? autosaveFileSystem = Sandbox.FileSystem.Data;
	/// <inheritdoc/>
	public string AutosavePath { get; set; } = "autosave{num}.json";

	/// <inheritdoc/>
	public BaseFileSystem FileSystem { get; set; } = Sandbox.FileSystem.Data;

	/// <inheritdoc/>
	public JsonSerializerOptions JsonOptions { get; set; }

	/// <inheritdoc/>
	public Func<SaveManager, BaseFileSystem, string, PersistentState>? LoadOverride { get; set; } = null;
	/// <inheritdoc/>
	public Action<SaveManager, string, bool, PersistentState>? SaveOverride { get; set; } = null;

	/// <summary>
	/// Initializes a default instance of <see cref="SaveOptions"/>.
	/// </summary>
	public SaveOptions()
	{
		JsonOptions = new JsonSerializerOptions()
		{
			Converters =
			{
				new ModelConverter(),
				new PropertyDescriptionConverter(),
				new TimeSinceConverter(),
				new TimeUntilConverter(),
				new TypeDescriptionConverter()
			}
		};
	}

	/// <summary>
	/// Initializes a new instance of <see cref="SaveOptions"/> with a copy of an existing <see cref="SaveOptions"/>.
	/// </summary>
	/// <param name="saveOptions">The instance of <see cref="SaveOptions"/> to copy.</param>
	public SaveOptions( SaveOptions saveOptions )
	{
		AutosaveEnabled = saveOptions.AutosaveEnabled;
		AutosaveCount = saveOptions.AutosaveCount;
		AutosaveInterval = saveOptions.AutosaveInterval;
		AutosaveFileSystem = saveOptions.AutosaveFileSystem;
		AutosavePath = saveOptions.AutosavePath;

		FileSystem = saveOptions.FileSystem;

		JsonOptions = saveOptions.JsonOptions;

		LoadOverride = saveOptions.LoadOverride;
		SaveOverride = saveOptions.SaveOverride;
	}

	/// <inheritdoc/>
	public IReadOnlySaveOptions Clone()
	{
		return new SaveOptions( this );
	}

	/// <summary>
	/// Sets the <see cref="AutosaveEnabled"/> property.
	/// </summary>
	/// <param name="autosaveEnabled">See <see cref="AutosaveEnabled"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithAutosaveEnabled( bool autosaveEnabled )
	{
		AutosaveEnabled = autosaveEnabled;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="AutosaveCount"/> property.
	/// </summary>
	/// <param name="autosaveCount">See <see cref="AutosaveCount"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithAutosaveCount( int autosaveCount )
	{
		Assert.True( AutosaveCount > 0, $"{nameof( autosaveCount )} must be greater than 0" );

		AutosaveCount = autosaveCount;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="AutosaveInterval"/> property.
	/// </summary>
	/// <param name="autosaveInterval">See <see cref="AutosaveInterval"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithAutosaveInterval( int autosaveInterval )
	{
		Assert.True( AutosaveInterval > 0, $"{nameof( autosaveInterval )} must be greater than 0" );

		AutosaveInterval = autosaveInterval;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="AutosaveFileSystem"/> property.
	/// </summary>
	/// <param name="autosaveFileSystem">See <see cref="AutosaveFileSystem"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithAutosaveFileSystem( BaseFileSystem autosaveFileSystem )
	{
		Assert.NotNull( autosaveFileSystem );

		AutosaveFileSystem = autosaveFileSystem;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="AutosavePath"/> property.
	/// </summary>
	/// <param name="autosavePath">See <see cref="AutosavePath"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithAutosavePath( string autosavePath )
	{
		Assert.NotNull( autosavePath );

		AutosavePath = autosavePath;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="FileSystem"/> property.
	/// </summary>
	/// <param name="fileSystem">See <see cref="FileSystem"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithFileSystem( BaseFileSystem fileSystem )
	{
		Assert.NotNull( fileSystem );

		FileSystem = fileSystem;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="LoadOverride"/> property.
	/// </summary>
	/// <param name="loadOverride">See <see cref="LoadOverride"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithLoadOverride( Func<SaveManager, BaseFileSystem, string, PersistentState>? loadOverride )
	{
		LoadOverride = loadOverride;
		return this;
	}

	/// <summary>
	/// Sets the <see cref="SaveOverride"/> property.
	/// </summary>
	/// <param name="saveOverride">See <see cref="SaveOverride"/>.</param>
	/// <returns>The <see cref="SaveOptions"/> instance.</returns>
	public SaveOptions WithSaveOverride( Action<SaveManager, string, bool, PersistentState>? saveOverride )
	{
		SaveOverride = saveOverride;
		return this;
	}
}
#nullable restore
