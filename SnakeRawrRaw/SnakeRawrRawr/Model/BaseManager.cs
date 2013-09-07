using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public abstract class BaseManager {
		#region Class variables
		private float elapsed;
		protected ContentManager content;
		protected Random rand;
		protected List<Entity> nodes;
		protected SpawnGenerator.HandleSpawn spawnHandler;
		private readonly float SPAWN_INTERVAL;
		#endregion Class variables

		#region Class properties
		public List<Entity> Nodes { get { return this.nodes; } }
		#endregion Class properties

		#region Constructor
		public BaseManager(ContentManager content, Random rand, float spawnInterval) {
			this.content = content;
			this.rand = rand;
			this.SPAWN_INTERVAL = spawnInterval;
			this.nodes = new List<Entity>();
		}
		#endregion Constructor

		#region Support methods
		protected virtual void create() {
			this.elapsed = 0f;
			if (this.spawnHandler != null) {
				SpawnGenerator.getInstance().SpawnRequests.Enqueue(this.spawnHandler);
			}
		}

		public virtual bool wasCollision(BoundingBox bbox) {
			bool collision = false;
			lock (this.nodes) {
				if (this.nodes != null) {
					foreach (Entity node in this.nodes) {
						if (node.BBox.Intersects(bbox)) {
							collision = true;
							break;
						}
					}
				}
			}
			return collision;
		}

		public virtual void update(float elapsed) {
			this.elapsed += elapsed;
			if (this.elapsed >= SPAWN_INTERVAL) {
				create();
			}
		}

		public virtual void render(SpriteBatch spriteBatch) {
			lock (this.nodes) {
				foreach (Entity node in this.nodes) {
					node.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}
