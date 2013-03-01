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
using SnakeRawrRawr.Model.Display;

namespace SnakeRawrRawr.Model {
	public class Portal : Entity {
		public enum Stage { Spawn, Idle, Entered, Exit, Closing }
		#region Class variables
		private float elapsedTime;
		private Stage lifeStage;
		private SoundEffect spawnSFX;
		private SoundEffect idleSFX;
		private SoundEffect closingSFX;
		private SoundEmitter sfxEmitter;
		private Animated2DSprite spawnSprite;
		private Animated2DSprite idleSprite;
		private const float SFX_EMITT_RADIUS = 75f;
		private const float MAX_OPEN_TIME = 14000f;
		private const string IDLE_SFX_NAME = "PortalIdleSFX";
		private const string SPAWN_SFX_NAME = "PortalSpawnSFX";
		private const string CLOSING_SFX_NAME = "PortalCloseSFX";
		#endregion Class variables

		#region Class propeties
		public Stage LifeStage { get { return this.lifeStage; } set { this.lifeStage = value; } }
		public bool Release { get; set; }
		#endregion Class properties

		#region Constructor
		public Portal(ContentManager content, Random rand) 
			: base(content) {
			this.lifeStage = Stage.Spawn;

			Animated2DSpriteLoadSingleRowBasedOnTexture parms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForward;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 7;
			parms.Position = new Vector2(PositionUtils.getPosition(rand.Next(1, Constants.MAX_X_TILES - 1)),
				PositionUtils.getPosition(rand.Next(1, Constants.MAX_Y_TILES - 1)) + Constants.HUD_OFFSET + Constants.TILE_SIZE / 2 + Constants.OVERLAP);
			parms.Texture = LoadingUtils.load<Texture2D>(content, "PortalOpen");
			parms.Origin = new Vector2(Constants.TILE_SIZE);
			parms.AnimationParams = animationParms;
			this.idleSprite = new Animated2DSprite(parms);

			animationParms.AnimationState = AnimationState.PlayForwardOnce;
			animationParms.TotalFrameCount = 4;
			parms.AnimationParams = animationParms;
			parms.Texture = LoadingUtils.load<Texture2D>(content, "PortalSpawn");
			parms.Position = new Vector2(parms.Position.X, parms.Position.Y);
			this.spawnSprite = new Animated2DSprite(parms);

			base.init(this.spawnSprite);

			this.spawnSFX = LoadingUtils.load<SoundEffect>(content, SPAWN_SFX_NAME);
			this.idleSFX = LoadingUtils.load<SoundEffect>(content, IDLE_SFX_NAME);
			this.closingSFX = LoadingUtils.load<SoundEffect>(content, CLOSING_SFX_NAME);

			SoundEmitterParams sfxEmitterParms = new SoundEmitterParams {
				SFXEngine = SoundManager.getInstance().SFXEngine,
				EmittRadius = SFX_EMITT_RADIUS,
				Position = this.spawnSprite.Position
			};
			this.sfxEmitter = new SoundEmitter(sfxEmitterParms);
			SoundManager.getInstance().addEmitter(this.sfxEmitter);
			SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.spawnSFX);
		}
		#endregion Constructor

		#region Support methods
		public void handleCollision() {
			this.lifeStage = Stage.Entered;
		}

		public bool wasCollision(BoundingBox bbox) {
			bool collision = false;
			if (this.LifeStage == Stage.Idle) {
				if (base.BBox.Intersects(bbox)) {
					handleCollision();
					collision = true;
					//this.sfxEmitter.playSoundEffect(this.enteredSFX, this.sfxEmitter.Position);
				}
			}
			return collision;
		}

		public bool wasTailCollision(BoundingBox bbox) {
			bool collision = false;
			// can we close it
			if (this.lifeStage == Stage.Entered || this.lifeStage == Stage.Exit) {
				if (base.BBox.Intersects(bbox)) {
					this.lifeStage = Stage.Closing;
					this.spawnSprite.AnimationManager.State = AnimationState.PlayReversedOnce;
					base.init(this.spawnSprite);
					SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.closingSFX);
					SoundManager.getInstance().SFXEngine.stop(IDLE_SFX_NAME);
				}
			}
			return collision;
		}

		public override void update(float elapsed) {
			// REMOVE ME!!!!
			if (InputManager.getInstance().wasKeyPressed(Keys.D9)) {
				SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.spawnSFX);
			}
			this.elapsedTime += elapsed;
			if (this.LifeStage == Stage.Spawn) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					this.lifeStage = Stage.Idle;
					this.idleSprite.AnimationManager.State = AnimationState.PlayForward;
					base.init(this.idleSprite);
					SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.idleSFX, true);
				}
			}
			if (this.lifeStage == Stage.Idle) {
				if (this.elapsedTime >= MAX_OPEN_TIME) {
					this.lifeStage = Stage.Closing;
					this.spawnSprite.AnimationManager.State = AnimationState.PlayReversedOnce;
					base.init(this.spawnSprite);
					SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.closingSFX);
					SoundManager.getInstance().SFXEngine.stop(IDLE_SFX_NAME);
				}
			}
			if (this.lifeStage == Stage.Closing) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					this.Release = true;
					base.init(null);
				}
			}
			base.update(elapsed);
		}

#if DEBUG
		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);
			if (GameDisplay.debugOn) {
				DebugUtils.drawRadius(spriteBatch, this.sfxEmitter.Position, Display.GameDisplay.radiusTexture, Color.Purple);
			}
		}
#endif

		~Portal() {
			if (this.sfxEmitter != null) {
				SoundManager.getInstance().removeEmitter(this.sfxEmitter);
			}
		}
		#endregion Support methods
	}
}
