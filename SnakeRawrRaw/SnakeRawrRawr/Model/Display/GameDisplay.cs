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

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class GameDisplay {
		#region Class variables
		private Random rand;
		private ContentManager content;
		private BackGround backGround;
		private Snake snake;
		private List<Food> foods;
		private HUD hud;
		private BoundingBox boundary;
#if DEBUG
		public static bool debugOn = false;
		private Texture2D debugLine;
#endif
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public GameDisplay(ContentManager content) {
			this.content = content;
			init(true);
		}
		#endregion Constructor

		#region Support methods
		private void init(bool fullRegen=false) {
			Vector3 min = new Vector3(0, Constants.HUD_OFFSET, 0f);
			Vector3 max = new Vector3(Constants.RESOLUTION_X, Constants.RESOLUTION_Y, 0f);
			this.boundary = new BoundingBox(min, max);
			this.rand = new Random();
			this.snake = new Snake(this.content);
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
				if (InputManager.getInstance().wasKeyPressed(Keys.Space)) {
					StateManager.getInstance().CurrentGameState = GameState.Active;
					if (StateManager.getInstance().PreviousGameState == GameState.GameOver) {
						init(true);
					} else if (StateManager.getInstance().PreviousGameState == GameState.Waiting) {
						init();
					}
				}
			} else if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				if (this.snake != null) {
					this.snake.update(elapsed);
					if (!this.snake.BBox.Intersects(this.boundary) || this.snake.didICollideWithMyself()) {
						StateManager.getInstance().CurrentGameState = GameState.GameOver;
					}
				}
				if (this.foods != null) {
					Food food = null;
					for (int i = 0; i < this.foods.Count; i++) {
						food = this.foods[i];
						if (food != null) {
							food.update(elapsed);
							if (food.wasCollision(this.snake.BBox, this.snake.Heading)) {
								this.snake.eat(food.SpeedMultiplier);
								spawnNode();
								this.hud.Score += food.Points;
								
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
#if DEBUG
			if (InputManager.getInstance().wasKeyPressed(Keys.D)) {
				debugOn = !debugOn;
			} else if (InputManager.getInstance().wasKeyPressed(Keys.A)) {
				this.snake.eat(10f);
			} else if (InputManager.getInstance().wasKeyPressed(Keys.K)) {
				this.foods[0].handleCollision(this.snake.Heading);
			}
#endif
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
			}
			if (this.hud != null) {
				this.hud.render(spriteBatch);
			}
			if (this.foods != null) {
				foreach (Food food in foods) {
					if (food != null) {
						food.render(spriteBatch);
					}
				}
			}
			if (this.snake != null) {
				this.snake.render(spriteBatch);
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
