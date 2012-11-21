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
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Engine;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class GameDisplay : IRenderable {
		#region Class variables
		private Random rand;
		private ContentManager content;
		private BackGround backGround;
		private Snake playerOne;
		private Snake playerTwo;
		private List<Food> foods;
		private HUD hud;
		private BoundingBox boundary;
#if DEBUG
		public static bool debugOn = false;
		public static Texture2D radiusTexture;
		private Texture2D debugLine;
#endif
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public GameDisplay(GraphicsDevice graphics, ContentManager content) {
			this.content = content;
			init(true);
#if DEBUG
			radiusTexture = TextureUtils.create2DRingTexture(graphics, 100, Color.White);
#endif
		}
		#endregion Constructor

		#region Support methods
		private void init(bool fullRegen=false) {
			Vector3 min = new Vector3(0, Constants.HUD_OFFSET, 0f);
			Vector3 max = new Vector3(Constants.RESOLUTION_X, Constants.RESOLUTION_Y, 0f);
			this.boundary = new BoundingBox(min, max);
			this.rand = new Random();
			if (StateManager.getInstance().GameMode == GameMode.OnePlayer) {
				this.playerOne = new Snake(this.content, Constants.HEADING_UP, 0f, ConfigurationManager.getInstance().PlayerOnesControls);
			} else {
				this.playerOne = new Snake(this.content, Constants.HEADING_UP, 100f, ConfigurationManager.getInstance().PlayerOnesControls);
				this.playerTwo = new Snake(this.content, Constants.HEADING_UP, -100f, ConfigurationManager.getInstance().PlayerTwosControls);
			}
			
			if (fullRegen) {
				this.backGround = new BackGround(this.content);
				this.hud = new HUD(this.content);
				this.foods = new List<Food>();
				for (int i = 0; i < Constants.EDIBLE_NODES; i++) {
					spawnNode();
				}
			}

#if DEBUG
			this.debugLine = LoadingUtils.load<Texture2D>(this.content, "Chip");
#endif
		}

		private void spawnNode() {
			if (this.rand.Next(Constants.RARE_SPAWN_ODDS) % Constants.RARE_SPAWN_ODDS == 0) {
				this.foods.Add(new Carebear(this.content, this.rand));
			} else {
				this.foods.Add(new Chicken(this.content, this.rand));
			}
		}

		public void update(float elapsed) {
			if (StateManager.getInstance().CurrentGameState != GameState.Active) {
				if (InputManager.getInstance().wasKeyPressed(Keys.Enter)) {
					if (StateManager.getInstance().CurrentGameState == GameState.GameOver) {
						StateManager.getInstance().CurrentGameState = GameState.Init;
					}
				}
			} else if (StateManager.getInstance().CurrentGameState == GameState.Active) {
					this.playerOne.update(elapsed);
					if (!this.playerOne.BBox.Intersects(this.boundary) || this.playerOne.didICollideWithMyself()) {
						StateManager.getInstance().CurrentGameState = GameState.GameOver;
					}
					SoundManager.getInstance().update(this.playerOne.Position);

				if (this.playerTwo != null) {
					this.playerTwo.update(elapsed);
					if (!this.playerTwo.BBox.Intersects(this.boundary) || this.playerTwo.didICollideWithMyself() ||
						this.playerTwo.wasCollisionWithBodies(this.playerOne.BBox) || this.playerOne.wasCollisionWithBodies(this.playerTwo.BBox)) {
						StateManager.getInstance().CurrentGameState = GameState.GameOver;
					}
					SoundManager.getInstance().update(this.playerTwo.Position);
				}
				
				if (this.foods != null) {
					Food food = null;
					for (int i = 0; i < this.foods.Count; i++) {
						food = this.foods[i];
						if (food != null) {
							food.update(elapsed);
							if (food.wasCollision(this.playerOne.BBox, this.playerOne.Heading)) {
								this.playerOne.eat(food.SpeedMultiplier);
								spawnNode();
								this.hud.PlayerOneScore += food.Points;

							} else if (this.playerTwo != null && food.wasCollision(this.playerTwo.BBox, this.playerTwo.Heading)) {
								this.playerTwo.eat(food.SpeedMultiplier);
								spawnNode();
								this.hud.PlayerTwoScore += food.Points;

							}
							if (food.Release) {
								this.foods[i] = null;
								this.foods.RemoveAt(i);
								i--;
							}
						}
					}
				}
			}

			if (this.hud != null) {
				this.hud.update(elapsed);
			}

			if (InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				StateManager.getInstance().CurrentGameState = GameState.MainMenu;
			}
#if DEBUG
			if (InputManager.getInstance().wasKeyPressed(Keys.D1)) {
				debugOn = !debugOn;
			} else if (InputManager.getInstance().wasKeyPressed(Keys.D2)) {
				this.playerOne.eat(10f);
			} else if (InputManager.getInstance().wasKeyPressed(Keys.K)) {
				this.foods[0].handleCollision(this.playerOne.Heading);
			}
#endif
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
			}
			if (this.foods != null) {
				foreach (Food food in foods) {
					if (food != null) {
						food.render(spriteBatch);
					}
				}
			}
			if (this.playerOne != null) {
				this.playerOne.render(spriteBatch);
			}
			if (this.playerTwo != null) {
				this.playerTwo.render(spriteBatch);
			}

			if (this.hud != null) {
				this.hud.render(spriteBatch);
			}
#if DEBUG
			if (GameDisplay.debugOn) {
				DebugUtils.drawBoundingBox(spriteBatch, this.boundary, Constants.DEBUG_BBOX_Color, debugLine);
			}
#endif
		}
		#endregion Support methods
	}
}
