using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using GWNorthEngine.Engine;

namespace SnakeRawrRawr {
	public class Constants {
		public const float TILE_SIZE = 16f;
		public const float OVERLAP = 2f;
		public const float HUD_OFFSET = 50f;
		public const float DEATH_DURATION = 5000f;// 5 seconds
		public const float SHOW_SPAWN_FOR = 800f;
		public const int MAX_X_TILES = 81;
		public const int MAX_Y_TILES = 45;
		public const int RESOLUTION_X = 1280;
		public const int RESOLUTION_Y = 768;
		public const int EDIBLE_NODES = 3;
		public const int RARE_SPAWN_ODDS = 7; // 1 in 7
		public static Vector2 HEADING_LEFT = new Vector2(-1, 0f);
		public static Vector2 HEADING_RIGHT = new Vector2(1, 0f);
		public static Vector2 HEADING_UP = new Vector2(0, -1f);
		public static Vector2 HEADING_DOWN = new Vector2(0, 1f);
		public static Color SNAKE_LIGHT = Color.LightGreen;

#if DEBUG
		public static Color DEBUG_BBOX_Color = Color.Red;
		public static Color DEBUG_PIVOT_Color = Color.White;
#endif
	}
}
