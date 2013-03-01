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

using GWNorthEngine.Audio;
using GWNorthEngine.Audio.Params;
using GWNorthEngine.Engine;
using GWNorthEngine.Engine.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

using SnakeRawrRawr.Engine;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model {
	public abstract class Food : Entity {
		public enum Stage { Spawn, Idle, Dying }
		#region Class variables
		private float elapsedTime;
		private Stage lifeStage;
		private bool lapseTime;
		private List<Texture2D> dyingCharacterTextures;
		private SoundEffect spawnSFX;
		private SoundEffect dyingSFX;
		private SoundEmitter sfxEmitter;
		private Texture2D deathParticleTexture;
		private Animated2DSprite spawnSprite;
		private Animated2DSprite idleSprite;
		private DeathParticleEmitter deathEmitter;
		protected BaseParticle2DEmitter idleEmitter;
		protected SoundEffect idleSFX;
		private const float SFX_EMITT_RADIUS = 100f;
		#endregion Class variables

		#region Class propeties
		public Stage LifeStage { get { return this.lifeStage; } set { this.lifeStage = value; } }
		public int Points { get; set; }
		public float SpeedMultiplier { get; set; }
		public bool Release { get; set; }
		#endregion Class properties

		#region Constructor
		public Food(ContentManager content, Random rand, int points, float speedMultiplier, List<string> dyingCharacterTextureNames, string deathParticleTextureName,
			string idleTextureName, string spawnTextureName, string spawnSFXName, string idleSFXName, string dyingSFXName, float spawnPositionYOffset = 0f) 
			: base(content) {
			this.Points = points;
			this.SpeedMultiplier = speedMultiplier;
			this.LifeStage = Stage.Spawn;

			this.dyingCharacterTextures = new List<Texture2D>();
			foreach (string texture in dyingCharacterTextureNames) {
				this.dyingCharacterTextures.Add(LoadingUtils.load<Texture2D>(content, texture));
			}
			this.deathParticleTexture = LoadingUtils.load<Texture2D>(content, deathParticleTextureName);

			Animated2DSpriteLoadSingleRowBasedOnTexture parms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForward;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 4;
			parms.Position = new Vector2(PositionUtils.getPosition(rand.Next(1, Constants.MAX_X_TILES - 1)),
				PositionUtils.getPosition(rand.Next(1, Constants.MAX_Y_TILES - 1)) + Constants.HUD_OFFSET + Constants.TILE_SIZE / 2 + Constants.OVERLAP);
			parms.Texture = LoadingUtils.load<Texture2D>(content, idleTextureName);
			parms.Scale = new Vector2(.5f);
			parms.Origin = new Vector2(Constants.TILE_SIZE);
			parms.AnimationParams = animationParms;
			this.idleSprite = new Animated2DSprite(parms);

			animationParms.AnimationState = AnimationState.PlayForwardOnce;
			parms.AnimationParams = animationParms;
			parms.Texture = LoadingUtils.load<Texture2D>(content, spawnTextureName);
			parms.Scale = new Vector2(1f);
			parms.Position = new Vector2(parms.Position.X, parms.Position.Y + spawnPositionYOffset);
			this.spawnSprite = new Animated2DSprite(parms);

			base.init(this.spawnSprite);

			this.spawnSFX = LoadingUtils.load<SoundEffect>(content, spawnSFXName);
			this.idleSFX = LoadingUtils.load<SoundEffect>(content, idleSFXName);
			this.dyingSFX = LoadingUtils.load<SoundEffect>(content, dyingSFXName);

			SoundEmitterParams sfxEmitterParms = new SoundEmitterParams {
				SFXEngine = SoundManager.getInstance().SFXEngine,
				EmittRadius = SFX_EMITT_RADIUS,
				Position = this.spawnSprite.Position
			};
			this.sfxEmitter = new SoundEmitter(sfxEmitterParms);
			SoundManager.getInstance().addEmitter(this.sfxEmitter);
		}
		#endregion Constructor

		#region Support methods
		protected abstract void createIdleEmitter();

		public virtual void handleCollision(Vector2 heading) {
			this.LifeStage = Stage.Dying;
			BaseParticle2DEmitterParams parms = new BaseParticle2DEmitterParams();
			parms.ParticleTexture = this. deathParticleTexture;
			this.deathEmitter = new DeathParticleEmitter(parms, base.Position, heading, this.dyingCharacterTextures);
			base.init(null);
		}

		public bool wasCollision(BoundingBox bbox, Vector2 heading) {
			bool collision = false;
			if (this.LifeStage == Stage.Idle) {
				if (base.BBox.Intersects(bbox)) {
					handleCollision(heading);
					this.idleEmitter = null;
					collision = true;
					SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.dyingSFX);
				}
			}
			return collision;
		}

		public override void update(float elapsed) {
			if (this.idleEmitter != null) {
				this.idleEmitter.update(elapsed);
			}

			if (this.LifeStage == Stage.Spawn) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					if (!this.lapseTime) {
						SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.spawnSFX);
					}
					this.elapsedTime += elapsed;
					this.lapseTime = true;
				}
				
				if (this.elapsedTime >= Constants.SHOW_SPAWN_FOR) {
					base.init(this.idleSprite);
					this.LifeStage = Stage.Idle;
					this.lapseTime = false;
					this.elapsedTime = 0f;
					createIdleEmitter();
				}
			} else if (this.LifeStage == Stage.Dying) {
				this.elapsedTime += elapsed;
				if (this.elapsedTime >= Constants.DEATH_DURATION) {
					this.Release = true;
				}
				if (this.deathEmitter != null) {
					this.deathEmitter.update(elapsed);
				}
			}
			base.update(elapsed);
		}

		public override void render(SpriteBatch spriteBatch) {
			if (StateManager.getInstance().CurrentGameState == GameState.Active) {
				if (this.lifeStage == Stage.Idle) {
					if (this.idleEmitter != null) {
						this.idleEmitter.render(spriteBatch);
					}
				}

				base.render(spriteBatch);
				if (this.lifeStage == Stage.Spawn && lapseTime) {
					this.idleSprite.render(spriteBatch);
				}
				if (this.deathEmitter != null) {
					this.deathEmitter.render(spriteBatch);
				}
			}

#if DEBUG
			if (Display.GameDisplay.debugOn) {
				DebugUtils.drawRadius(spriteBatch, this.sfxEmitter.Position, Display.GameDisplay.radiusTexture, Constants.DEBUG_RADIUS_COLOUR);
			}
#endif
		}

		~Food() {
			if (this.sfxEmitter != null) {
				SoundManager.getInstance().removeEmitter(this.sfxEmitter);
			}
		}
		#endregion Support methods
	}
}
