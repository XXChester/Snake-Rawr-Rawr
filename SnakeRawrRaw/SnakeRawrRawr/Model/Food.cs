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
	public abstract class Food : Entity {
		public enum Stage { Spawn, Idle, Dying }
		#region Class variables
		private float elapsedTime;
		private Stage previousLifeStage;
		private Stage lifeStage;
		private bool lapseTime;
		protected Animated2DSprite spawnSprite;
		protected Animated2DSprite idleSprite;
		protected DeathParticleEmitter deathEmitter;
		protected BaseParticle2DEmitter idleEmitter;
		protected List<Texture2D> dyingCharacterTextures;
		#endregion Class variables

		#region Class propeties
		public Stage LifeStage {
			get { return this.lifeStage; }
			set { this.previousLifeStage = this.lifeStage; this.lifeStage = value; }
		}
		public int Points { get; set; }
		public float SpeedMultiplier { get; set; }
		public bool Release { get; set; }
		#endregion Class properties

		#region Constructor
		public Food(ContentManager content, int points, float speedMultiplier) : base(content) {
			this.Points = points;
			this.SpeedMultiplier = speedMultiplier;
			this.LifeStage = Stage.Spawn;
		}
		#endregion Constructor

		#region Support methods
		protected abstract void createIdleEmitter();
		public abstract void handleCollision(Vector2 heading);

		public bool wasCollision(BoundingBox bbox, Vector2 heading) {
			bool collision = false;
			if (this.LifeStage == Stage.Idle) {
				if (base.BBox.Intersects(bbox)) {
					handleCollision(heading);
					this.idleEmitter = null;
					collision = true;
				}
			}
			return collision;
		}

		public override void update(float elapsed) {
			if (this.previousLifeStage != this.LifeStage) {
				this.elapsedTime = 0f;
			}

			if (this.idleEmitter != null) {
				this.idleEmitter.update(elapsed);
			}

			if (this.LifeStage == Stage.Spawn) {
				if (this.spawnSprite.AnimationManager.State == AnimationState.Paused) {
					this.elapsedTime += elapsed;
					this.lapseTime = true;
					createIdleEmitter();
				}
				
				if (this.elapsedTime >= Constants.SHOW_SPAWN_FOR) {
					base.init(this.idleSprite);
					this.LifeStage = Stage.Idle;
					this.lapseTime = false;
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
			base.render(spriteBatch);
			if (this.lifeStage == Stage.Spawn && lapseTime) {
				this.idleSprite.render(spriteBatch);
			}
			if (this.idleEmitter != null) {
				this.idleEmitter.render(spriteBatch);
			}
			if (this.deathEmitter != null) {
				this.deathEmitter.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}
