﻿
using Microsoft.Xna.Framework;


namespace SnakeRawrRawr {
	public class Constants {
		public const float TILE_SIZE = 16f;
		public const float OVERLAP = 2f;
		public const float HUD_OFFSET = 50f;
		public const float DEATH_DURATION = 5000f;// 5 seconds
		public const float SHOW_SPAWN_FOR = 800f;
		public const float GENERATION_OVERLAP = TILE_SIZE / 2f;
		public const float GENERATION_Y_SIZE = Constants.HUD_OFFSET + Constants.GENERATION_OVERLAP;
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
		public static Color TEXT_COLOUR = Color.Red;

#if DEBUG
		public static Color DEBUG_BBOX_Color = Color.Red;
		public static Color DEBUG_PIVOT_Color = Color.White;
		public static Color DEBUG_RADIUS_COLOUR = Color.LightPink;
#endif
	}
}
