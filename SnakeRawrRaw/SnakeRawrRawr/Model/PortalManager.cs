using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model {
	public class PortalManager : BaseManager {
		#region Class variables
		private const float SPAWN_INTERVAL = 5000f;
		private const int POINTS = 15;
		#endregion Class variables

		#region Class propeties
		public Portal One {
			get {
				Portal node = null;
				if (base.nodes != null && base.nodes.Count > 0) {
					node = (Portal)base.nodes[0];
				}
				return node;
			}
		}
		public Portal Two {
			get {
				Portal node = null;
				if (base.nodes != null && base.nodes.Count > 1) {
					node = (Portal)base.nodes[1];
				}
				return node;
			}
		}
		public WarpCoordinates WarpCoords { get; set; }
		public int Points { get { return POINTS; } }
		#endregion Class properties

		#region Constructor
		public PortalManager(ContentManager content, Random rand) : base(content, rand, SPAWN_INTERVAL) {
			base.spawnHandler = delegate() {
				this.nodes.Add(new Portal(base.content, base.rand));
				this.nodes.Add(new Portal(base.content, base.rand));
			};
		}
		#endregion Constructor

		#region Support methods
		public bool wasCollision(BoundingBox bbox, Vector2 snakesPosition) {
			bool collision = false;
			if (this.One != null) {
				if (this.One.wasCollision(bbox)) {
					collision = true;
					if (this.Two != null) {
						this.WarpCoords = new WarpCoordinates(snakesPosition, this.Two.Position);
						this.Two.LifeStage = Portal.Stage.Exit;
					}
				}
			}
			if (this.Two != null) {
				if (this.Two.wasCollision(bbox)) {
					collision = true;
					if (this.One != null) {
						this.WarpCoords = new WarpCoordinates(snakesPosition, this.One.Position);
						this.One.LifeStage = Portal.Stage.Exit;
					}
				}
			}
			return collision;
		}

		public override bool wasCollision(BoundingBox tailBBox) {
			bool collision = false;
			if (this.One != null) {
				if (this.One.wasTailCollision(tailBBox)) {
					collision = true;
				}
			}
			if (this.Two != null) {
				if (this.Two.wasTailCollision(tailBBox)) {
					collision = true;
				}
			}
			return collision;
		}

		public override void update(float elapsed) {
			Entity node = null;
			for (int i = base.nodes.Count - 1; i >= 0; i--) {
				node = base.nodes[i];
				node.update(elapsed);
				if (((Portal)node).Release) {
					base.nodes.RemoveAt(i);
				}
			}
			if (base.nodes.Count == 0) {
				this.WarpCoords = null;
				base.update(elapsed);
			}
		}
		#endregion Support methods
	}
}
