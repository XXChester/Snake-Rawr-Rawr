using System;
using System.Collections.Generic;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeRawrRawr.Model {
	public class BackGround {
		#region Class variables
		private StaticDrawable2D backGround;
		private List<StaticDrawable2D> grassBlades;
		private const int GRASS_BLADES = 500;
		private static Color COLOUR = Color.DarkGreen;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public BackGround(ContentManager content) {
			StaticDrawable2DParams backGroundParms = new StaticDrawable2DParams();
			backGroundParms.Position = new Vector2(0f, Constants.HUD_OFFSET);
			backGroundParms.Texture = LoadingUtils.load<Texture2D>(content, "BackGround1");
			backGroundParms.LightColour = COLOUR;
			this.backGround = new StaticDrawable2D(backGroundParms);

			this.grassBlades = new List<StaticDrawable2D>();
			Texture2D grass1Texture = LoadingUtils.load<Texture2D>(content, "Grass1");
			StaticDrawable2DParams grassParms = new StaticDrawable2DParams();
			grassParms.Texture = grass1Texture;

			List<Point> usedIndexes = new List<Point>();
			Point point;
			Random rand = new Random();
			for (int i = 0; i < GRASS_BLADES; i++) {
				point = new Point(rand.Next(Constants.MAX_X_TILES), rand.Next(Constants.MAX_Y_TILES));
				if (usedIndexes.Contains(point)) {
					i--;
					continue;
				} else {
					usedIndexes.Add(point);
					grassParms.Position = new Vector2(point.X * Constants.TILE_SIZE, (point.Y * Constants.TILE_SIZE) + Constants.HUD_OFFSET);
					this.grassBlades.Add(new StaticDrawable2D(grassParms));
				}
				
			}
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {

		}

		public void render(SpriteBatch spriteBatch) {
			if (this.backGround != null) {
				this.backGround.render(spriteBatch);
			}
			if (this.grassBlades != null) {
				foreach (StaticDrawable2D grassBlade in this.grassBlades) {
					grassBlade.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}