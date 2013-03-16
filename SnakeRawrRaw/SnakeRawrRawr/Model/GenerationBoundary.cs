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

namespace SnakeRawrRawr.Model {
	public class GenerationBoundary : Entity {
		#region Constructor
		public GenerationBoundary(ContentManager content, Vector2 position, float rotation=0f) : base(content, true) {
			StaticDrawable2DParams boundaryParam = new StaticDrawable2DParams {
				Texture = LoadingUtils.load<Texture2D>(this.content, "Chip"),
				LightColour = new Color(100f, 0f, 0f),
				Scale = new Vector2(Constants.RESOLUTION_X, Constants.GENERATION_Y_SIZE),
				Rotation = MathHelper.ToRadians(rotation),
				Position = position
			};
			base.init(new StaticDrawable2D(boundaryParam));
		}
		#endregion Constructor
	}
}
