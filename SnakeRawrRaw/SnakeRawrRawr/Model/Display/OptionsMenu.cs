﻿using System.Collections.Generic;
using GWNorthEngine.GUI;
using GWNorthEngine.GUI.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeRawrRawr.Engine;
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
		private List<TexturedEffectButton> buttons;
		private PulseEffectParams effectParms;
		private const float SPACE = 65f;
		private readonly string[] BUTTON_NAMES = { "SetToDefault", "SaveAndReturn", "Return" };
		private readonly Vector2 DEFAULT_SCALE = new Vector2(1f, .75f);

#if DEBUG
		private Texture2D debugTexture;
		private StaticDrawable2D center;
#endif
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public OptionsMenu(ContentManager content) :base(content, "OptionsMenu", new Vector2(Constants.RESOLUTION_X / 2, 100f)) {
			this.effectParms = new PulseEffectParams {
				ScaleBy = 1f,
				ScaleDownTo = .9f,
				ScaleUpTo = 1.1f
			};

			float x = Constants.RESOLUTION_X / 10;
			float leftSideX = Constants.RESOLUTION_X / 2 - 25f;
			float rightSideX = leftSideX + 250f;
			float bindingsStartPaddingX = 125f;
			this.playerOneSection = new OptionsSection(content, new Vector2(x, 325f), x + bindingsStartPaddingX, "One", ConfigurationManager.getInstance().PlayerOnesControls);
			this.playerTwoSection = new OptionsSection(content, new Vector2(x * 6, 325f), x * 6 + bindingsStartPaddingX, "Two", ConfigurationManager.getInstance().PlayerTwosControls);

			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");

			Vector2 position = new Vector2(leftSideX, 200f);
			SoundEngineSliderParams soundEngineParams = new SoundEngineSliderParams {
				Position = position,
				BallColour = Constants.TEXT_COLOUR,
				BarColour = Constants.TEXT_COLOUR,
				Content = content,
				CurrentValue = SoundManager.getInstance().MusicEngine.Volume,
				Font = font,
				LightColour = Constants.TEXT_COLOUR,
				SoundEngine = SoundManager.getInstance().MusicEngine,
				CheckBoxPosition = new Vector2(position.X + 250f, position.Y),
				Checked = SoundManager.getInstance().MusicEngine.Muted,
				CheckBoxText = "Muted",
			};
			this.musicSlider = new SoundEngineSlider(soundEngineParams);

			
			Text2DParams textParms = new Text2DParams {
				Font = font,
				LightColour = Constants.TEXT_COLOUR,
				Position = new Vector2(position.X - 300f, position.Y),
				WrittenText = "Music",
				Origin = new Vector2(16f)
			};
			this.musicSliderText = new Text2D(textParms);


			soundEngineParams.Position = new Vector2(position.X, position.Y + SPACE);
			soundEngineParams.CheckBoxPosition = new Vector2(soundEngineParams.CheckBoxPosition.X, position.Y + SPACE);
			soundEngineParams.SoundEngine = SoundManager.getInstance().SFXEngine;
			soundEngineParams.CurrentValue = SoundManager.getInstance().SFXEngine.Volume;
			soundEngineParams.Checked = SoundManager.getInstance().SFXEngine.Muted;
			this.sfxSlider = new SoundEngineSlider(soundEngineParams);

			textParms.Position = new Vector2(position.X - 300f, position.Y + SPACE);
			textParms.WrittenText = "SFX";
			this.sfxSliderText = new Text2D(textParms);
			this.buttons = new List<TexturedEffectButton>();
			Vector2 origin = new Vector2(90f, 64f);
			position = new Vector2(position.X, position.Y + 275f);
			TexturedEffectButtonParams buttonParms = new TexturedEffectButtonParams {
				Position = position,
				Origin = origin,
				Scale = new Vector2(1f),
				Effects = new List<BaseEffect> {
					new PulseEffect(this.effectParms)
				},
				PickableArea = getRect(origin, position),
				ResetDelegate = delegate(StaticDrawable2D button) {
					button.Scale = new Vector2(1f);
				}
			};
			for (int i = 0; i < this.BUTTON_NAMES.Length; i++) {
				buttonParms.Texture = LoadingUtils.load<Texture2D>(content, BUTTON_NAMES[i]);
				buttonParms.Position = new Vector2(buttonParms.Position.X, buttonParms.Position.Y + SPACE * 1.3f);
				buttonParms.PickableArea = getRect(origin, buttonParms.Position);
				this.buttons.Add(new TexturedEffectButton(buttonParms));
			}

#if DEBUG
			this.debugTexture = LoadingUtils.load<Texture2D>(content, "Chip");
			StaticDrawable2DParams centerParms = new StaticDrawable2DParams {
				Position = new Vector2(Constants.RESOLUTION_X / 2, 0),
				Scale = new Vector2(1f, Constants.RESOLUTION_Y),
				Texture = this.debugTexture,
				LightColour = Color.Green
			};
			this.center = new StaticDrawable2D(centerParms);
#endif
		}
		#endregion Constructor

		#region Support methods
		private Rectangle getRect(Vector2 origin, Vector2 position) {
			float originY = origin.Y / 2 + 4f;
			return new Rectangle((int)(position.X - origin.X), (int)(position.Y - originY), 256, 70);
		}

		private void setKeyBindings() {
			Controls controls = new Controls() {
				Left = this.playerOneSection.KeyBindings["Left"].BoundKey,
				Up = this.playerOneSection.KeyBindings["Up"].BoundKey,
				Right = this.playerOneSection.KeyBindings["Right"].BoundKey,
				Down = this.playerOneSection.KeyBindings["Down"].BoundKey,
			};
			ConfigurationManager.getInstance().PlayerOnesControls = controls;

			controls = new Controls() {
				Left = this.playerTwoSection.KeyBindings["Left"].BoundKey,
				Up = this.playerTwoSection.KeyBindings["Up"].BoundKey,
				Right = this.playerTwoSection.KeyBindings["Right"].BoundKey,
				Down = this.playerTwoSection.KeyBindings["Down"].BoundKey,
			};
			ConfigurationManager.getInstance().PlayerTwosControls = controls;
		}

		public override void update(float elapsed) {
			base.update(elapsed);
			this.playerOneSection.update(elapsed);
			this.playerTwoSection.update(elapsed);
			this.musicSlider.update(elapsed);
			this.sfxSlider.update(elapsed);
			this.sfxSliderText.update(elapsed);
			this.musicSliderText.update(elapsed);

			foreach (TexturedEffectButton button in this.buttons) {
				button.update(elapsed);
				button.processActorsMovement(InputManager.getInstance().MousePosition);
			}

			if (InputManager.getInstance().wasLeftButtonPressed()) {
				foreach (TexturedEffectButton button in this.buttons) {
					if (button.isActorOver(InputManager.getInstance().MousePosition)) {
						// we clicked a button
						if (button.Texture.Name.Equals(BUTTON_NAMES[0])) {
							IOHelper.resetToDefaultConfiguration();
							StateManager.getInstance().CurrentGameState = GameState.LoadOptions;
						} else if (button.Texture.Name.Equals(BUTTON_NAMES[1])) {
							setKeyBindings();
							IOHelper.saveCurrentConfiguration();
							StateManager.getInstance().CurrentGameState = GameState.MainMenu;
						} else if (button.Texture.Name.Equals(BUTTON_NAMES[2])) {
							IOHelper.loadConfiguration(IOHelper.getConfiguration());
							StateManager.getInstance().CurrentGameState = GameState.MainMenu;
						}
						break;
					}
				}
			}

			if (!this.playerOneSection.Binding && !this.playerTwoSection.Binding && InputManager.getInstance().wasKeyPressed(Keys.Escape)) {
				StateManager.getInstance().CurrentGameState = GameState.MainMenu;
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
			foreach (TexturedEffectButton button in this.buttons) {
				button.render(spriteBatch);
#if DEBUG
				DebugUtils.drawRectangle(spriteBatch, button.PickableArea, Color.Green, this.debugTexture);
#endif
			}
#if DEBUG
			this.center.render(spriteBatch);
#endif
		}
		#endregion Support methods
	}
}
