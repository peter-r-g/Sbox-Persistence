#nullable enable
using Gooman.Persistence.Options;
using Sandbox;
using Sandbox.Diagnostics;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Gooman.Persistence;

/// <summary>
/// Manages save files and autosaving in the game.
/// </summary>
public sealed class SaveManager
{
	/// <summary>
	/// A read-only set of options that this manager has been configured with.
	/// </summary>
	public IReadOnlySaveOptions Options { get; }

	/// <summary>
	/// The last <see cref="BaseFileSystem"/> that was used to save.
	/// </summary>
	public BaseFileSystem? LatestSaveFileSystem { get; private set; }

	/// <summary>
	/// The file path to the latest save that has occurred.
	/// </summary>
	public string? LatestSavePath { get; private set; }

	/// <summary>
	/// The cancellation token source of the autosave task.
	/// </summary>
	private CancellationTokenSource? autosaveToken;
	/// <summary>
	/// The number of the next autosave to save.
	/// </summary>
	private int autosaveNumber = 1;

	/// <summary>
	/// Initializes a new instance of <see cref="SaveManager"/> with a pre-configured set of options.
	/// </summary>
	/// <param name="options">The options for this manager to use.</param>
	public SaveManager( IReadOnlySaveOptions options )
	{
		Assert.NotNull( options );

		Options = options.Clone();
		if ( !Options.AutosaveEnabled )
			return;

		var dir = Path.GetDirectoryName( FileSystem.NormalizeFilename( Options.AutosavePath ) );
		if ( dir != "." )
			FileSystem.Data.CreateDirectory( dir );
		RestartAutosave();
	}

	/// <summary>
	/// Loads an existing save.
	/// </summary>
	/// <param name="fs">The <see cref="BaseFileSystem"/> containing the path to the save.</param>
	/// <param name="path">The path to the save.</param>
	/// <exception cref="FileNotFoundException">Thrown when no file exists at the given file path.</exception>
	/// <exception cref="InvalidOperationException">Thrown when no <see cref="PersistentState"/> could be deserialized from the data at the path specified.</exception>
	public PersistentState Load( BaseFileSystem fs, string path )
	{
		if ( Options.LoadOverride is not null )
			return Options.LoadOverride( this, fs, path );
		
		if ( !fs.FileExists( path ) )
			throw new FileNotFoundException( path );

		using var stream = fs.OpenRead( path );
		var state = JsonSerializer.Deserialize<PersistentState>( stream, Options.JsonOptions );
		if ( state is null )
			throw new InvalidOperationException( $"Failed to deserialize a {nameof( PersistentState )} from the data stored in \"{path}\"" );

		return state;
	}

	/// <summary>
	/// Loads the latest save.
	/// </summary>
	/// <exception cref="FileNotFoundException">Thrown when no file exists at the latest save path.</exception>
	/// <exception cref="InvalidOperationException">
	/// 1. Thrown when no recent save has occurred.
	/// 2. Thrown when no <see cref="PersistentState"/> could be deserialized from the data at the path specified.
	/// </exception>
	public PersistentState LoadLatest()
	{
		if ( LatestSaveFileSystem is null || string.IsNullOrEmpty( LatestSavePath ) )
			throw new InvalidOperationException( "No recent save to load" );

		return Load( LatestSaveFileSystem, LatestSavePath );
	}

	/// <summary>
	/// Saves the current game state.
	/// </summary>
	/// <param name="path">The path to save at.</param>
	/// <param name="state">The current state of the game. If null, a new state will be retrieved.</param>
	/// <returns>The state that was saved.</returns>
	public PersistentState Save( string path, PersistentState? state = null ) => InternalSave( path, state );

	/// <summary>
	/// The internal method to saving game state.
	/// </summary>
	/// <param name="path">The path to save at.</param>
	/// <param name="state">The current state of the game. If null, a new state will be retrieved.</param>
	/// <param name="autosave">Whether or not the save was triggered by the autosave mechanism.</param>
	/// <returns>The state that was saved.</returns>
	private PersistentState InternalSave( string path, PersistentState? state = null, bool autosave = false )
	{
		state ??= PersistenceManager.GetCurrentState();

		if ( Options.SaveOverride is null )
		{
			var fs = autosave ? Options.AutosaveFileSystem : Options.FileSystem;
			using var stream = fs.OpenWrite( path, FileMode.Create );
			JsonSerializer.Serialize( stream, state, Options.JsonOptions );
		}
		else
			Options.SaveOverride( this, path, autosave, state );

		LatestSaveFileSystem = autosave ? Options.AutosaveFileSystem : Options.FileSystem;
		LatestSavePath = path;

		return state;
	}

	/// <summary>
	/// The autosave task.
	/// </summary>
	/// <param name="token">The token to check for cancellation.</param>
	/// <returns>The asynchronous task that spawns from this invocation.</returns>
	private async Task AutosaveTimer( CancellationToken token )
	{
		var currentTime = Options.AutosaveInterval;
		while ( !token.IsCancellationRequested && Options.AutosaveEnabled )
		{
			await Task.Delay( 1000, token );
			if ( token.IsCancellationRequested || !Options.AutosaveEnabled )
				return;

			currentTime--;
			if ( currentTime > 0 )
				continue;

			InternalSave( GetAutosaveName(), null, true );
			if ( token.IsCancellationRequested || !Options.AutosaveEnabled )
				return;

			currentTime = Options.AutosaveInterval;
		}
	}

	/// <summary>
	/// Gets a path to a new autosave.
	/// </summary>
	/// <returns>A path to a new autosave.</returns>
	private string GetAutosaveName()
	{
		if ( autosaveNumber > Options.AutosaveCount )
			autosaveNumber = 1;

		return Options.AutosavePath.Replace( "{num}", (autosaveNumber++).ToString() );
	}

	/// <summary>
	/// Restarts the <see cref="AutosaveTimer(CancellationToken)"/>.
	/// </summary>
	[Event.Hotload]
	private void RestartAutosave()
	{
		autosaveToken?.Cancel();
		if ( !Options.AutosaveEnabled )
			return;

		autosaveToken = new CancellationTokenSource();
		_ = AutosaveTimer( autosaveToken.Token );
	}
}
#nullable restore
