using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic {
	public class PivotPoint : TargetPosition {
		public Vector2 Heading { get; set; }
		public float Rotation { get; set; }

		public PivotPoint(Vector2 position, Vector2 heading, float rotation)
			: base(position) {
			this.Heading = heading;
			this.Rotation = rotation;
		}
	}
}
