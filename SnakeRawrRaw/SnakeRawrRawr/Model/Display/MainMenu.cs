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
	public class MainMenu : IRenderable {
		#region Class variables
		private StaticDrawable2D background;
		private int index;
		private StaticDrawable2D[] menuItems;
		private PulseEffectParams effectParms;
		private const float SPACE = 65f;
		private readonly string[] BUTTON_NAMES = { "OnePlayer", "TwoPlayer", "Options", "Exit" };
		private readonly Color DEFAULT = Color.Red;
		private readonly Vector2 DEFAULT_SCALE = new Vector2(1f, .75f);
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public MainMenu(ContentManager content) {
			Texture2D texture = LoadingUtils.load<Texture2D>(content, "MainMenu");
			StaticDrawable2DParams parms = new StaticDrawable2DParams {
				Texture = texture,
				Origin = new Vector2(texture.Width / 2, texture.Height / 2),
				Position = new Vector2(Constants.RESOLUTION_X / 2, Constants.RESOLUTION_Y / 8 * 3)
			};
			this.background = new StaticDrawable2D(parms);

			this.effectParms = new PulseEffectParams {
				ScaleBy = 1f,
				ScaleDownTo = .9f,
				ScaleUpTo = 1.1f
			};
			this.index = 0;

			this.menuItems = new StaticDrawable2D[4];
			Vector2 startPosition = new Vector2(parms.Position.X, parms.Position.Y + 250f);

			parms = new StaticDrawable2DParams {
				Origin = new Vector2(128f),
				Scale = DEFAULT_SCALE,
			};
			for (int i = 0; i < this.menuItems.Length; i++) {
				parms.Position = new Vector2(startPosition.X, startPosition.Y + (i * SPACE));
				parms.Texture = LoadingUtils.load<Texture2D>(content, BUTTON_NAMES[i]);

				this.menuItems[i] = new StaticDrawable2D(parms);
			}
			this.menuItems[0].addEffect(new PulseEffect(this.effectParms));
		}
		#endregion Constructor

		#region Support methods
		private void buttonChange(int newIndex) {
			this.menuItems[this.index].Scale = DEFAULT_SCALE;
			this.menuItems[this.index].Effects.RemoveAt(0);
			this.index = newIndex;
			this.menuItems[this.index].addEffect(new PulseEffect(this.effectParms));
		}

		public void update(float elapsed) {
			this.background.update(elapsed);

			int newIndex;
			if (InputManager.getInstance().wasKeyPressed(Keys.Down)) {
				newIndex = (this.index + 1) % this.menuItems.Length;
				buttonChange(newIndex);
			} else if (InputManager.getInstance().wasKeyPressed(Keys.Up)) {
				newIndex = this.index - 1;
				if (newIndex < 0) {
					newIndex = this.menuItems.Length - 1;
				}
				buttonChange(newIndex);
			} else if (InputManager.getInstance().wasKeyPressed(Keys.Enter)) {
				if (this.index == 0) {
					StateManager.getInstance().CurrentGameState = GameState.Init;
					StateManager.getInstance().GameMode = GameMode.OnePlayer;
				} else if (this.index == 1) {
					StateManager.getInstance().CurrentGameState = GameState.Init;
					StateManager.getInstance().GameMode = GameMode.TwoPlayer;
				} else if (this.index == 2) {
					StateManager.getInstance().CurrentGameState = GameState.Options;
				} else {
					StateManager.getInstance().CurrentGameState = GameState.Exit;
				}
			}

			if (this.menuItems != null) {
				foreach (Base2DSpriteDrawable menuItem in this.menuItems) {
					menuItem.update(elapsed);
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.background != null) {
				this.background.render(spriteBatch);
			}

			if (this.menuItems != null) {
				foreach (Base2DSpriteDrawable menuItem in this.menuItems) {
					menuItem.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}
