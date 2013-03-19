﻿
using GWNorthEngine.Model;
using GWNorthEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SnakeRawrRawr.Logic;

namespace SnakeRawrRawr.Model {
	public abstract class Entity {
		#region Class variables
		private Base2DSpriteDrawable image;
		protected ContentManager content;
		protected bool alwaysRender;
#if DEBUG
		private Texture2D debugLine;
#endif
		#endregion Class variables

		#region Class propeties
		public BoundingBox BBox { get; set; }
		public float Rotation {
			get { return this.image.Rotation; }
			set { this.image.Rotation = value; }
		}
		public Vector2 Scale {
			get { return this.image.Scale; }
			set { this.image.Scale = value; }
		}
		public Vector2 Position {
			get { return this.image.Position; }
			set {
				this.image.Position = value;
				this.BBox = getBBox();
			}
		}
		#endregion Class properties

		#region Constructor
		public Entity(ContentManager content, bool alwaysRender=false) : this(content, null, alwaysRender) { }
		public Entity(ContentManager content, Base2DSpriteDrawable image, bool alwaysRender = false) {
			this.content = content;
			this.alwaysRender = alwaysRender;
			init(image);
#if DEBUG
			this.debugLine = LoadingUtils.load<Texture2D>(content, "Chip");
#endif
		}
		#endregion Constructor

		#region Support methods
		protected void init(Base2DSpriteDrawable image) {
			this.image = image;
			if (this.image != null) {
				this.Position = this.image.Position;
			}
		}

		protected virtual BoundingBox getBBox() {
			return PositionUtils.getBBox(this.Position);
		}

		public virtual void update(float elapsed) {
			if (this.image != null) {
				this.image.update(elapsed);
			}
		}

		public virtual void render(SpriteBatch spriteBatch) {
			if (StateManager.getInstance().CurrentGameState == GameState.Active || alwaysRender) {
				if (this.image != null) {
					this.image.render(spriteBatch);
				}
			}
#if DEBUG
			if (Model.Display.GameDisplay.debugOn) {
				DebugUtils.drawBoundingBox(spriteBatch, this.BBox, Constants.DEBUG_BBOX_Color, debugLine);
			}
#endif
		}
		#endregion Support methods
	}
}
