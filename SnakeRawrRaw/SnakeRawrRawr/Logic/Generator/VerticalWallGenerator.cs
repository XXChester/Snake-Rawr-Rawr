using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic.Generator {
	public class VerticalWallGenerator : BaseWallGenerator {
		#region Constructor
		public VerticalWallGenerator(Random rand)
			: base(rand) {
		}
		#endregion Constructor

		#region Support methods
		protected override int getSize() {
			return base.RAND.Next(2, 15);
		}

		public override List<Vector2> generate() {
			base.positions = new List<Vector2>(getSize());
			Vector2 centreNode = PositionGenerator.getInstance().generateSpawn(markGeneratedPosition: false);
			base.positions.Add(centreNode);
			Vector2 lastPosition;
			Vector2 desiredPosition;
			for (int j = -1; j <= 2; j += 2) {
				lastPosition = centreNode;
				for (int i = 0; i < base.positions.Capacity / 2; i++) {
					desiredPosition = Vector2.Add(lastPosition, new Vector2(0f, j * Constants.TILE_SIZE));
					if (!base.checkPosition(desiredPosition, centreNode, out lastPosition)) {
						break;
					}
				}
			}
			return base.generate();
		}
		#endregion Support methods
	}
}
