
using Microsoft.Xna.Framework.Graphics;

namespace SnakeRawrRawr.Model.Display {
	public interface IRenderable {
		void render(SpriteBatch spriteBatch);
		void update(float elapsed);
	}
}
