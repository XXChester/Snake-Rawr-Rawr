
using GWNorthEngine.Audio;
using GWNorthEngine.Audio.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeRawrRawr.Logic;
using SnakeRawrRawr.Logic.Generator;

namespace SnakeRawrRawr.Model {
	public class Wall : Entity {
		private enum Stage { Opening, Idle, Closing };
		#region Class variables
		private float timeAlive;
		private Stage stage;
		private DustParticleEmitter dustEmitter;
		private Animated2DSprite spawnSprite;
		private StaticDrawable2D idleImage;
		private SoundEmitter sfxEmitter;
		private SoundEffect crumpleSFX;
		private const string SFX_NAME_CRUMPLE = "Crumple";
		private const float TIME_TO_LIVE = 12000f;
		private const float SFX_EMITT_RADIUS = 50f;
		#endregion Class variables

		#region Class propeties
		public bool Release { get; set; }
		#endregion Class properties

		#region Constructor
		public Wall(ContentManager content, Vector2 position)
			:base(content, true) {

			Texture2D texture = null;
			texture = LoadingUtils.load<Texture2D>(content, "Fence");

			StaticDrawable2DParams wallParams = new StaticDrawable2DParams {
				Position = position,
				Texture = texture,
				Scale = new Vector2(.5f),
				Origin = new Vector2(Constants.TILE_SIZE)
			};
			this.idleImage = new StaticDrawable2D(wallParams);

			Animated2DSpriteLoadSingleRowBasedOnTexture parms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForwardOnce;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 3;
			parms.Position = position;
			parms.Scale = new Vector2(.25f);
			parms.Texture = LoadingUtils.load<Texture2D>(content, "FenceOpening");
			parms.Origin = new Vector2(Constants.TILE_SIZE);
			parms.AnimationParams = animationParms;
			this.spawnSprite = new Animated2DSprite(parms);
			base.init(spawnSprite);

			BaseParticle2DEmitterParams emitterParams = new BaseParticle2DEmitterParams() {
				ParticleTexture = LoadingUtils.load<Texture2D>(content, "Dust"),
			};
			this.dustEmitter = new DustParticleEmitter(emitterParams, base.Position);
			for (int i = 0; i < 5; i++) {
				this.dustEmitter.createParticle();
			}

			this.crumpleSFX = LoadingUtils.load<SoundEffect>(content, SFX_NAME_CRUMPLE);
			SoundEmitterParams sfxEmitterParms = new SoundEmitterParams {
				SFXEngine = SoundManager.getInstance().SFXEngine,
				EmittRadius = SFX_EMITT_RADIUS,
				Position = this.spawnSprite.Position
			};
			this.sfxEmitter = new SoundEmitter(sfxEmitterParms);
			SoundManager.getInstance().addEmitter(this.sfxEmitter);
			SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.crumpleSFX);
			this.stage = Stage.Opening;
		}
		#endregion Constructor

		#region Support methods
		public override void update(float elapsed) {
			this.timeAlive += elapsed;
			base.update(elapsed);
			this.dustEmitter.update(elapsed);

			PositionGenerator.getInstance().markPosition(base.Position, false);
			if (this.stage == Stage.Opening) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					this.stage = Stage.Idle;
					base.init(this.idleImage);
				}
			} else if (this.stage == Stage.Closing) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					this.Release = true;
				}
			} else {
				if (this.timeAlive >= TIME_TO_LIVE) {
					this.stage = Stage.Closing;
					this.spawnSprite.AnimationManager.State = AnimationState.PlayReversedOnce;
					this.spawnSprite.reset();
					base.init(this.spawnSprite);
					PositionGenerator.getInstance().markPosition(base.Position);
					SoundManager.getInstance().playSoundEffect(this.sfxEmitter, this.crumpleSFX);
				}
			}
		}

		public override void render(SpriteBatch spriteBatch) {
			base.render(spriteBatch);
			if (this.stage == Stage.Idle) {
				this.dustEmitter.render(spriteBatch);
			}
		}

		~Wall() {
			if (this.sfxEmitter != null) {
				SoundManager.getInstance().removeEmitter(this.sfxEmitter);
			}
		}
		#endregion Support methods
	}
}
