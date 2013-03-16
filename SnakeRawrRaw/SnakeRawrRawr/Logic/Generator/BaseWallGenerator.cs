using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic.Generator {
	public abstract class BaseWallGenerator {
		#region Class variables
		protected List<Vector2> positions;
		protected readonly Random RAND;
		#endregion Class variables

		#region Constructor
		public BaseWallGenerator(Random rand) {
			this.RAND = rand;
		}
		#endregion Constructor

		#region Support methods
		protected virtual bool checkPosition(Vector2 desiredPosition, Vector2 startPosition, out Vector2 lastPosition) {
			bool isSafe = PositionGenerator.getInstance().isPositionSafe(desiredPosition);
			if (isSafe) {
				this.positions.Add(desiredPosition);
				lastPosition = desiredPosition;
			} else {
				lastPosition = startPosition;
			}
			return isSafe;
		}
		
		protected abstract int getSize();

		public virtual List<Vector2> generate() {
			PositionGenerator.getInstance().markPositions(this.positions, false);
			return this.positions;
		}
		#endregion
	}
}
