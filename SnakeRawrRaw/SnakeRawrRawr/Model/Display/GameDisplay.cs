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
using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model.Display {
	public class GameDisplay : IRenderable {
		#region Class variables
		private Random rand;
		private ContentManager content;
		private BackGround backGround;
		private Snake playerOne;
		private Snake playerTwo;
		private FoodManager foodManager;
		private WallManager walls;
		private PortalManager portals;
		private HUD hud;
		private BoundingBox boundary;
#if DEBUG
		public static bool debugOn = false;
		public static Texture2D radiusTexture;
		private Texture2D debugLine;
		private Texture2D overlayTexture;
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
			this.overlayTexture = LoadingUtils.load<Texture2D>(content, "Overlay");
#endif
		}
		#endregion Constructor

		#region Support methods
		private void init(bool fullRegen=false) {
			Vector3 min = new Vector3(0, Constants.HUD_OFFSET, 0f);
			Vector3 max = new Vector3(Constants.RESOLUTION_X, Constants.RESOLUTION_Y, 0f);
			this.boundary = new BoundingBox(min, max);
			this.rand = new Random();
			PositionGenerator.getInstance().init(this.rand);
			if (StateManager.getInstance().GameMode == GameMode.OnePlayer) {
				this.playerOne = new Snake(this.content, Constants.HEADING_UP, 0f, ConfigurationManager.getInstance().PlayerOnesControls);
			} else {
				this.playerOne = new Snake(this.content, Constants.HEADING_UP, 100f, ConfigurationManager.getInstance().PlayerOnesControls);
				this.playerTwo = new Snake(this.content, Constants.HEADING_UP, -100f, ConfigurationManager.getInstance().PlayerTwosControls);
			}
			
			if (fullRegen) {
				this.backGround = new BackGround(this.content);
				this.hud = new HUD(this.content);
				this.foodManager = new FoodManager(content, this.rand);
				this.portals = new PortalManager(content, this.rand);
				this.walls = new WallManager(content, this.rand);
			}

#if DEBUG
			this.debugLine = LoadingUtils.load<Texture2D>(this.content, "Chip");
#endif
		}

		public void update(float elapsed) {
			if (StateManager.getInstance().CurrentGameState != GameState.Active) {
				if (InputManager.getInstance().wasKeyPressed(Keys.Enter)) {
					if (StateManager.getInstance().CurrentGameState == GameState.GameOver) {
						StateManager.getInstance().CurrentGameState = GameState.LoadGame;
					}
				}
			} else if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				this.playerOne.update(elapsed);
				if (!this.playerOne.BBox.Intersects(this.boundary) || this.playerOne.didICollideWithMyself()) {
					StateManager.getInstance().CurrentGameState = GameState.GameOver;
				}
				List<Vector2> listeners = new List<Vector2> { this.playerOne.Position };

				if (this.playerTwo != null) {
					this.playerTwo.update(elapsed);
					if (!this.playerTwo.BBox.Intersects(this.boundary) || this.playerTwo.didICollideWithMyself() ||
						this.playerTwo.wasCollisionWithBodies(this.playerOne.BBox) || this.playerOne.wasCollisionWithBodies(this.playerTwo.BBox)) {
						StateManager.getInstance().CurrentGameState = GameState.GameOver;
					}
					listeners.Add(this.playerTwo.Position);
				}
				SoundManager.getInstance().update(listeners.ToArray());

				if (this.foodManager != null) {
					this.foodManager.update(elapsed);
					Food food = null;
					for (int i = 0; i < this.foodManager.Foods.Count; i++) {
						food = this.foodManager.Foods[i];
						if (food != null) {
							food.update(elapsed);
							if (food.wasCollision(this.playerOne.BBox, this.playerOne.Heading)) {
								this.playerOne.eat(food.SpeedMultiplier);
								this.hud.PlayerOneScore += food.Points;
							} else if (this.playerTwo != null && food.wasCollision(this.playerTwo.BBox, this.playerTwo.Heading)) {
								this.playerTwo.eat(food.SpeedMultiplier);
								this.hud.PlayerTwoScore += food.Points;
							}
							if (food.Release) {
								this.foodManager.Foods[i] = null;
								this.foodManager.Foods.RemoveAt(i);
								i--;
							}
						}
					}
				}

				if (this.portals != null) {
					this.portals.update(elapsed);
					if (this.portals.wasCollision(this.playerOne.BBox, this.playerOne.Position)) {
						this.playerOne.handlePortalCollision(this.portals.WarpCoords);
						this.hud.PlayerOneScore += this.portals.Points;
					}
					this.portals.wasClosingCollision(this.playerOne.TailsBBox);
					if (this.playerTwo != null) {
						if (this.portals.wasCollision(this.playerTwo.BBox, this.playerTwo.Position)) {
							this.playerTwo.handlePortalCollision(this.portals.WarpCoords);
							this.hud.PlayerTwoScore += this.portals.Points;
						}
						this.portals.wasClosingCollision(this.playerTwo.TailsBBox);
					}
				}

				if (this.walls != null) {
					this.walls.update(elapsed);
					if (this.walls.wasCollision(this.playerOne.BBox)) {
						StateManager.getInstance().CurrentGameState = GameState.GameOver;
					} else {
						if (this.playerTwo != null && this.walls.wasCollision(this.playerTwo.BBox)) {
							StateManager.getInstance().CurrentGameState = GameState.GameOver;
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
				this.playerOne.eat(1f);
			}
#endif
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
			}

			if (this.foodManager != null) {
				this.foodManager.render(spriteBatch);
			}
			if (this.portals != null) {
				this.portals.render(spriteBatch);
			}
			if (this.playerOne != null) {
				this.playerOne.render(spriteBatch);
			}
			if (this.playerTwo != null) {
				this.playerTwo.render(spriteBatch);
			}

			if (this.walls != null) {
				this.walls.render(spriteBatch);
			}

			if (this.hud != null) {
				this.hud.render(spriteBatch);
			}
#if DEBUG
			if (GameDisplay.debugOn) {
				DebugUtils.drawBoundingBox(spriteBatch, this.boundary, Constants.DEBUG_BBOX_Color, debugLine);
				Vector2 position;
				for (int y = 0; y <= PositionGenerator.getInstance().Layout.GetUpperBound(0); y++) {
					for (int x = 0; x <= PositionGenerator.getInstance().Layout.GetUpperBound(1); x++) {
						position = new Vector2(PositionGenerator.GRID_PIECE_SIZE * x, PositionGenerator.GRID_PIECE_SIZE * y);
						if (PositionGenerator.getInstance().Layout[y, x]) {
							spriteBatch.Draw(this.overlayTexture, position, Color.Gray);
						} else {
							spriteBatch.Draw(this.overlayTexture, position, Color.Red);
						}
					}
				}
			}
#endif
		}
		#endregion Support methods
	}
}
