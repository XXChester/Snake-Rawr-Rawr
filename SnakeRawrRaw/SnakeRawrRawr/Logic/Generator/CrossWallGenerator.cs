using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic.Generator {
	public class CrossWallGenerator : BaseWallGenerator {
		#region Constructor
		public CrossWallGenerator(Random rand)
			: base(rand) {
		}
		#endregion Constructor

		#region Support methods
		protected override int getSize() {
			return base.RAND.Next(2, 7);
		}

		public override List<Vector2> generate() {
			int eachBranchesNodeCount = getSize();
			base.positions = new List<Vector2>(eachBranchesNodeCount * 4 + 1);
			Vector2 centreNode = PositionGenerator.getInstance().generateSpawn(markGeneratedPosition: false);
			base.positions.Add(centreNode);
			Vector2 lastPosition = centreNode;
			Vector2 desiredPosition;
			// horizontal branches
			for (int j = -1; j <= 2; j += 2) {
				lastPosition = centreNode;
				for (int i = 0; i < eachBranchesNodeCount; i++) {
					desiredPosition = Vector2.Add(lastPosition, new Vector2(0f, j * Constants.TILE_SIZE));
					if (!base.checkPosition(desiredPosition, centreNode, out lastPosition)) {
						break;
					}
				}
			}

			// vertical branches
			for (int j = -1; j <= 2; j += 2) {
				lastPosition = centreNode;
				for (int i = 0; i < eachBranchesNodeCount; i++) {
					desiredPosition = Vector2.Add(lastPosition, new Vector2(j * Constants.TILE_SIZE, 0f));
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
