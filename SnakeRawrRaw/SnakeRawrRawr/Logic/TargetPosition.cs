﻿
using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic {
	public abstract class TargetPosition {
		public Vector2 Position { get; set; }

		public TargetPosition(Vector2 position) {
			this.Position = position;
		}
	}
}
