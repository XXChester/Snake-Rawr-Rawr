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
	public class HUD {
		#region Class variables
		private Text2D scoreText;
		private Text2D statusText;
		private PulseDirection pulseDirection;
		private float lerp;
		private const float LERP_BY = .03f;
		private const string TEXT_WAITING = "Waiting" + TEXT_RESTART;
		private const string TEXT_GAME_OVER = "Game over" + TEXT_RESTART;
		private const string TEXT_RESTART = ".....Press {SPACE} to start";
		private const string TEXT_SCORE = "Score: ";
		private static Color TOP_COLOUR = Color.Red;
		private static Color BOTTOM_COLOUR = Color.White;
		#endregion Class variables

		#region Class propeties
		public int Score { get; set; }
		#endregion Class properties

		#region Constructor
		public HUD(ContentManager content) {
			this.pulseDirection = PulseDirection.Down;
			this.lerp = 0.001f;

			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");
			Text2DParams parms = new Text2DParams();
			parms.Font = font;
			parms.LightColour = TOP_COLOUR;

			parms.Position = new Vector2(Constants.RESOLUTION_X / 4, 0f);
			parms.WrittenText = TEXT_WAITING;
			this.statusText = new Text2D(parms);

			parms.Position = new Vector2(Constants.RESOLUTION_X - 250f, 0f);
			parms.WrittenText = TEXT_SCORE + getScore();
			this.scoreText = new Text2D(parms);
		}
		#endregion Constructor

		#region Support methods
		private string getScore() {
			return this.Score.ToString().PadLeft(5, '0');
		}

		public void update(float elapsed) {
			if (StateManager.getInstance().CurrentGameState == GameState.Waiting) {
				this.statusText.WrittenText = TEXT_WAITING;
			} else if (StateManager.getInstance().CurrentGameState == GameState.GameOver) {
				this.statusText.WrittenText = TEXT_GAME_OVER;
			} else if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				this.scoreText.WrittenText = TEXT_SCORE + getScore();
			}

			if (this.pulseDirection == PulseDirection.Up) {
				this.lerp += LERP_BY;
				if (this.lerp >= 1) {
					this.pulseDirection = PulseDirection.Down;
				}
			} else {
				this.lerp -= LERP_BY;
				if (this.lerp <= 0) {
					this.pulseDirection = PulseDirection.Up;
				}
			}
			Color lerped = Color.Lerp(TOP_COLOUR, BOTTOM_COLOUR, this.lerp);
			this.statusText.LightColour = lerped;
			this.scoreText.LightColour = lerped;
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.scoreText != null) {
				this.scoreText.render(spriteBatch);
			}
			if (StateManager.getInstance().CurrentGameState != GameState.Active) {
				if (this.statusText != null) {
					this.statusText.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}
