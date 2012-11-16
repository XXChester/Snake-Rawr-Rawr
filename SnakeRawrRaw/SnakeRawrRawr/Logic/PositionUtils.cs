using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

using GWNorthEngine.Engine;
namespace SnakeRawrRawr.Logic {
	public class PositionUtils {
		public static float getPosition(int index) {
			return (index * Constants.TILE_SIZE);
		}

		public static Vector2 getDelta(Vector2 heading, float distance) {
			Vector2 delta = new Vector2();
			if (heading == Constants.HEADING_UP) {
				delta = new Vector2(0f, -distance);
			} else if (heading == Constants.HEADING_DOWN) {
				delta = new Vector2(0f, distance);
			} else if (heading == Constants.HEADING_LEFT) {
				delta = new Vector2(-distance, 0f);
			} else if (heading == Constants.HEADING_RIGHT) {
				delta = new Vector2(distance, 0f);
			}
			return delta;
		}

		public static float getRotation(Vector2 currentHeading, Vector2 newHeading) {
			float rotation = 90f;
			if (newHeading == Constants.HEADING_LEFT) {
				if (currentHeading == Constants.HEADING_UP) {
					rotation = -rotation;
				}
			} else if (newHeading == Constants.HEADING_RIGHT) {
				if (currentHeading == Constants.HEADING_DOWN) {
					rotation = -rotation;
				}
			} else if (newHeading == Constants.HEADING_UP) {
				if (currentHeading == Constants.HEADING_RIGHT) {
					rotation = -rotation;
				}
			} else if (newHeading == Constants.HEADING_DOWN) {
				if (currentHeading == Constants.HEADING_LEFT) {
					rotation = -rotation;
				}
			}
			return rotation;
		}

		public static void getMinMax(Vector2 vector1, Vector2 vector2, out Vector2 minVector, out Vector2 maxVector) {
			minVector = new Vector2(MathHelper.Min(vector1.X, vector2.X), MathHelper.Min(vector1.Y, vector2.Y));
			maxVector = new Vector2(MathHelper.Max(vector1.X, vector2.X), MathHelper.Max(vector1.Y, vector2.Y));
		}

		public static void handleChildMovement(float distance, ref Vector2 heading, ref Vector2 position, ref float rotation, ref List<PivotPoint> pivotPoints) {
			Vector2 currentPosition = position;
			Vector2 delta = PositionUtils.getDelta(heading, distance);
			if (pivotPoints != null && pivotPoints.Count > 0) {
				// for this calculation to see if we are at the pivot node, we need to work with positive only numbers
				float y = delta.Y;
				float x = delta.X;
				if (delta.Y < 0) {
					y = -y;
				}
				if (delta.X < 0) {
					x = -x;
				}
				// the first node is our current target
				PivotPoint pivotPoint = pivotPoints[0];
				Vector2 minVector, maxVector;
				PositionUtils.getMinMax(pivotPoint.Position, currentPosition, out minVector, out maxVector);
				Vector2 distancePosPivot = Vector2.Subtract(maxVector, minVector);
				if (x >= distancePosPivot.X && y >= distancePosPivot.Y) {
					Vector2 deltaMinusPivotDistance1 = Vector2.Subtract(delta, distancePosPivot);
					// we are either at or going to pass through the pivot point so we need to set to pivot point + the extra distance
					PositionUtils.getMinMax(new Vector2(x,y), distancePosPivot, out minVector, out maxVector);
					Vector2 deltaMinusPivotDistance = Vector2.Subtract(maxVector, minVector);

					// we need to apply the remainder to the new direction
					position = pivotPoint.Position;
					if (heading == Constants.HEADING_DOWN || heading == Constants.HEADING_UP) {
						delta = PositionUtils.getDelta(pivotPoint.Heading, deltaMinusPivotDistance.Y);
						position = new Vector2(position.X + delta.X, position.Y);
					} else if (heading == Constants.HEADING_LEFT || heading == Constants.HEADING_RIGHT) {
						delta = PositionUtils.getDelta(pivotPoint.Heading, deltaMinusPivotDistance.X);
						position = new Vector2(position.X, position.Y   + delta.Y);
					}
					rotation += MathHelper.ToRadians(pivotPoint.Rotation);
					heading = pivotPoint.Heading;

					// lastly remove the pivot point
					pivotPoints.RemoveAt(0);
				} else {
					position += delta;
				}
			} else {
				position += delta;
			}
		}

		public static BoundingBox getBBox(Vector2 position) {
			float half = Constants.TILE_SIZE / 2f;
			return new BoundingBox(
				new Vector3(Vector2.Subtract(position, new Vector2(half)), 0f),
				new Vector3(Vector2.Add(position, new Vector2(half)), 0f));
		}
	}
}