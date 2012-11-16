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
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Logic;
using GWNorthEngine.Logic.Params;
using GWNorthEngine.Input;
using GWNorthEngine.Utils;
using GWNorthEngine.Scripting;

namespace SnakeRawrRawr.Model {
	public class ConstantSpeedParticleEmitter : AutoParticle2DEmitter {
		#region Class variables
		private Vector2 position;
		private Vector2 origin;
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
		public ConstantSpeedParticleEmitter(BaseParticle2DEmitterParams parms, Vector2 position, Vector2 origin)
			: base(parms) {

			this.position = position;
			this.origin = origin;
		}
		#endregion Constructor

		#region Support methods
		public override void createParticle() {
			Vector2 heading = HEADINGS[base.RANDOM.Next(HEADINGS.Length)];

			BaseParticle2DParams particleParms = new BaseParticle2DParams();
			particleParms.TimeToLive = 250;
			particleParms.Origin = this.origin;
			particleParms.Texture = base.particleTexture;
			particleParms.Scale = new Vector2(.5f);
			particleParms.Position = this.position;
			particleParms.Direction = heading;
			particleParms.Acceleration = new Vector2(50f, 50f);

			ConstantSpeedParticle particle = new ConstantSpeedParticle(particleParms);
			ScaleOverTimeEffectParams effectParms = new ScaleOverTimeEffectParams {
				Reference = particle,
				ScaleBy = new Vector2(.05f)
			};
			particle.addEffect(new ScaleOverTimeEffect(effectParms));
			base.particles.Add(particle);

			base.createParticle();
		}
		#endregion Support methods
	}
}
