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

namespace SnakeRawrRawr.Model {
	public class Snake : Entity {
		#region Class variables
		private Tail tail;
		private Body body;
		private Vector2 heading;
		private List<PivotPoint> pivotPoints;
		private StaticDrawable2DParams cornerParms;
		private List<StaticDrawable2D> corners;
		private float currentSpeed;
		private Controls controls;
#if DEBUG
		private List<StaticDrawable2D> debugPivotPoints;
		private StaticDrawable2DParams pivotParms;
#endif
		private const float STARTING_SPEED = 200f;
		#endregion Class variables

		#region Class propeties
		public Vector2 Heading { get { return this.heading; } }
		#endregion Class properties

		#region Constructor
		public Snake(ContentManager content, Vector2 heading, float xOffSet, Controls controls) : base(content) {
			this.pivotPoints = new List<PivotPoint>();
			this.heading = heading;
			this.currentSpeed = STARTING_SPEED;
			this.controls = controls;

			Animated2DSpriteLoadSingleRowBasedOnTexture headParms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForward;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 4;
			headParms.Position = new Vector2(xOffSet + PositionUtils.getPosition(Constants.MAX_X_TILES / 2), PositionUtils.getPosition(Constants.MAX_Y_TILES / 2 - 1));
			headParms.Texture = LoadingUtils.load<Texture2D>(content, "SnakeHead");
			headParms.Scale = new Vector2(.5f);
			headParms.Origin = new Vector2(Constants.TILE_SIZE);
			headParms.LightColour = Constants.SNAKE_LIGHT;
			headParms.AnimationParams = animationParms;
			base.init(new Animated2DSprite(headParms));

			this.body = new Body(content, new Vector2(headParms.Position.X, headParms.Position.Y + Constants.TILE_SIZE - Constants.OVERLAP), this.heading);

			this.tail = new Tail(content, new Vector2(headParms.Position.X, headParms.Position.Y + (Constants.TILE_SIZE - Constants.OVERLAP) * 2), this.heading);
			this.cornerParms = new StaticDrawable2DParams {
				Texture = LoadingUtils.load<Texture2D>(content, "SnakeBody"),
				Origin = new Vector2(Constants.TILE_SIZE / 2),
				Scale = new Vector2(.8f),
			};
			this.corners = new List<StaticDrawable2D>();
#if DEBUG
			this.debugPivotPoints = new List<StaticDrawable2D>();
			this.pivotParms = new StaticDrawable2DParams();
			this.pivotParms.Texture = LoadingUtils.load<Texture2D>(content, "Chip");
			this.pivotParms.LightColour = Constants.DEBUG_PIVOT_Color;
			this.pivotParms.Scale = new Vector2(4f);
#endif
		}
		#endregion Constructor

		#region Support methods
		private void createPivotPoint(float rotation) {
			base.Rotation += MathHelper.ToRadians(rotation);
			PivotPoint pivot = new PivotPoint { Heading = this.heading, Position = base.Position, Rotation = rotation };
			this.pivotPoints.Add(pivot);
			this.body.addPivotPoint(pivot);
			this.tail.PivotPoints.Add(pivot);
			this.cornerParms.Position = base.Position;
			this.cornerParms.Rotation += MathHelper.ToRadians(rotation);
			this.corners.Add(new StaticDrawable2D(this.cornerParms));
#if DEBUG
			this.pivotParms.Position = base.Position;
			this.debugPivotPoints.Add(new StaticDrawable2D(this.pivotParms));
#endif
		}

		private void updateMovement(float elapsed) {
			float distance = (this.currentSpeed / 1000) * elapsed;

			base.Position += PositionUtils.getDelta(this.heading, distance);
			this.body.updateMovement(distance);
			this.body.update(elapsed);
			this.tail.updateMovement(distance);
			this.tail.update(elapsed);
			for (int i = 0; i < this.corners.Count; i++) {
				if (!this.tail.hasPivot(this.corners[i].Position)) {
					this.corners.RemoveAt(i);
					i--;
				}
			}
		}

		private void handleInput() {
			// get key press for new direction and log the pivot point
			Vector2 previousHeading = this.heading;
			if (InputManager.getInstance().wasKeyPressed(controls.Left) && this.heading != Constants.HEADING_RIGHT && this.heading != Constants.HEADING_LEFT) {
				this.heading = Constants.HEADING_LEFT;
				createPivotPoint(PositionUtils.getRotation(previousHeading, this.heading));
			} else if (InputManager.getInstance().wasKeyPressed(controls.Right) && this.heading != Constants.HEADING_LEFT && this.heading != Constants.HEADING_RIGHT) {
				this.heading = Constants.HEADING_RIGHT;
				createPivotPoint(PositionUtils.getRotation(previousHeading, this.heading));
			} else if (InputManager.getInstance().wasKeyPressed(controls.Up) && this.heading != Constants.HEADING_DOWN && this.heading != Constants.HEADING_UP) {
				this.heading = Constants.HEADING_UP;
				createPivotPoint(PositionUtils.getRotation(previousHeading, this.heading));
			} else if (InputManager.getInstance().wasKeyPressed(controls.Down) && this.heading != Constants.HEADING_UP && this.heading != Constants.HEADING_DOWN) {
				this.heading = Constants.HEADING_DOWN;
				createPivotPoint(PositionUtils.getRotation(previousHeading, this.heading));
			}
		}

		protected override BoundingBox getBBox() {
			float quarter = Constants.TILE_SIZE / 4f;
			return new BoundingBox(
				new Vector3(Vector2.Subtract(this.Position, new Vector2(quarter)), 0f),
				new Vector3(Vector2.Add(this.Position, new Vector2(quarter)), 0f));
		}

		public bool wasCollisionWithBodies(BoundingBox bbox) {
			bool collision = false;
			if (body != null && body.Child != null) {
				Body node = this.body.Child;
				while (node != null) {
					// was there a collision with our body?
					if (bbox.Intersects(node.BBox)) {
						collision = true;
						break;
					}
					node = node.Child;
				}
			}
			return collision;
		}

		public bool didICollideWithMyself() {
			return wasCollisionWithBodies(this.BBox);
		}

		public void eat(float speedMultiplier) {
			if (this.body != null) {
				Body node = this.body;
				while (node != null) {
					// did we find our last link?
					if (node.Child == null) {
						 break;
					}
					node = node.Child;
				}
				// create our new link
				List<PivotPoint> clonedPivots = new List<PivotPoint>();
				clonedPivots.AddRange(this.tail.PivotPoints);
				node.Child = new Body(this.content, this.tail.Position, this.tail.Heading, this.tail.Rotation, clonedPivots);
				// we need to push the tail back
				Vector2 delta = PositionUtils.getDelta(-this.tail.Heading, Constants.TILE_SIZE - Constants.OVERLAP);
				this.tail.Position += delta;
				// increase our speed
				this.currentSpeed += speedMultiplier;
			}
		}

		public override void update(float elapsed) {
			base.update(elapsed);
			// update movement
			updateMovement(elapsed);
			handleInput();
		}

		public override void render(SpriteBatch spriteBatch) {
			if (this.corners != null) {
				foreach (StaticDrawable2D corner in this.corners) {
					corner.render(spriteBatch);
				}
			}
			base.render(spriteBatch);
			if (this.body != null) {
				this.body.render(spriteBatch);
			}
			if (this.tail != null) {
				this.tail.render(spriteBatch);
			}
#if DEBUG
			if (Model.Display.GameDisplay.debugOn) {
				foreach (StaticDrawable2D pivot in this.debugPivotPoints) {
					pivot.render(spriteBatch);
				}
			}
#endif
		}
		#endregion Support methods
	}
}
