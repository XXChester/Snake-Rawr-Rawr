using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace SnakeRawrRawr.Model.Display {
	public interface IRenderable {
		void render(SpriteBatch spriteBatch);
		void update(float elapsed);
	}
}
