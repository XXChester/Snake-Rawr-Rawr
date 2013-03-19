using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic.Generator {
	public class ScatteredWallGenerator : BaseWallGenerator {
		#region Constructor
		public ScatteredWallGenerator(Random rand) : base(rand){
		}
		#endregion Constructor

		#region Support methods
		protected override int getSize() {
			return this.RAND.Next(1, 10);
		}

		public override List<Vector2> generate() {
			base.positions = new List<Vector2>(getSize());
			for (int i = 0; i < base.positions.Capacity; i++) {
				base.positions.Add(PositionGenerator.getInstance().generateSpawn());
			}
			return base.generate();
		}
		#endregion Support methods
	}
}
