
using GWNorthEngine.GUI;
using GWNorthEngine.GUI.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeRawrRawr.Model {
	public class KeyBinding {
		#region Class variables
		private Text2D text;
		private KeyBindingTextBox keyBinder;
		private bool wasBinding;
		#endregion Class variables

		#region Class propeties
		public bool Binding { get { return this.keyBinder.HasFocus; } }
		public Keys BoundKey { get { return this.keyBinder.BoundTo; } }
		#endregion Class properties

		#region Constructor
		public KeyBinding(SpriteFont font, ContentManager content, Vector2 textPosition, Vector2 binderPosition, string text, Keys boundKey) {
			Text2DParams textParams = new Text2DParams() {
				Font = font,
				Position = textPosition,
				WrittenText = text,
				LightColour = Constants.TEXT_COLOUR
			};
			this.text = new Text2D(textParams);

			KeyBindingTextBoxParams binderParams = new KeyBindingTextBoxParams() {
				Content = content,
				Font = font,
				LightColour = Constants.TEXT_COLOUR,
				Position = binderPosition,
				Scale = new Vector2(250f, 1f),
				Text = KeyLists.translate(boundKey),
				BoundTo  = boundKey,
				TextColour = Constants.TEXT_COLOUR
			};
			this.keyBinder = new KeyBindingTextBox(binderParams);
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			this.keyBinder.update(elapsed);
			this.wasBinding = Binding;
		}

		public void render(SpriteBatch spriteBatch) {
			this.text.render(spriteBatch);
			this.keyBinder.render(spriteBatch);
		}
		#endregion Support methods
	}
}
