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
	public class WallGroup {
		#region Class variables
		private ContentManager content;
		private Random rand;
		private float elapsed;
		private List<Wall> walls;
		private const float SPAWN_INTERVAL = 1000f;
		#endregion Class variables

		#region Class propeties
		#endregion Class properties

		#region Constructor
		public WallGroup(ContentManager content, Random rand) {
			this.content = content;
			this.rand = rand;
			this.walls = new List<Wall>();
		}
		#endregion Constructor

		#region Support methods
		private void create() {
			this.walls.Add(new Wall(this.content, new Vector2(100f)));
			this.walls.Add(new Wall(this.content, new Vector2(100f, 132f)));
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
