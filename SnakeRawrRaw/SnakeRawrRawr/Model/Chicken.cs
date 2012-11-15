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
	public class Chicken : Food {
		#region Class variables
		private const int VALUE = 5;
		private const float SPEED_MULTIPLIER = 10f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Chicken(ContentManager content, Random rand) : base(content, VALUE, SPEED_MULTIPLIER) {
			StaticDrawable2DParams parms = new StaticDrawable2DParams();
			parms.Texture = LoadingUtils.load<Texture2D>(content, "Chicken");
			parms.Origin = new Vector2(Constants.TILE_SIZE / 2);
			parms.Position = new Vector2(PositionUtils.getPosition(rand.Next(1, Constants.MAX_X_TILES - 1)), 
				PositionUtils.getPosition(rand.Next(1, Constants.MAX_Y_TILES - 1)) + Constants.HUD_OFFSET + Constants.TILE_SIZE / 2 + Constants.OVERLAP);
			base.init(new StaticDrawable2D(parms));
		}
		#endregion Constructor

		#region Support methods
		public override void handleCollision(Vector2 heading) {
			throw new NotImplementedException();
		}
		#endregion Support methods
	}
}
