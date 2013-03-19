
using GWNorthEngine.Input;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class MainMenu : BaseMenu {
		#region Class variables
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
		public MainMenu(ContentManager content): base(content, "MainMenu", new Vector2(Constants.RESOLUTION_X / 2, Constants.RESOLUTION_Y / 8 * 3)) {
			this.effectParms = new PulseEffectParams {
				ScaleBy = 1f,
				ScaleDownTo = .9f,
				ScaleUpTo = 1.1f
			};
			this.index = 0;

			this.menuItems = new StaticDrawable2D[BUTTON_NAMES.Length];
			Vector2 startPosition = new Vector2(Constants.RESOLUTION_X / 2, (Constants.RESOLUTION_Y / 8 * 3) + 250f);

			StaticDrawable2DParams parms = new StaticDrawable2DParams {
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

		public override void update(float elapsed) {
			base.update(elapsed);

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
					StateManager.getInstance().CurrentGameState = GameState.LoadGame;
					StateManager.getInstance().GameMode = GameMode.OnePlayer;
				} else if (this.index == 1) {
					StateManager.getInstance().CurrentGameState = GameState.LoadGame;
					StateManager.getInstance().GameMode = GameMode.TwoPlayer;
				} else if (this.index == 2) {
					StateManager.getInstance().CurrentGameState = GameState.LoadOptions;
				} else {
					StateManager.getInstance().CurrentGameState = GameState.Exit;
				}
			} else if (InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				StateManager.getInstance().CurrentGameState = GameState.Exit;
			}

			if (this.menuItems != null) {
				foreach (Base2DSpriteDrawable menuItem in this.menuItems) {
					menuItem.update(elapsed);
				}
			}
		}

		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);

			if (this.menuItems != null) {
				foreach (Base2DSpriteDrawable menuItem in this.menuItems) {
					menuItem.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}
