﻿using System;
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

namespace SnakeRawrRawr.Model {
	public class DeathParticleEmitter : BaseParticle2DEmitter {
		#region Class variables
		private Vector2 position;
		private const int MAX_RANGE_FROM_EMITTER = 15;
		private const int MAX_SCALE = 70;
		private const int MIN_SCALE = 5;
		#endregion Class variables

		#region Class propeties

		#endregion Class properties

		#region Constructor
		public DeathParticleEmitter(BaseParticle2DEmitterParams parms, Vector2 position, Vector2 heading, List<Texture2D> characterTextures)
			: base(parms) {

			this.position = position;
			BaseParticle2DParams particleParms = new BaseParticle2DParams();
			particleParms.TimeToLive = 350;
			particleParms.Acceleration = new Vector2(100f);
			particleParms.Direction = heading;
			particleParms.Origin = new Vector2(Constants.TILE_SIZE);
			particleParms.Texture = parms.ParticleTexture;
			base.particleParams = particleParms;

			// create additional effects
			for (int i = 0; i < 20; i++) {
				createParticle();
			}

			particleParms.Scale = new Vector2(.5f);
			particleParms.Position = position;
			foreach (Texture2D texture in characterTextures) {
				particleParms.Texture = texture;
				base.particles.Add(new DeacceleratingParticle(particleParms));
			}
		}
		#endregion Constructor

		#region Support methods
		public override void createParticle() {
			int positionX = base.RANDOM.Next(MAX_RANGE_FROM_EMITTER);
			int positionY = base.RANDOM.Next(MAX_RANGE_FROM_EMITTER);
			int directionX = base.RANDOM.Next(2);
			int directionY = base.RANDOM.Next(2);
			float x, y;
			if (directionX == 0) {
				x = this.position.X + positionX;
			} else {
				x = this.position.X - positionX;
			}

			if (directionY == 0) {
				y = this.position.Y + positionY;
			} else {
				y = this.position.Y - positionY;
			}

			base.particleParams.Scale = new Vector2(base.RANDOM.Next(MIN_SCALE, MAX_SCALE) / 100f);
			base.particleParams.Position = new Vector2(x, y);
			base.particles.Add(new DeacceleratingParticle(base.particleParams));
			base.createParticle();
		}
		#endregion Support methods
	}
}