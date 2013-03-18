using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class Cinematic : IRenderable {
		#region Class variables
		private StaticDrawable2D cinematic;
		private float elapsedWaitTime;
		private const float WAIT_TIME = 1500f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Cinematic(ContentManager content) {
			Texture2D texture = LoadingUtils.load<Texture2D>(content, "Logo");
			StaticDrawable2DParams parms = new StaticDrawable2DParams {
				Texture = texture,
				Origin = new Vector2(texture.Width / 2, texture.Height / 2),
				Position = new Vector2(Constants.RESOLUTION_X / 2, Constants.RESOLUTION_Y / 2)
			};
			this.cinematic = new StaticDrawable2D(parms);
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			if (StateManager.getInstance().CurrentTransitionState == TransitionState.None) {
				this.elapsedWaitTime += elapsed;
				if (this.elapsedWaitTime >= WAIT_TIME) {
					StateManager.getInstance().CurrentGameState = GameState.MainMenu;
				}
			}
			if (StateManager.getInstance().CurrentTransitionState == TransitionState.None || StateManager.getInstance().CurrentTransitionState == TransitionState.TransitionIn) {
				if (InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
					StateManager.getInstance().CurrentGameState = GameState.MainMenu;
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.cinematic != null) {
				this.cinematic.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
