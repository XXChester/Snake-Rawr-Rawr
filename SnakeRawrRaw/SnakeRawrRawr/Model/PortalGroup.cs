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

using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model {
	public class PortalGroup {
		#region Class variables
		private ContentManager content;
		private Random rand;
		private float elapsed;
		private const float SPAWN_INTERVAL = 5000f;
		private const int POINTS = 30;
		#endregion Class variables

		#region Class propeties
		public Portal One { get; set; }
		public Portal Two { get; set; }
		public WarpCoordinates WarpCoords { get; set; }
		public int Points { get { return POINTS; } }
		#endregion Class properties

		#region Constructor
		public PortalGroup(ContentManager content, Random rand) {
			this.content = content;
			this.rand = rand;
			create();
		}
		#endregion Constructor

		#region Support methods
		private void create() {
			this.One = new Portal(this.content, this.rand);
			this.Two = new Portal(this.content, this.rand);
			this.elapsed = 0f;
		}

		public bool wasCollision(BoundingBox bbox, Vector2 snakesPosition) {
			bool collision = false;
			if (this.One != null) {
				if (this.One.wasCollision(bbox)) {
					collision = true;
					if (this.Two != null) {
						this.WarpCoords = new WarpCoordinates(snakesPosition, this.Two.Position);
						this.Two.LifeStage = Portal.Stage.Exit;
					}
				}
			}
			if (this.Two != null) {
				if (this.Two.wasCollision(bbox)) {
					collision = true;
					if (this.One != null) {
						this.WarpCoords = new WarpCoordinates(snakesPosition, this.One.Position);
						this.One.LifeStage = Portal.Stage.Exit;
					}
				}
			}
			return collision;
		}

		public bool wasClosingCollision(BoundingBox tailBBox) {
			bool collision = false;
			if (this.One != null) {
				if (this.One.wasTailCollision(tailBBox)) {
					collision = true;
				}
			}
			if (this.Two != null) {
				if (this.Two.wasTailCollision(tailBBox)) {
					collision = true;
				}
			}
			return collision;
		}

		public void update(float elapsed) {
			if (this.One != null) {
				this.One.update(elapsed);
				if (this.One.Release) {
					this.One = null;
				}
			}
			if (this.Two != null) {
				this.Two.update(elapsed);
				if (this.Two.Release) {
					this.Two = null;
				}
			}
			if (this.One == null && this.Two == null) {
				this.elapsed += elapsed;
				this.WarpCoords = null;
				if (this.elapsed >= SPAWN_INTERVAL) {
					create();
				}
			}
		}

		public void render(SpriteBatch spriteBatch) {
			if (this.One != null) {
				this.One.render(spriteBatch);
			}
			if (this.Two != null) {
				this.Two.render(spriteBatch);
			}
		}
		#endregion Support methods
	}
}