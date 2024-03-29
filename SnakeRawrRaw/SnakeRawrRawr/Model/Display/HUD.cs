﻿
using GWNorthEngine.Model;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model.Display {
	public class HUD {
		#region Class variables
		private Text2D[] scoreTexts;
		private Text2D[] winnerTexts;
		private Text2D statusText;
		private StaticDrawable2D activeCountdownItem;
		private StaticDrawable2D[] countDownImages;
		private StaticDrawable2D gameOver;
		private ScaleOverTimeEffectParams scaleOverTimeEffectParms;
		private SoundEffect soundEffect;
		private float elapsedTime;
		private int index;
		private const string TEXT_RESTART = "Press {Enter} to replay";
		private const string TEXT_SCORE = "Score: ";
		private const string TEXT_WINNER = "Winner";
		private const float RIGHT_X = Constants.RESOLUTION_X - 250f;
		private const float LEFT_X = 15f;
		private const float SCORES_Y = -5f;
		private const float WINNER_Y_OFFSET = 22f;
		private const float WINNER_X_PADDING = 60f;
		#endregion Class variables

		#region Class propeties
		public int PlayerOneScore { get; set; }
		public int PlayerTwoScore { get; set; }
		#endregion Class properties

		#region Constructor
		public HUD(ContentManager content) {
			SpriteFont font = LoadingUtils.load<SpriteFont>(content, "SpriteFont1");
			Text2DParams parms = new Text2DParams();
			parms.Font = font;
			parms.LightColour = Color.Red;

			parms.Position = new Vector2(Constants.RESOLUTION_X / 3, 450f);
			parms.WrittenText = TEXT_RESTART;
			this.statusText = new Text2D(parms);

			ColourLerpEffectParams effectParms = new ColourLerpEffectParams {
				LerpBy = 5f,
				LerpDownTo = Color.White,
				LerpUpTo = Color.Red
			};

			int size = 1;
			if (StateManager.getInstance().GameMode == GameMode.TwoPlayer) {
				size = 2;
			}
			this.scoreTexts = new Text2D[size];
			this.winnerTexts = new Text2D[size];

			parms.Position = new Vector2(RIGHT_X, SCORES_Y);
			parms.WrittenText = TEXT_SCORE + getScore(0);
			this.scoreTexts[0] = new Text2D(parms);
			this.scoreTexts[0].addEffect(new ColourLerpEffect(effectParms));

			parms.Position = new Vector2(RIGHT_X + WINNER_X_PADDING, SCORES_Y + WINNER_Y_OFFSET);
			parms.WrittenText = TEXT_WINNER;
			this.winnerTexts[0] = new Text2D(parms);
			this.winnerTexts[0].addEffect(new ColourLerpEffect(effectParms));

			if (StateManager.getInstance().GameMode == GameMode.TwoPlayer) {
				parms.Position = new Vector2(LEFT_X, SCORES_Y);
				parms.WrittenText = TEXT_SCORE + getScore(1);
				this.scoreTexts[1] = new Text2D(parms);
				this.scoreTexts[1].addEffect(new ColourLerpEffect(effectParms));

				parms.Position = new Vector2(LEFT_X + WINNER_X_PADDING, SCORES_Y + WINNER_Y_OFFSET);
				parms.WrittenText = TEXT_WINNER;
				this.winnerTexts[1] = new Text2D(parms);
				this.winnerTexts[1].addEffect(new ColourLerpEffect(effectParms));
			}

			this.countDownImages = new StaticDrawable2D[3];
			StaticDrawable2DParams countDownParms = new StaticDrawable2DParams {
				Position = new Vector2(Constants.RESOLUTION_X / 2, Constants.RESOLUTION_Y / 2),
				Origin = new Vector2(256f),
			};
			for (int i = 0; i < this.countDownImages.Length; i++) {
				countDownParms.Texture = LoadingUtils.load<Texture2D>(content, (i + 1).ToString());
				this.countDownImages[i] = new StaticDrawable2D(countDownParms);
			}

			StaticDrawable2DParams gameOverParms = new StaticDrawable2DParams {
				Position = new Vector2(Constants.RESOLUTION_X / 2, Constants.RESOLUTION_Y / 2),
				Origin = new Vector2(512f),
				Texture = LoadingUtils.load<Texture2D>(content, "GameOver")
			};
			this.gameOver = new StaticDrawable2D(gameOverParms);

			scaleOverTimeEffectParms = new ScaleOverTimeEffectParams {
				ScaleBy = new Vector2(-1f)
			};

			this.soundEffect = LoadingUtils.load<SoundEffect>(content, "ShortBeep");

			this.index = 2;
			this.activeCountdownItem = this.countDownImages[this.index];
			this.activeCountdownItem.addEffect(new ScaleOverTimeEffect(scaleOverTimeEffectParms));
			this.elapsedTime = 0f;
			SoundManager.getInstance().SFXEngine.playSoundEffect(this.soundEffect);
		}
		#endregion Constructor

		#region Support methods
		private string getScore(int player) {
			if (player == 0) {
				return this.PlayerOneScore.ToString().PadLeft(5, '0');
			} else {
				return this.PlayerTwoScore.ToString().PadLeft(5, '0');
			}
		}

		public void update(float elapsed) {
			this.elapsedTime += elapsed;

			if (StateManager.getInstance().CurrentGameState == GameState.Waiting) {
				if (this.elapsedTime >= 1000f && this.index > -1) {
					this.index -= 1;
					if (this.index == -1) {
						this.activeCountdownItem = null;
						StateManager.getInstance().CurrentGameState = GameState.Active;
					} else {
						this.activeCountdownItem = this.countDownImages[this.index];
						this.activeCountdownItem.addEffect(new ScaleOverTimeEffect(this.scaleOverTimeEffectParms));
						SoundManager.getInstance().SFXEngine.playSoundEffect(this.soundEffect);
					}
					this.elapsedTime = 0f;
				}
				if (this.activeCountdownItem != null) {
					this.activeCountdownItem.update(elapsed);
				}
			}

			if (StateManager.getInstance().CurrentGameState == GameState.GameOver) {
				this.statusText.WrittenText = TEXT_RESTART;
			} else if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				for (int i = 0; i < this.scoreTexts.Length; i++) {
					this.scoreTexts[i].WrittenText = TEXT_SCORE + getScore(i);
				}
			}
			if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				for (int i = 0; i < this.scoreTexts.Length; i++) {
					this.scoreTexts[i].update(elapsed);
				}
			} else {
				for (int i = 0; i < this.scoreTexts.Length; i++) {
					this.scoreTexts[i].LightColour = Color.Red;
				}
			}

			if (this.winnerTexts != null) {
				for (int i = 0; i < this.winnerTexts.Length; i++) {
					this.winnerTexts[i].update(elapsed);
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.scoreTexts != null) {
				for (int i = 0; i < this.scoreTexts.Length; i++) {
					this.scoreTexts[i].render(spriteBatch);
				}
			}

			if (StateManager.getInstance().CurrentGameState == GameState.GameOver) {
				if (this.statusText != null) {
					this.statusText.render(spriteBatch);
				}
				if (this.gameOver != null) {
					this.gameOver.render(spriteBatch);
				}
				if (StateManager.getInstance().WhoWon != Winner.None) {
					if (StateManager.getInstance().WhoWon == Winner.PlayerOne) {
						this.winnerTexts[0].render(spriteBatch);
					} else {
						this.winnerTexts[1].render(spriteBatch);
					}
				}
			} else if (StateManager.getInstance().CurrentGameState == GameState.Waiting) {
				if (this.activeCountdownItem != null) {
					this.activeCountdownItem.render(spriteBatch);
				}
			}
		}
		#endregion Support methods
	}
}
