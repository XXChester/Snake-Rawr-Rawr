using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic {
	public class WarpCoordinates {
		public Vector2 WarpFrom { get; set; }
		public Vector2 WarpTo { get; set; }

		public WarpCoordinates(Vector2 warpFrom, Vector2 warpTo) {
			this.WarpFrom = warpFrom;
			this.WarpTo = warpTo;
		}
	}
}
