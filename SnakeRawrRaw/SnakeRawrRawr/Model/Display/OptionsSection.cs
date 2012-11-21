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

namespace SnakeRawrRawr.Model.Display {
	public class OptionsSection {
		#region Class variables
		private Text2D heading;
		private readonly string[] BINDING_NAMES = { "Left, Up, Right, Down" };
		#endregion Class variables

		#region Class propeties
		public bool Binding { get; set; }
		#endregion Class properties

		#region Constructor
		public OptionsSection(ContentManager content, Vector2 position, string sectionName) {
			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");
			Text2DParams parms = new Text2DParams {
				Font = font,
				LightColour = Color.Red,
				Position = position,
				WrittenText = "Player " + sectionName + "'s Key Bindings",
			};

			this.heading = new Text2D(parms);
		}
		#endregion Constructor

		#region Support methods
		public void update(float elapsed) {
			this.heading.update(elapsed);
		}

		public void render(SpriteBatch spriteBatch) {
			this.heading.render(spriteBatch);
		}
		#endregion Support methods
	}
}
