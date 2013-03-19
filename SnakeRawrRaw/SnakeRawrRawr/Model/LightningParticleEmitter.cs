using System;
using GWNorthEngine.Model;
using GWNorthEngine.Model.Params;
using Microsoft.Xna.Framework;


namespace SnakeRawrRawr.Model {
	public class LightningParticleEmitter : AutoParticle2DEmitter {
		#region Class variables
		private Vector2 position;
		#endregion Class variables

		#region Class propeties
		public bool Emitt { get; set; }
		#endregion Class properties

		#region Constructor
		public LightningParticleEmitter(BaseParticle2DEmitterParams parms, Vector2 position)
			: base(parms) {
			this.position = position;
			this.Emitt = true;
		}
		#endregion Constructor

		#region Support methods
		public override void createParticle() {
			if (Emitt) {
				float x = RANDOM.Next(0, 180);
				if (RANDOM.Next(0, 2) == 1) {
					x = -x;
				}
				float y = RANDOM.Next(0, 180);
				if (RANDOM.Next(0, 2) == 1) {
					y = -y;
				}
				float rotation = (float)Math.Atan2(x, -y);

				BaseParticle2DParams particleParms = new BaseParticle2DParams();
				particleParms.TimeToLive = 1000;
				particleParms.Texture = base.particleTexture;
				particleParms.Scale = new Vector2(.75f);
				particleParms.Position = this.position;
				particleParms.Origin = new Vector2(Constants.TILE_SIZE / 2);
				particleParms.Direction = new Vector2(x, y);
				particleParms.Acceleration = new Vector2(.15f);
				particleParms.Rotation = rotation;
				particleParms.LightColour = Color.White;


				ConstantSpeedParticle particle = new ConstantSpeedParticle(particleParms);
				base.particles.Add(particle);
				base.createParticle();
			}
		}
		#endregion Support methods
	}
}
