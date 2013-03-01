using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

using GWNorthEngine.Engine;
using GWNorthEngine.Engine.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Logic;
using SnakeRawrRawr.Model.Display;

namespace SnakeRawrRawr.Engine {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Renderer : BaseRenderer {
		private Cinematic cinematic;
		private GameDisplay gameDisplay;
		private MainMenu mainMenu;
		private OptionsMenu optionsMenu;
		private IRenderable activeDisplay;
		private const string GAME_NAME = "Snake Rawr Rawr";

		public Renderer() {
			BaseRendererParams baseParms = new BaseRendererParams();
			baseParms.MouseVisible = true;
			baseParms.WindowsText = GAME_NAME;
			baseParms.ScreenWidth = Constants.RESOLUTION_X;
			baseParms.ScreenHeight = Constants.RESOLUTION_Y;
			baseParms.ContentRootDirectory = "SnakeRawrRawrContent";
			baseParms.MouseVisible = false;
#if DEBUG
			baseParms.RunningMode = RunningMode.Debug;
#else
			baseParms.RunningMode = RunningMode.Release;
#endif
			base.initialize(baseParms);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			SoundManager.getInstance().init(Content);
			IOHelper.loadConfiguration(IOHelper.getConfiguration());

			this.cinematic = new Cinematic(Content);
			this.mainMenu = new MainMenu(Content);
			this.optionsMenu = new OptionsMenu(Content);
#if WINDOWS
#if DEBUG
			ScriptManager.getInstance().LogFile = "Log.log";
			if (StateManager.getInstance().CurrentGameState == GameState.Active || StateManager.getInstance().CurrentGameState == GameState.Waiting) {
				this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
			}
#endif
#endif

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			base.UnloadContent();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
#if DEBUG
			base.Window.Title = GAME_NAME + "...FPS: " + FrameRate.getInstance().calculateFrameRate(gameTime) + "    X:" +
				InputManager.getInstance().MouseX + " Y:" + InputManager.getInstance().MouseY;

			if (InputManager.getInstance().wasKeyPressed(Keys.R)) {
				this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
			}
			if (InputManager.getInstance().wasKeyPressed(Keys.Escape) ||
			InputManager.getInstance().wasButtonPressed(PlayerIndex.One, Buttons.B)) {
				this.Exit();
			}
#endif

			if (StateManager.getInstance().CurrentGameState == GameState.Exit) {
				this.Exit();
			} else if (StateManager.getInstance().CurrentGameState == GameState.LoadGame) {
				this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
				StateManager.getInstance().CurrentGameState = GameState.Waiting;
			} else if (StateManager.getInstance().CurrentGameState == GameState.LoadOptions) {
				this.optionsMenu = new OptionsMenu(Content);
				StateManager.getInstance().CurrentGameState = GameState.Options;
			}

			base.IsMouseVisible = false;
			if (StateManager.getInstance().CurrentGameState == GameState.CompanyCinematic) {
				this.activeDisplay = this.cinematic;
			} else if (StateManager.getInstance().CurrentGameState == GameState.MainMenu) {
				this.activeDisplay = this.mainMenu;
			} else if (StateManager.getInstance().CurrentGameState == GameState.Waiting || StateManager.getInstance().CurrentGameState == GameState.Active ||
				StateManager.getInstance().CurrentGameState == GameState.GameOver) {
				this.activeDisplay = this.gameDisplay;
			} else if (StateManager.getInstance().CurrentGameState == GameState.Options) {
				if (!InputManager.getInstance().isLeftButtonDown()) {
					base.IsMouseVisible = true;
				}
				this.activeDisplay = this.optionsMenu;
			}

			float elapsed = gameTime.ElapsedGameTime.Milliseconds;
			this.activeDisplay.update(elapsed);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			base.spriteBatch.Begin();
			this.activeDisplay.render(base.spriteBatch);
			base.spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
