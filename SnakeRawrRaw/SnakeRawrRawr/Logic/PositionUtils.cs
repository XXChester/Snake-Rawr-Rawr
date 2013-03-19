using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

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

		public static bool goingToBeAtOrPastPoint(Vector2 currentPosition, Vector2 target, ref Vector2 delta, out Vector2 distanceMinusTarget,
			out Vector2 positiveDelta) {
			bool goingToBeAtOrPast = false;
			// for this calculation to see if we are at the target, we need to work with positive only numbers
			float y = delta.Y;
			float x = delta.X;
			if (delta.Y < 0) {
				y = -y;
			}
			if (delta.X < 0) {
				x = -x;
			}
			positiveDelta = new Vector2(x, y);

			Vector2 minVector, maxVector;
			PositionUtils.getMinMax(target, currentPosition, out minVector, out maxVector);
			distanceMinusTarget = Vector2.Subtract(maxVector, minVector);
			if (x >= distanceMinusTarget.X && y >= distanceMinusTarget.Y) {
				goingToBeAtOrPast = true;
			}

			return goingToBeAtOrPast;
		}

		public static void handleChildMovement(float distance, ref Vector2 heading, ref Vector2 position, ref float rotation, ref List<TargetPosition> targetPositions) {

			Vector2 delta = PositionUtils.getDelta(heading, distance);
			Vector2 distanceMinusTarget, minVector, maxVector, positiveDelta;

			if (targetPositions != null && targetPositions.Count > 0 && goingToBeAtOrPastPoint(position, targetPositions[0].Position, ref delta, out distanceMinusTarget, out positiveDelta)) {
				TargetPosition targetPosition = targetPositions[0];
				Type type = targetPosition.GetType();
				if (type == typeof(WarpCoordinates)) {
					PositionUtils.getMinMax(positiveDelta, distanceMinusTarget, out minVector, out maxVector);
					Vector2 deltaMinusTargetDistance = Vector2.Subtract(maxVector, minVector);

					// we need to apply the remainder to the new direction
					position = ((WarpCoordinates)targetPosition).WarpTo;
					if (heading == Constants.HEADING_DOWN || heading == Constants.HEADING_UP) {
						delta = PositionUtils.getDelta(heading, deltaMinusTargetDistance.Y);
						position = new Vector2(position.X, position.Y + delta.Y);
					} else if (heading == Constants.HEADING_LEFT || heading == Constants.HEADING_RIGHT) {
						delta = PositionUtils.getDelta(heading, deltaMinusTargetDistance.X);
						position = new Vector2(position.X + delta.X, position.Y);
					}
				} else if (type == typeof(PivotPoint)) {
					PivotPoint pivotPoint = ((PivotPoint)targetPosition);
					// we are either at or going to pass through the pivot point so we need to set to pivot point + the extra distance
					PositionUtils.getMinMax(positiveDelta, distanceMinusTarget, out minVector, out maxVector);
					Vector2 deltaMinusPivotDistance = Vector2.Subtract(maxVector, minVector);

					// we need to apply the remainder to the new direction
					position = pivotPoint.Position;
					if (heading == Constants.HEADING_DOWN || heading == Constants.HEADING_UP) {
						delta = PositionUtils.getDelta(pivotPoint.Heading, deltaMinusPivotDistance.Y);
						position = new Vector2(position.X + delta.X, position.Y);
					} else if (heading == Constants.HEADING_LEFT || heading == Constants.HEADING_RIGHT) {
						delta = PositionUtils.getDelta(pivotPoint.Heading, deltaMinusPivotDistance.X);
						position = new Vector2(position.X, position.Y + delta.Y);
					}
					rotation += MathHelper.ToRadians(pivotPoint.Rotation);
					heading = pivotPoint.Heading;
				}
				targetPositions.RemoveAt(0);
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