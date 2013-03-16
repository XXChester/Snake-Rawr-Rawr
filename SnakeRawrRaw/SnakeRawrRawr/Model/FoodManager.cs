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
using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public class FoodManager {
		#region Class variables
		private ContentManager content;
		private Random rand;
		private float elapsed;
		private List<Food> foods;
		private const int BURST_CHANCE = 10; 
		private const float SPAWN_INTERVAL = 3000f;
		#endregion Class variables

		#region Class propeties
		public List<Food> Foods { get { return this.foods; } }
		#endregion Class properties

		#region Constructor
		public FoodManager(ContentManager content, Random rand) {
			this.content = content;
			this.rand = rand;
			this.foods = new List<Food>();
			// initially create a couple nodes around the center
			do {
				this.foods.Add(new Chicken(this.content, this.rand));
			} while (this.foods.Count < 2);
		}
		#endregion Constructor

		#region Support methods
		private void create() {
			int spawn = 1;
			if (this.rand.Next(BURST_CHANCE) % BURST_CHANCE == 0) {
				spawn = 3;
			}
			do {
				if (this.rand.Next(Constants.RARE_SPAWN_ODDS) % Constants.RARE_SPAWN_ODDS == 0) {
					this.foods.Add(new Carebear(this.content, this.rand));
				} else {
					this.foods.Add(new Chicken(this.content, this.rand));
				}
				spawn--;
			} while (spawn > 0);
			this.elapsed = 0f;
		}

		public bool wasCollision(BoundingBox bbox) {
			bool collision = false;
			if (this.foods != null) {
				foreach (Food food in this.foods) {
					if (food.BBox.Intersects(bbox)) {
						collision = true;
						break;
					}
				}
			}
			return collision;
		}

		public void update(float elapsed) {
			this.elapsed += elapsed;
			if (this.elapsed >= SPAWN_INTERVAL) {
				create();
			}
		}

		public void render(SpriteBatch spriteBatch) {
			foreach (Food food in this.foods) {
				food.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
