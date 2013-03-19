using System.Collections.Generic;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model {
	public class Tail : Entity {
		#region Class variables
		private Vector2 heading;
		private List<TargetPosition> targetPositions;
		#endregion Class variables

		#region Class propeties
		public List<TargetPosition> TargetPositions { get { return this.targetPositions; } set { this.targetPositions = value; } }
		public Vector2 Heading { get { return this.heading; } }
		#endregion Class properties

		#region Constructor
		public Tail(ContentManager content, Vector2 position, Vector2 heading)
			: base(content, true) {
			Animated2DSpriteLoadSingleRowBasedOnTexture tailParms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForward;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 4;
			tailParms.Position = position;
			tailParms.Rotation = MathHelper.ToRadians(180);
			tailParms.Texture = LoadingUtils.load<Texture2D>(content, "SnakeTail");
			tailParms.Scale = new Vector2(.5f);
			tailParms.Origin = new Vector2(Constants.TILE_SIZE);
			tailParms.LightColour = Constants.SNAKE_LIGHT;
			tailParms.AnimationParams = animationParms;
			base.init(new Animated2DSprite(tailParms));

			this.heading = heading;
			this.targetPositions = new List<TargetPosition>();
		}
		#endregion Constructor

		#region Support methods
		public bool hasPivot(Vector2 position) {
			bool pivotFound = false;
			foreach (TargetPosition target in this.targetPositions) {
				if (position.Equals(target.Position)) {
					pivotFound = true;
					break;
				}
			}
			return pivotFound;
		}
		
		public void updateMovement(float distance) {
			Vector2 position = base.Position;
			float rotation = base.Rotation;
			PositionUtils.handleChildMovement(distance, ref this.heading, ref position, ref rotation, ref this.targetPositions);

			base.Position = position;
			base.Rotation = rotation;
		}
		#endregion Support methods
	}
}
