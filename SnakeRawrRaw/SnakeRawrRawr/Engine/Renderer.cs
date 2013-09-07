using GWNorthEngine.Engine;
using GWNorthEngine.Engine.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Scripting;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeRawrRawr.Logic;
using SnakeRawrRawr.Logic.Generator;
using SnakeRawrRawr.Model.Display;

namespace SnakeRawrRawr.Engine {
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Renderer : BaseRenderer {
		private Cinematic cinematic;
		private GameDisplay gameDisplay;
		private MainMenu mainMenu;
		private OptionsMenu optionsMenu;
		private IRenderable activeDisplay;
		private StaticDrawable2D transitionItem;
		private FadeEffect fadeEffect;
		private FadeEffectParams fadeParams;
		private float elapsedTransitionTime;
		private const string GAME_NAME = "Snake Rawr Rawr";
		private const float TRANSITION_TIME = 750f;

		public Renderer() {
			BaseRendererParams baseParms = new BaseRendererParams();
			baseParms.MouseVisible = true;
			baseParms.WindowsText = GAME_NAME;
			baseParms.ScreenWidth = Constants.RESOLUTION_X;
			baseParms.ScreenHeight = Constants.RESOLUTION_Y;
			baseParms.ContentRootDirectory = "SnakeRawrRawrContent";
			baseParms.MouseVisible = false;
#if DEBUG
			baseParms.RunningMode = RunningMode.Debug;
#else
			baseParms.RunningMode = RunningMode.Release;
#endif
			base.initialize(baseParms);
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent() {
			SoundManager.getInstance().init(Content);
			IOHelper.loadConfiguration(IOHelper.getConfiguration());

			this.cinematic = new Cinematic(Content);
			this.mainMenu = new MainMenu(Content);
			this.optionsMenu = new OptionsMenu(Content);

			this.fadeParams = new FadeEffectParams {
				OriginalColour = Color.Black,
				State = FadeEffect.FadeState.Out,
				TotalTransitionTime = TRANSITION_TIME
			};
			this.fadeEffect = new FadeEffect(fadeParams);
			
			StaticDrawable2DParams transitionParms = new StaticDrawable2DParams {
				Texture =  LoadingUtils.load<Texture2D>(Content, "Chip"),
				Scale = new Vector2(Constants.RESOLUTION_X, Constants.RESOLUTION_Y),
				LightColour = Color.Black
			};
			this.transitionItem = new StaticDrawable2D(transitionParms);
			this.transitionItem.addEffect(this.fadeEffect);

#if WINDOWS
#if DEBUG
			ScriptManager.getInstance().LogFile = "Log.log";
			if (StateManager.getInstance().CurrentGameState == GameState.Active || StateManager.getInstance().CurrentGameState == GameState.Waiting ||
				StateManager.getInstance().CurrentGameState == GameState.GameOver) {
				this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
			}
#endif
#endif

		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent() {
			base.UnloadContent();
		}

		private void handleNewTransition() {
			if (StateManager.getInstance().CurrentTransitionState == TransitionState.InitTransitionIn) {
				this.fadeEffect.State = FadeEffect.FadeState.Out;
				this.fadeEffect.reset();
				StateManager.getInstance().CurrentTransitionState = TransitionState.TransitionIn;
				this.elapsedTransitionTime = 0f;
			} else if (StateManager.getInstance().CurrentTransitionState == TransitionState.InitTransitionOut) {
				this.fadeEffect.State = FadeEffect.FadeState.In;
				// did we interrupt the previous transition
				if (StateManager.getInstance().PreviousTransitionState == TransitionState.TransitionIn) {
					this.fadeEffect.reset();
					this.elapsedTransitionTime = 0f;
				} else {
					// we need to reverse the fade from its current position since we interrupted the previous transition
					this.fadeEffect.interruptFade();
					this.elapsedTransitionTime = this.fadeEffect.ElapsedTransitionTime;
				}
				StateManager.getInstance().CurrentTransitionState = TransitionState.TransitionOut;
			}

			if (StateManager.getInstance().CurrentTransitionState == TransitionState.TransitionIn && this.elapsedTransitionTime == 0f) {
				// we just finished transitioning so we need to create our new Displays
				if (StateManager.getInstance().CurrentGameState == GameState.LoadGame) {
					SoundManager.getInstance().removeAllEmitters();
					this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
					StateManager.getInstance().CurrentGameState = GameState.Waiting;
				} else if (StateManager.getInstance().CurrentGameState == GameState.LoadOptions) {
					this.optionsMenu = new OptionsMenu(Content);
					StateManager.getInstance().CurrentGameState = GameState.Options;
				}

				// we need to change our display
				if (StateManager.getInstance().CurrentGameState == GameState.CompanyCinematic) {
					this.activeDisplay = this.cinematic;
				} else if (StateManager.getInstance().CurrentGameState == GameState.MainMenu) {
					this.activeDisplay = this.mainMenu;
				} else if (StateManager.getInstance().CurrentGameState == GameState.Waiting || StateManager.getInstance().CurrentGameState == GameState.Active ||
					StateManager.getInstance().CurrentGameState == GameState.GameOver) {
					this.activeDisplay = this.gameDisplay;
				} else if (StateManager.getInstance().CurrentGameState == GameState.Options) {
					this.activeDisplay = this.optionsMenu;
				}
			}
		}

		private void handleTransitionState(float elapsed) {
			// if we are transitioning
			if (StateManager.getInstance().CurrentTransitionState == TransitionState.TransitionIn || StateManager.getInstance().CurrentTransitionState == TransitionState.TransitionOut) {
				this.elapsedTransitionTime += elapsed;
				// if our transition time is up we need to change transition states
				if (this.elapsedTransitionTime >= TRANSITION_TIME) {
					if (StateManager.getInstance().CurrentTransitionState == TransitionState.TransitionIn) {
						StateManager.getInstance().CurrentTransitionState = TransitionState.None;
					} else {
						StateManager.getInstance().CurrentTransitionState = TransitionState.InitTransitionIn;
					}
				}
			}
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime) {
#if DEBUG
			base.Window.Title = GAME_NAME + "...FPS: " + FrameRate.getInstance().calculateFrameRate(gameTime) + "    X:" +
				InputManager.getInstance().MouseX + " Y:" + InputManager.getInstance().MouseY;

			if (InputManager.getInstance().wasKeyPressed(Keys.R)) {
				SoundManager.getInstance().removeAllEmitters();
				this.gameDisplay = new GameDisplay(GraphicsDevice, Content);
			}
			if (InputManager.getInstance().wasKeyPressed(Keys.Escape) ||
			InputManager.getInstance().wasButtonPressed(PlayerIndex.One, Buttons.B)) {
				//SpawnGenerator.getInstance().Running = false;
				//this.Exit();
			}
#endif
			// start the transitions
			handleNewTransition();

			if (StateManager.getInstance().CurrentGameState == GameState.Exit) {
				SpawnGenerator.getInstance().Running = false;
				this.Exit();
			}


			base.IsMouseVisible = false;
			if (StateManager.getInstance().CurrentGameState == GameState.Options) {
				if (!InputManager.getInstance().isLeftButtonDown()) {
					base.IsMouseVisible = true;
				}
			}

			float elapsed = gameTime.ElapsedGameTime.Milliseconds;
			handleTransitionState(elapsed);
			SoundManager.getInstance().update();

			this.activeDisplay.update(elapsed);
			this.transitionItem.update(elapsed);
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Black);

			base.spriteBatch.Begin();
			this.activeDisplay.render(base.spriteBatch);
			this.transitionItem.render(base.spriteBatch);
			base.spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
