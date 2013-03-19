
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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
