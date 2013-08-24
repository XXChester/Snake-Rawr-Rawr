using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SnakeRawrRawr.Logic.Generator {
	public class SpawnRequest {

		public bool Surround { get; set; }
		public bool MarkGeneratedPosition { get; set; }
		public PositionGenerator.SpawnProcessor Processor { get; set; }

		public SpawnRequest() { }
		public SpawnRequest(PositionGenerator.SpawnProcessor processor, bool surround = true, bool markGeneratedPosition = true) {
			this.Surround = surround;
			this.MarkGeneratedPosition = markGeneratedPosition;
			this.Processor = processor;
		}
	}
}
