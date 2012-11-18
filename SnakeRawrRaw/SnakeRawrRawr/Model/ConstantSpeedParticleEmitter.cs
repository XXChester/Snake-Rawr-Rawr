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

namespace SnakeRawrRawr.Model {
	public class ConstantSpeedParticleEmitter : AutoParticle2DEmitter {
		#region Class variables
		private bool sway;
		private Vector2 position;
		private Vector2 origin;
		private SoundEffect idleSFX;
		private SoundEmitter sfxEmitter;
		private const float VERTICLE_HEADING = -1f;
		private readonly Vector2[] HEADINGS = new Vector2[] { 
			new Vector2(-1f, VERTICLE_HEADING), 
			new Vector2(-.5f, VERTICLE_HEADING), 
			new Vector2(1f, VERTICLE_HEADING), 
			new Vector2(.5f, VERTICLE_HEADING), 
			new Vector2(0f, VERTICLE_HEADING) 
		};
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public ConstantSpeedParticleEmitter(BaseParticle2DEmitterParams parms, Vector2 position, Vector2 origin, SoundEffect idleSFX, float emittRadius, bool sway=true)
			: base(parms) {

			this.position = position;
			this.origin = origin;
			this.sway = sway;
			this.idleSFX = idleSFX;

			SoundEmitterParams sfxEmitterParms = new SoundEmitterParams {
				EmittRadius = emittRadius,
				Position = this.position,
				SFXEngine = SoundManager.getInstance().SFXEngine
			};
			this.sfxEmitter = new SoundEmitter(sfxEmitterParms);
			SoundManager.getInstance().addEmitter(this.sfxEmitter);
		}
		#endregion Constructor

		#region Support methods
		public override void createParticle() {
			Vector2 heading = HEADINGS[base.RANDOM.Next(HEADINGS.Length)];

			BaseParticle2DParams particleParms = new BaseParticle2DParams();
			particleParms.TimeToLive = 500;
			particleParms.Origin = this.origin;
			particleParms.Texture = base.particleTexture;
			particleParms.Scale = new Vector2(.5f);
			particleParms.Position = this.position;
			particleParms.Direction = heading;
			particleParms.Acceleration = new Vector2(25f, 35f);

			ConstantSpeedParticle particle = new ConstantSpeedParticle(particleParms);
			ScaleOverTimeEffectParams effectParms = new ScaleOverTimeEffectParams {
				Reference = particle,
				ScaleBy = new Vector2(1.75f)
			};
			particle.addEffect(new ScaleOverTimeEffect(effectParms));

			if (this.sway) {
				PulseDirection direction = EnumUtils.numberToEnum<PulseDirection>(base.RANDOM.Next(2));
				SwayEffectParms swayEffectParms = new SwayEffectParms {
					Reference = particle,
					SwayBy = new Vector2(50f, 0f),
					SwayDownTo = -5f,
					SwayUpTo = 5f,
					Direction = direction
				};
				particle.addEffect(new SwayEffect(swayEffectParms));
			}
			this.sfxEmitter.playSoundEffect(idleSFX, this.position);
			base.particles.Add(particle);

			base.createParticle();
		}
		#endregion Support methods
	}
}
