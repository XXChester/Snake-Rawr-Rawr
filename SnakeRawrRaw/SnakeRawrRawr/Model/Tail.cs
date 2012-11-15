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
	public class Tail : Entity {
		#region Class variables
		private Vector2 heading;
		private List<PivotPoint> pivotPoints;
		#endregion Class variables

		#region Class propeties
		public List<PivotPoint> PivotPoints { get { return this.pivotPoints; } set { this.pivotPoints = value; } }
		public Vector2 Headind { get { return this.heading; } }
		#endregion Class properties

		#region Constructor
		public Tail(ContentManager content, Vector2 position, Vector2 heading) :base(content) {
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
			tailParms.AnimationParams = animationParms;
			base.init(new Animated2DSprite(tailParms));

			this.heading = heading;
			this.PivotPoints = new List<PivotPoint>();
		}
		#endregion Constructor

		#region Support methods
		public void updateMovement(float distance) {
			Vector2 position = base.Position;
			float rotation = base.Rotation;
			PositionUtils.handleChildMovement(distance, ref this.heading, ref position, ref rotation, ref this.pivotPoints);

			base.Position = position;
			base.Rotation = rotation;
		}
		#endregion Support methods
	}
}
