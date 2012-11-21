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
	public class OptionsMenu : BaseMenu {
		#region Class variables
		private OptionsSection playerOneSection;
		private OptionsSection playerTwoSection;
		private const float SPACE = 65f;
		private readonly string[] BUTTON_NAMES = { "SaveAndReturn", "Save", "ReturnToMain" };
		private readonly Color DEFAULT = Color.Red;
		private readonly Vector2 DEFAULT_SCALE = new Vector2(1f, .75f);
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public OptionsMenu(ContentManager content) :base(content, "OptionsMenu", new Vector2(Constants.RESOLUTION_X / 2, 100f)) {
			float x = Constants.RESOLUTION_X / 10;
			this.playerOneSection = new OptionsSection(content, new Vector2(x, 400f), "One");
			this.playerTwoSection = new OptionsSection(content, new Vector2(x * 6, 400f), "Two");
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			base.update(elapsed);
			this.playerOneSection.update(elapsed);
			this.playerTwoSection.update(elapsed);

			if (!this.playerOneSection.Binding && !this.playerTwoSection.Binding && InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				//StateManager.getInstance().CurrentGameState = GameState.MainMenu;
				StateManager.getInstance().CurrentGameState = GameState.Exit;
			}
		}

		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);
			this.playerOneSection.render(spriteBatch);
			this.playerTwoSection.render(spriteBatch);
		}
		#endregion Support methods
	}
}
