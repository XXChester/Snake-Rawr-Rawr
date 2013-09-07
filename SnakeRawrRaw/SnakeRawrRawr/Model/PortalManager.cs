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
		public Portal One { get; set; }
		public Portal Two { get; set; }
		public WarpCoordinates WarpCoords { get; set; }
		public int Points { get { return POINTS; } }
		#endregion Class properties

		#region Constructor
		public PortalManager(ContentManager content, Random rand) : base(content, rand, SPAWN_INTERVAL) {
		}
		#endregion Constructor

		#region Support methods
		protected override void create() {
			this.One = new Portal(this.content, this.rand);
			this.Two = new Portal(this.content, this.rand);
			base.nodes.Add(this.One);
			base.nodes.Add(this.Two);
			base.create();
		}

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
			if (this.One != null) {
				this.One.update(elapsed);
				if (this.One.Release) {
					this.One = null;
				}
			}
			if (this.Two != null) {
				this.Two.update(elapsed);
				if (this.Two.Release) {
					this.Two = null;
				}
			}
			if (this.One == null && this.Two == null) {
				this.WarpCoords = null;
				base.update(elapsed);
			}
		}
		#endregion Support methods
	}
}
