using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using GWNorthEngine.Model.Effects;
using GWNorthEngine.Model.Effects.Params;
using GWNorthEngine.Utils;
namespace SnakeRawrRawr.Model {
	public class DustParticleEmitter : BaseParticle2DEmitter {
		#region Class variables
		private readonly Vector2 POSITION;
		private const float TIME_TO_LIVE = 700f;
		private const int MAX_RANGE_FROM_EMITTER = 3;
		public static Color COLOUR = new Color(210, 200, 190);
		#endregion Class variables

		#region Class properties

		#endregion Class properties

		#region Constructor
		public DustParticleEmitter(BaseParticle2DEmitterParams parms, Vector2 position)
			: base(parms) {
			BaseParticle2DParams particleParams = new BaseParticle2DParams();
			particleParams.Scale = new Vector2(.25f);
			particleParams.Origin = new Vector2(32f, 32f);
			particleParams.Texture = parms.ParticleTexture;
			particleParams.LightColour = COLOUR;
			particleParams.TimeToLive = TIME_TO_LIVE;
			base.particleParams = particleParams;
			this.POSITION = position;
		}
		#endregion Constructor

		#region Support methods
		public void updateColours(Color colour) {
			if (base.particles != null) {
				foreach (BaseParticle2D particle in base.particles) {
					particle.LightColour = colour;
				}
			}
		}

		public override void createParticle() {
			int positionX = this.RANDOM.Next(MAX_RANGE_FROM_EMITTER);
			int positionY = this.RANDOM.Next(MAX_RANGE_FROM_EMITTER);
			int directionX = this.RANDOM.Next(2);
			int directionY = this.RANDOM.Next(2);
			float x, y;
			if (directionX == 0) {
				x = POSITION.X + positionX;
			} else {
				x = POSITION.X - positionX;
			}

			if (directionY == 0) {
				y = POSITION.Y + positionY;
			} else {
				y = POSITION.Y - positionY;
			}

			base.particleParams.Position = new Vector2(x, y);
			BaseParticle2D particle = new BaseParticle2D(base.particleParams);
			ScaleOverTimeEffectParams effectParms = new ScaleOverTimeEffectParams {
				ScaleBy = new Vector2(.5f)
			};
			particle.addEffect(new ScaleOverTimeEffect(effectParms));
			RotateOverTimeEffectParams rotateEffectParms = new RotateOverTimeEffectParams {
				RotateBy = 2f
			};
			particle.addEffect(new RotateOverTimeEffect(rotateEffectParms));
			FadeEffectParams fadeEffectParms = new FadeEffectParams {
				State = FadeEffect.FadeState.Out,
				TotalTransitionTime = TIME_TO_LIVE,
				OriginalColour = COLOUR
			};
			particle.addEffect(new FadeEffect(fadeEffectParms));
			base.particles.Add(particle);
			base.createParticle();
		}
		#endregion Support methods
	}
}
