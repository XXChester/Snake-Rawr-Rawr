using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public class WallManager {
		#region Class variables
		private ContentManager content;
		private Random rand;
		private float elapsed;
		private List<Wall> walls;
		private const float SPAWN_INTERVAL = 6000f;
		private readonly List<BaseWallGenerator> GENERATORS;
		#endregion Class variables

		#region Class propeties
		#endregion Class properties

		#region Constructor
		public WallManager(ContentManager content, Random rand) {
			this.content = content;
			this.rand = rand;
			this.GENERATORS = new List<BaseWallGenerator>() {
				new ScatteredWallGenerator(rand),
				new HorizontalWallGenerator(rand),
				new VerticalWallGenerator(rand),
				new CrossWallGenerator(rand),
				new RightAngleWallGenerator(rand),
			};
			this.walls = new List<Wall>();
		}
		#endregion Constructor

		#region Support methods
		private void create() {
			List<Vector2> positions = this.GENERATORS[this.rand.Next(0, this.GENERATORS.Count - 1)].generate();
			foreach (Vector2 position in positions) {
				this.walls.Add(new Wall(this.content, position));
			}
			this.elapsed = 0f;
		}

		public bool wasCollision(BoundingBox bbox) {
			bool collision = false;
			if (this.walls != null) {
				foreach (Wall wall in this.walls) {
					if (wall.BBox.Intersects(bbox)) {
						collision = true;
						break;
					}
				}
			}
			return collision;
		}

		public void update(float elapsed) {
			Wall wall = null;
			for(int i = this.walls.Count - 1; i >= 0; i--) {
				wall = this.walls[i];
				if (wall != null) {
					wall.update(elapsed);
					if (wall.Release) {
						this.walls.RemoveAt(i);
					}
				}
			}

			this.elapsed += elapsed;
			if (this.elapsed >= SPAWN_INTERVAL) {
				create();
			}
		}

		public void render(SpriteBatch spriteBatch) {
			foreach (Wall wall in this.walls) {
				wall.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
