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
		private const int VALUE = 30;
		private const float SPEED_MULTIPLIER = 7.5f;
		private const float IDLE_SFX_EMIT_RADIUS = 75f;
		private const string TEXTURE_NAME_DEATH_PARTICLE = "Blood";
		private const string TEXTURE_NAME_SPAWN = "Egg";
		private const string TEXTURE_NAME_IDLE = "Chicken";
		private const string SFX_NAME_SPAWN = "ChickenSpawn";
		private const string SFX_NAME_IDLE = "ChickenIdle";
		private const string SFX_NAME_DIE = "ChickenDie";
		private static List<string> DYING_CHAR_TEXTURE_NAMES = new List<string> {
			"ChickenHead", "ChickenLeftArm", "ChickenRightArm", "ChickenLeftLeg", "ChickenRightLeg"
		};
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public Chicken(ContentManager content, Random rand)
			: base(content, rand, VALUE, SPEED_MULTIPLIER, DYING_CHAR_TEXTURE_NAMES, TEXTURE_NAME_DEATH_PARTICLE, TEXTURE_NAME_IDLE, TEXTURE_NAME_SPAWN, SFX_NAME_SPAWN,
				SFX_NAME_IDLE, SFX_NAME_DIE) { }
		#endregion Constructor

		#region Support methods
		protected override void createIdleEmitter() {
			Vector2 origin = new Vector2(Constants.TILE_SIZE, Constants.TILE_SIZE*2);
			BaseParticle2DEmitterParams emitterParms = new BaseParticle2DEmitterParams();
			emitterParms.ParticleTexture = LoadingUtils.load<Texture2D>(content, "Bawk");
			emitterParms.SpawnDelay = 4000f;

			base.idleEmitter = new ConstantSpeedParticleEmitter(emitterParms, base.Position, origin, base.idleSFX, IDLE_SFX_EMIT_RADIUS, false);
		}
		#endregion Support methods
	}
}
