using System.Collections.Generic;
using GWNorthEngine.Logic;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeRawrRawr.Logic;
using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public class Body : Entity {
		#region Class variables
		private Body child;
		private Vector2 heading;
		private List<TargetPosition> targetPositions;
		private PulseDirection pulseDirection;
		private const float PULSE_BY = .02f;
		private const float PULSE_UP = 1.3f;
		private const float PULSE_DOWN = 1f;
		#endregion Class variables

		#region Class propeties
		public Body Child { get { return this.child; } set { this.child = value; } }
		#endregion Class properties

		#region Constructor
		public Body(ContentManager content, Vector2 position, Vector2 heading) : this(content, position, heading, 0f, new List<TargetPosition>()) { }

		public Body(ContentManager content, Vector2 position, Vector2 heading, float rotation, List<TargetPosition> targetPositions)
			: base(content, true) {
			StaticDrawable2DParams bodyParms = new StaticDrawable2DParams();
			bodyParms.Texture = LoadingUtils.load<Texture2D>(content, "Snakebody");
			bodyParms.Position = position;
			bodyParms.Rotation = rotation;
			bodyParms.Origin = new Vector2(Constants.TILE_SIZE / 2);
			bodyParms.LightColour = Constants.SNAKE_LIGHT;
			base.init(new StaticDrawable2D(bodyParms));

			this.heading = heading;
			this.targetPositions = targetPositions;
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

		public void addTargetPosition(TargetPosition targetPosition) {
			this.targetPositions.Add(targetPosition);
			if (this.child != null) {
				this.child.addTargetPosition(targetPosition);
			}
		}

		public void updateMovement(float distance) {
			Vector2 currentPosition = base.Position;
			Vector2 position = base.Position;
			float rotation = base.Rotation;
			PositionUtils.handleChildMovement(distance, ref this.heading, ref position, ref rotation, ref this.targetPositions);

			base.Position = position;
			base.Rotation = rotation;
			PositionGenerator.getInstance().updateLocation(currentPosition, position);

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
