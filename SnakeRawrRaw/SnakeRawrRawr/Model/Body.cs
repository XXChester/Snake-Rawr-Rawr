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
	public class Body : Entity {
		#region Class variables
		private Body child;
		private Vector2 heading;
		private List<PivotPoint> pivotPoints;
		private WarpCoordinates warpCoords;
		private PulseDirection pulseDirection;
		private const float PULSE_BY = .02f;
		private const float PULSE_UP = 1.3f;
		private const float PULSE_DOWN = 1f;
		#endregion Class variables

		#region Class propeties
		public Body Child { get { return this.child; } set { this.child = value; } }
		#endregion Class properties

		#region Constructor
		public Body(ContentManager content, Vector2 position, Vector2 heading) :this(content, position, heading, 0f, new List<PivotPoint>()) { }

		public Body(ContentManager content, Vector2 position, Vector2 heading, float rotation, List<PivotPoint> pivotPoints)
			: base(content, true) {
			StaticDrawable2DParams bodyParms = new StaticDrawable2DParams();
			bodyParms.Texture = LoadingUtils.load<Texture2D>(content, "Snakebody");
			bodyParms.Position = position;
			bodyParms.Rotation = rotation;
			bodyParms.Origin = new Vector2(Constants.TILE_SIZE / 2);
			bodyParms.LightColour = Constants.SNAKE_LIGHT;
			base.init(new StaticDrawable2D(bodyParms));

			this.heading = heading;
			this.pivotPoints = pivotPoints;
			this.pulseDirection = PulseDirection.Up;
		}
		#endregion Constructor

		#region Support methods
		private void updatePulse(float newScale) {
			float myPreviousScale = base.Scale.X;
			base.Scale = new Vector2(newScale, base.Scale.Y);
			if (this.child != null) {
				this.child.updatePulse(myPreviousScale);
			}
		}

		public void addPivotPoint(PivotPoint pivotPoint) {
			this.pivotPoints.Add(pivotPoint);
			if (this.child != null) {
				this.child.addPivotPoint(pivotPoint);
			}
		}

		public void addWarpPosition(WarpCoordinates warpCoords) {
			this.warpCoords = warpCoords;
			if (this.child != null) {
				this.child.addWarpPosition(warpCoords);
			}
		}

		public void updateMovement(float distance) {
			Vector2 position = base.Position;
			float rotation = base.Rotation;
			PositionUtils.handleChildMovement(distance, ref this.heading, ref position, ref rotation, ref this.pivotPoints, ref warpCoords);

			base.Position = position;
			base.Rotation = rotation;

			if (this.child != null) {
				this.child.updateMovement(distance);
			}
		}

		public override void update(float elapsed) {
			float newScale;
			if (this.pulseDirection == PulseDirection.Up) {
				newScale = base.Scale.X + PULSE_BY;
				if (newScale >= PULSE_UP) {
					this.pulseDirection = PulseDirection.Down;
				}
			} else {
				newScale = base.Scale.X - PULSE_BY;
				if (newScale <= PULSE_DOWN) {
					this.pulseDirection = PulseDirection.Up;
				}
			}
			updatePulse(newScale);
		}

		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);
			if (this.child != null) {
				this.child.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
