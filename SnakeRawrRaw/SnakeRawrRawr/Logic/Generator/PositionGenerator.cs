using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;


namespace SnakeRawrRawr.Logic.Generator {
	public class PositionGenerator {
		#region Class variables
		private Random rand;
		private bool[,] layout = new bool[MAX_Y, MAX_X];
		private static PositionGenerator instance = new PositionGenerator();
		public const int GRID_PIECE_SIZE = 16;
		private const int MAX_X = Constants.RESOLUTION_X / GRID_PIECE_SIZE;
		private const int MAX_Y = Constants.RESOLUTION_Y / GRID_PIECE_SIZE;
		/*private readonly int[,] DIRECTIONS = new int[8, 2] {
					{0,1},		// right
					{0,-1},		// left
					{1,0},		// bottom
					{-1, 0},	// top
					{-1,-1},	// top left corner
					{-1,1},		// top right corner
					{1,-1},		// bottom left corner
					{1,1}		// bottom right corner

				};*/
		private readonly int[,] DIRECTIONS = new int[12, 2] {
						{0,1},		// right
					{0,-1},		// left
					{1,0},		// bottom
					{-1, 0},	// top
					{-1,-1},	// top left corner
					{-1,1},		// top right corner
					{1,-1},		// bottom left corner
					{1,1},		// bottom right corner
					{0,-2},		// left left
					{-2, 0},	// top top
					{0, 2},		// right right
					{2, 0}		// bottom bottom

		};
		#endregion Class variables

		#region Class properties
		public bool[,] Layout { get { return this.layout; } }
		#endregion Class properties

		#region Support methods
		public static PositionGenerator getInstance() {
			return instance;
		}

		private Point getIndexes(Vector2 position) {
			Vector2 indexes = Vector2.Divide(position, (float)GRID_PIECE_SIZE);
			return new Point((int)indexes.X, (int)indexes.Y);
		}

		public void init(Random rand) {
			this.rand = rand;
			int offset = (int)Constants.HUD_OFFSET / GRID_PIECE_SIZE;
			for (int y = 0; y <= layout.GetUpperBound(0); y++) {
				for (int x = 0; x <= layout.GetUpperBound(1); x++) {
					if (y < offset) {
						this.layout[y, x] = false;
					} else {
						this.layout[y, x] = true;
					}
				}
			}
		}

		public void markPositions(List<Vector2> positions, bool available = true, bool surround = true, bool extraSurround = false) {
			foreach (Vector2 position in positions) {
				markPosition(position, available, surround, extraSurround);
			}
		}

		public void markPosition(Vector2 position, bool available = true, bool surround = true, bool extraSurround = false) {
			Point point = getIndexes(position);
			// bounds check
			if (point.Y >= 0 && point.Y < MAX_Y && point.X >= 0 && point.X < MAX_X) {
				this.layout[point.Y, point.X] = available;
				if (surround) {
					int x, y;
					int loopAmount = 16;
					if (extraSurround) {
						loopAmount = 24;
					}
					for (int i = 0; i < loopAmount; i++) {
						y = point.Y + DIRECTIONS[i / 2, 0];
						x = point.X + DIRECTIONS[i / 2, 1];
						if (x > -1 && y > -1 && x < MAX_X && y < MAX_Y) {
							this.layout[y, x] = available;
						}
					}
				}
			}
		}

		public void updateLocation(Vector2 previousPosition, Vector2 newPosition, bool surround = true, bool extraSurround = false) {
			markPosition(previousPosition, surround: surround, extraSurround: extraSurround);
			markPosition(newPosition, false, surround, extraSurround);
		}

		public bool isPositionSafe(Vector2 position) {
			bool safe = false;
			Point point = getIndexes(position);
			// bounds check
			if (point.Y >= 0 && point.Y < MAX_Y && point.X >= 0 && point.X < MAX_X) {
				if (this.layout[point.Y, point.X]) {
					safe = true;
				}
			}
			return safe;
		}

		public Vector2 generateSpawn(bool surround = true, bool markGeneratedPosition=true) {
			Vector2 position;
			do {
				position = new Vector2(PositionUtils.getPosition(rand.Next(1, Constants.MAX_X_TILES - 1)),
				PositionUtils.getPosition(rand.Next(1, Constants.MAX_Y_TILES - 1)) + Constants.HUD_OFFSET + Constants.TILE_SIZE / 2 + Constants.OVERLAP);
			} while (!isPositionSafe(position));
			if (markGeneratedPosition) {
				markPosition(position, false, surround);
			}
			return position;
		}
		#endregion support methods
	}
}
