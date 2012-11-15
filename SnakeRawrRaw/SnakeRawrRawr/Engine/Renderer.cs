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

		private GameDisplay gameDisplay;
		private const string GAME_NAME = "SnakeRawrRawr";

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
			this.gameDisplay = new GameDisplay(Content);
#if WINDOWS
#if DEBUG
			ScriptManager.getInstance().LogFile = "Log.log";
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
				this.gameDisplay = new GameDisplay(Content);
			}
#endif
			if (InputManager.getInstance().wasKeyPressed(Keys.Escape) ||
						InputManager.getInstance().wasButtonPressed(PlayerIndex.One, Buttons.B)) {
				this.Exit();
			}

			float elapsed = gameTime.ElapsedGameTime.Milliseconds;
			this.gameDisplay.update(elapsed);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			base.spriteBatch.Begin();
			this.gameDisplay.render(base.spriteBatch);
			base.spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
