using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public class WallManager : BaseManager {
		#region Class variables
		private const float SPAWN_INTERVAL = 6000f;
		private readonly List<BaseWallGenerator> GENERATORS;
		#endregion Class variables

		#region Class propeties
		#endregion Class properties

		#region Constructor
		public WallManager(ContentManager content, Random rand) : base(content, rand, SPAWN_INTERVAL) {
			this.GENERATORS = new List<BaseWallGenerator>() {
				new ScatteredWallGenerator(rand),
				new HorizontalWallGenerator(rand),
				new VerticalWallGenerator(rand),
				new CrossWallGenerator(rand),
				new RightAngleWallGenerator(rand),
			};
		}
		#endregion Constructor

		#region Support methods
		protected override  void create() {
			List<Vector2> positions = this.GENERATORS[base.rand.Next(0, this.GENERATORS.Count - 1)].generate();
			foreach (Vector2 position in positions) {
				base.nodes.Add(new Wall(base.content, position));
			}
			base.create();
		}

		public override void update(float elapsed) {
			Wall wall = null;
			for(int i = base.nodes.Count - 1; i >= 0; i--) {
				wall = (Wall)base.nodes[i];
				if (wall != null) {
					wall.update(elapsed);
					if (wall.Release) {
						base.nodes.RemoveAt(i);
					}
				}
			}

			base.update(elapsed);
		}
		#endregion Support methods
	}
}
