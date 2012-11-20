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
		private float elapsedTransitionTime;
		private bool wait;
		private const float TRANSITION_TIME = 1500f;
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

			FadeEffectParams effectParms = new FadeEffectParams {
				OriginalColour = Color.White,
				TotalTransitionTime = 1000f,
				State = FadeEffect.FadeState.In
			};
			this.cinematic.addEffect(new FadeEffect(effectParms));
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			this.elapsedTransitionTime += elapsed;
			this.cinematic.update(elapsed);
			FadeEffect effect = ((FadeEffect)this.cinematic.Effects[0]);
			if (wait) {
				if (this.elapsedTransitionTime >= TRANSITION_TIME) {
					effect.State = FadeEffect.FadeState.Out;
					effect.ElapsedTransitionTime = 0f;
					wait = false;
				}
			} else if (effect.State == FadeEffect.FadeState.In) {
				if (effect.ElapsedTransitionTime >= TRANSITION_TIME) {
					wait = true;
					this.elapsedTransitionTime = 0f;
				}
			} else {
				if (effect.ElapsedTransitionTime >= TRANSITION_TIME) {
					StateManager.getInstance().CurrentGameState = GameState.MainMenu;
				}
			}

			if (InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				StateManager.getInstance().CurrentGameState = GameState.MainMenu;
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
