using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic {
	public struct PivotPoint {
		public Vector2 Position { get; set; }
		public Vector2 Heading { get; set; }
		public float Rotation { get; set; }
	}
}
