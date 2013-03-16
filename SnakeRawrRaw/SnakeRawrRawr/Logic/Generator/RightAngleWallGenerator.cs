using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic.Generator {
	public class RightAngleWallGenerator : BaseWallGenerator {
		#region Constructor
		public RightAngleWallGenerator(Random rand)
			: base(rand) {
		}
		#endregion Constructor

		#region Support methods
		protected override int getSize() {
			return base.RAND.Next(2, 15);
		}

		public override List<Vector2> generate() {
			int eachBranchesNodeCount = getSize();
			base.positions = new List<Vector2>(eachBranchesNodeCount * 2 + 1);
			Vector2 centreNode = PositionGenerator.getInstance().generateSpawn(markGeneratedPosition: false);
			base.positions.Add(centreNode);
			Vector2 lastPosition = centreNode;
			Vector2 desiredPosition;
			int direction = 1;
			if (base.RAND.Next(0, 2) == 1) {
				direction = -1;
			}

			// hoizontal
			for (int i = 0; i < eachBranchesNodeCount; i++) {
				desiredPosition = Vector2.Add(lastPosition, new Vector2(direction * Constants.TILE_SIZE, 0f));
				if (!base.checkPosition(desiredPosition, centreNode, out lastPosition)) {
					break;
				}
			}

			direction = 1;
			if (base.RAND.Next(0, 2) == 1) {
				direction = -1;
			}

			// vertical
			lastPosition = centreNode;
			for (int i = 0; i < eachBranchesNodeCount; i++) {
				desiredPosition = Vector2.Add(lastPosition, new Vector2(0f, direction * Constants.TILE_SIZE));
				if (!base.checkPosition(desiredPosition, centreNode, out lastPosition)) {
					break;
				}
			}
			return base.generate();
		}
		#endregion Support methods
	}
}
