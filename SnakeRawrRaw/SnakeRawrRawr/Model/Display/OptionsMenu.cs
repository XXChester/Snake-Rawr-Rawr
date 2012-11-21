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
using GWNorthEngine.GUI;
using GWNorthEngine.GUI.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class OptionsMenu : BaseMenu {
		#region Class variables
		private OptionsSection playerOneSection;
		private OptionsSection playerTwoSection;
		private Text2D sfxSliderText;
		private Text2D musicSliderText;
		private SoundEngineSlider sfxSlider;
		private SoundEngineSlider musicSlider;
		private const float SPACE = 65f;
		private readonly string[] BUTTON_NAMES = { "SaveAndReturn", "Save", "ReturnToMain" };
		private readonly Color DEFAULT = Color.Red;
		private readonly Vector2 DEFAULT_SCALE = new Vector2(1f, .75f);
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public OptionsMenu(ContentManager content) :base(content, "OptionsMenu", new Vector2(Constants.RESOLUTION_X / 2, 100f)) {
			float x = Constants.RESOLUTION_X / 10;
			this.playerOneSection = new OptionsSection(content, new Vector2(x, 350f), "One");
			this.playerTwoSection = new OptionsSection(content, new Vector2(x * 6, 350f), "Two");

			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");

			Vector2 position = new Vector2(Constants.RESOLUTION_X / 2 - 25f, 200f);
			SoundEngineSliderParams soundEngineParams = new SoundEngineSliderParams {
				Position = position,
				BallColour = DEFAULT,
				BarColour = DEFAULT,
				Content = content,
				CurrentValue = .5f,
				Font = font,
				LightColour = DEFAULT,
				SoundEngine = SoundManager.getInstance().MusicEngine,
				CheckBoxPosition = new Vector2(position.X + 250f, position.Y),
				Checked = SoundManager.getInstance().MusicEngine.Muted,
				CheckBoxText = "Muted",
			};
			this.musicSlider = new SoundEngineSlider(soundEngineParams);

			
			Text2DParams textParms = new Text2DParams {
				Font = font,
				LightColour = DEFAULT,
				Position = new Vector2(position.X - 300f, position.Y),
				WrittenText = "Music",
				Origin = new Vector2(16f)
			};
			this.musicSliderText = new Text2D(textParms);


			soundEngineParams.Position = new Vector2(position.X, position.Y + SPACE);
			soundEngineParams.CheckBoxPosition = new Vector2(soundEngineParams.CheckBoxPosition.X, position.Y + SPACE);
			soundEngineParams.SoundEngine = SoundManager.getInstance().SFXEngine;
			soundEngineParams.Checked = SoundManager.getInstance().SFXEngine.Muted;
			this.sfxSlider = new SoundEngineSlider(soundEngineParams);

			textParms.Position = new Vector2(position.X - 300f, position.Y + SPACE);
			textParms.WrittenText = "SFX";
			this.sfxSliderText = new Text2D(textParms);
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			base.update(elapsed);
			this.playerOneSection.update(elapsed);
			this.playerTwoSection.update(elapsed);
			this.musicSlider.update(elapsed);
			this.sfxSlider.update(elapsed);
			this.sfxSliderText.update(elapsed);
			this.musicSliderText.update(elapsed);

			if (!this.playerOneSection.Binding && !this.playerTwoSection.Binding && InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				//StateManager.getInstance().CurrentGameState = GameState.MainMenu;
				StateManager.getInstance().CurrentGameState = GameState.Exit;
			}
		}

		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);
			this.playerOneSection.render(spriteBatch);
			this.playerTwoSection.render(spriteBatch);
			this.musicSlider.render(spriteBatch);
			this.sfxSlider.render(spriteBatch);
			this.musicSliderText.render(spriteBatch);
			this.sfxSliderText.render(spriteBatch);
		}
		#endregion Support methods
	}
}
