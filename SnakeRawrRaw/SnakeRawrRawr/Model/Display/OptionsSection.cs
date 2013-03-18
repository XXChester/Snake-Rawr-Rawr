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

using GWNorthEngine.Engine;
using GWNorthEngine.Engine.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class OptionsSection {
		#region Class variables
		private Text2D heading;
		//private List<KeyBinding> bindings;
		private Dictionary<string, KeyBinding> bindings;
		private readonly string[] BINDING_NAMES = { "Left", "Up", "Right", "Down" };
		private const float SPACE = 35f;
		#endregion Class variables

		#region Class propeties
		public bool Binding {
			get {
				bool isBinding = false;
				foreach (var binding in this.bindings) {
					if (binding.Value.Binding) {
						isBinding = true;
						break;
					}
				};
				return isBinding;
			}
		}
		public Dictionary<string, KeyBinding> KeyBindings { get { return this.bindings; } }
		#endregion Class properties

		#region Constructor
		public OptionsSection(ContentManager content, Vector2 position, float bindersX, string sectionName, Controls controls) {
			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");
			Text2DParams parms = new Text2DParams {
				Font = font,
				LightColour = Color.Red,
				Position = position,
				WrittenText = "Player " + sectionName + "'s Key Bindings",
			};

			this.heading = new Text2D(parms);
			Vector2 textPosition = new Vector2(position.X, position.Y + SPACE);
			Vector2 bindersPosition = new Vector2(bindersX, textPosition.Y);
			//this.bindings = new List<KeyBinding>();
			this.bindings = new Dictionary<string, KeyBinding>();

			this.bindings.Add(BINDING_NAMES[0], new KeyBinding(font, content, textPosition, bindersPosition, BINDING_NAMES[0], controls.Left));
			getPositions(ref textPosition, ref bindersPosition);

			this.bindings.Add(BINDING_NAMES[1], new KeyBinding(font, content, textPosition, bindersPosition, BINDING_NAMES[1], controls.Up));
			getPositions(ref textPosition, ref bindersPosition);

			this.bindings.Add(BINDING_NAMES[2], new KeyBinding(font, content, textPosition, bindersPosition, BINDING_NAMES[2], controls.Right));
			getPositions(ref textPosition, ref bindersPosition);

			this.bindings.Add(BINDING_NAMES[3], new KeyBinding(font, content, textPosition, bindersPosition, BINDING_NAMES[3], controls.Down));
			getPositions(ref textPosition, ref bindersPosition);
		}
		#endregion Constructor

		#region Support methods
		private void getPositions(ref Vector2 textPosition, ref Vector2 bindersPosition) {
			textPosition = new Vector2(textPosition.X, textPosition.Y + SPACE);
			bindersPosition = new Vector2(bindersPosition.X, textPosition.Y);
		}

		public void update(float elapsed) {
			this.heading.update(elapsed);

			foreach (var binding in this.bindings) {
				binding.Value.update(elapsed);
			}
			
		}

		public void render(SpriteBatch spriteBatch) {
			this.heading.render(spriteBatch);
			foreach (var binding in this.bindings) {
				binding.Value.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
