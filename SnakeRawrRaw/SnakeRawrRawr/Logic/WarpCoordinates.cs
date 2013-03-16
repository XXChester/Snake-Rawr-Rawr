using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SnakeRawrRawr.Logic {
	public class WarpCoordinates : TargetPosition {
		public Vector2 WarpTo { get; set; }

		public WarpCoordinates(Vector2 warpFrom, Vector2 warpTo) : base(warpFrom) {
			this.WarpTo = warpTo;
		}
	}
}
