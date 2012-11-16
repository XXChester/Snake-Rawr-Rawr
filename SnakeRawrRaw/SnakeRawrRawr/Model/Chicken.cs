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

namespace SnakeRawrRawr.Model {
	public class Chicken : Food {
		#region Class variables
		private Texture2D bloodTexture;
		private const int VALUE = 5;
		private const float SPEED_MULTIPLIER = 10f;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Chicken(ContentManager content, Random rand) : base(content, VALUE, SPEED_MULTIPLIER) {
			base.dyingCharacterTextures = new List<Texture2D>{
				LoadingUtils.load<Texture2D>(content, "ChickenHead"),
				LoadingUtils.load<Texture2D>(content, "ChickenLeftArm"),
				LoadingUtils.load<Texture2D>(content, "ChickenRightArm"),
				LoadingUtils.load<Texture2D>(content, "ChickenLeftLeg"),
				LoadingUtils.load<Texture2D>(content, "ChickenRightLeg"),
			};

			this.bloodTexture = LoadingUtils.load<Texture2D>(content, "Blood");

			Animated2DSpriteLoadSingleRowBasedOnTexture parms = new Animated2DSpriteLoadSingleRowBasedOnTexture();
			BaseAnimationManagerParams animationParms = new BaseAnimationManagerParams();
			animationParms.AnimationState = AnimationState.PlayForward;
			animationParms.FrameRate = 100f;
			animationParms.TotalFrameCount = 4;
			parms.Position = new Vector2(PositionUtils.getPosition(rand.Next(1, Constants.MAX_X_TILES - 1)),
				PositionUtils.getPosition(rand.Next(1, Constants.MAX_Y_TILES - 1)) + Constants.HUD_OFFSET + Constants.TILE_SIZE / 2 + Constants.OVERLAP);
			parms.Texture = LoadingUtils.load<Texture2D>(content, "Chicken");
			parms.Scale = new Vector2(.5f);
			parms.Origin = new Vector2(Constants.TILE_SIZE);
			parms.AnimationParams = animationParms;
			base.idleSprite = new Animated2DSprite(parms);

			animationParms.AnimationState = AnimationState.PlayForwardOnce;
			parms.AnimationParams = animationParms;
			parms.Texture = LoadingUtils.load<Texture2D>(content, "Egg");
			parms.Scale = new Vector2(1f);
			base.spawnSprite = new Animated2DSprite(parms);

			base.init(base.spawnSprite);
		}
		#endregion Constructor

		#region Support methods
		protected override void createIdleEmitter() {
			Vector2 origin = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE*2);
			BaseParticle2DEmitterParams emitterParms = new BaseParticle2DEmitterParams();
			emitterParms.ParticleTexture = LoadingUtils.load<Texture2D>(content, "Bawk");
			emitterParms.SpawnDelay = 4000f;
			base.idleEmitter = new ConstantSpeedParticleEmitter(emitterParms, base.Position, origin);
		}

		public override void handleCollision(Vector2 heading) {
			this.LifeStage = Stage.Dying;
			BaseParticle2DEmitterParams parms = new BaseParticle2DEmitterParams();
			parms.ParticleTexture = this.bloodTexture;
			base.deathEmitter = new DeathParticleEmitter(parms, base.Position, heading, base.dyingCharacterTextures);
			base.init(null);
		}
		#endregion Support methods
	}
}
