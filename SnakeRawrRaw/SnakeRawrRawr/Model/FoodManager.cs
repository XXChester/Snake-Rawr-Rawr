using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace SnakeRawrRawr.Model {
	public class FoodManager : BaseManager {
		#region Class variables
		private const int BURST_CHANCE = 10; 
		private const float SPAWN_INTERVAL = 3000f;
		#endregion Class variables

		#region Class propeties
		
		#endregion Class properties

		#region Constructor
		public FoodManager(ContentManager content, Random rand) : base(content, rand, SPAWN_INTERVAL) {
			// initially create a couple nodes around the center
			do {
				base.nodes.Add(new Chicken(this.content, this.rand));
			} while (base.nodes.Count < 2);
			base.spawnHandler = delegate() {
				int spawn = 1;
				if (this.rand.Next(BURST_CHANCE) % BURST_CHANCE == 0) {
					spawn = 3;
				}
				do {
					if (this.rand.Next(Constants.RARE_SPAWN_ODDS) % Constants.RARE_SPAWN_ODDS == 0) {
						base.nodes.Add(new Carebear(this.content, this.rand));
					} else {
						base.nodes.Add(new Chicken(this.content, this.rand));
					}
					spawn--;
				} while (spawn > 0);
			};
		}
		#endregion Constructor

		#region Support methods
		public override bool wasCollision(BoundingBox bbox) {
			bool collision = false;
			if (base.nodes != null) {
				foreach (Food food in base.nodes) {
					if (food.BBox.Intersects(bbox)) {
						collision = true;
						break;
					}
				}
			}
			return collision;
		}
		#endregion Support methods
	}
}
