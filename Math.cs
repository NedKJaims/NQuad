using Microsoft.Xna.Framework;
using System;

namespace NQuad {
    public static class Collision {

        public static bool CheckCollisionRecs(float rec1X, float rec1Y, float rec1Width, float rec1Height, float rec2X, float rec2Y, float rec2Width, float rec2Height) {
            return (rec1X < (rec2X + rec2Width) && (rec1X + rec1Width) > rec2X) &&
                (rec1Y < (rec2Y + rec2Height) && (rec1Y + rec1Height) > rec2Y);
        }
        public static bool CheckCollisionCircles(Vector2 center1, float radius1, Vector2 center2, float radius2) {
            float dx = center2.X - center1.X;      // X distance between centers
            float dy = center2.Y - center1.Y;      // Y distance between centers

            float distance = (float)Math.Sqrt(dx * dx + dy * dy); // Distance between centers

            return (distance <= (radius1 + radius2));
        }

        public static bool CheckCollisionCircleRec(Vector2 center, float radius, float recX, float recY, float recWidth, float recHeight) {
            int recCenterX = (int)(recX + recWidth / 2.0f);
            int recCenterY = (int)(recY + recHeight / 2.0f);

            float dx = Math.Abs(center.X - recCenterX);
            float dy = Math.Abs(center.Y - recCenterY);

            if (dx > (recWidth / 2.0f + radius)) { return false; }
            if (dy > (recHeight / 2.0f + radius)) { return false; }

            if (dx <= (recWidth / 2.0f)) { return true; }
            if (dy <= (recHeight / 2.0f)) { return true; }

            float cornerDistanceSq = (dx - recWidth / 2.0f) * (dx - recWidth / 2.0f) +
                                     (dy - recHeight / 2.0f) * (dy - recHeight / 2.0f);

            return (cornerDistanceSq <= (radius * radius));
        }

        public static bool CheckCollisionPointRec(Vector2 point, float recX, float recY, float recWidth, float recHeight) {

            return (point.X >= recX) && (point.X <= (recX + recWidth)) && (point.Y >= recY) && (point.Y <= (recY + recHeight));
        }
        public static bool CheckCollisionPointCircle(Vector2 point, Vector2 center, float radius) {
            return CheckCollisionCircles(point, 0, center, radius);
        }
        public static bool CheckCollisionPointTriangle(Vector2 point, Vector2 p1, Vector2 p2, Vector2 p3) {
            float alpha = ((p2.Y - p3.Y) * (point.X - p3.X) + (p3.X - p2.X) * (point.Y - p3.Y)) /
                          ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float beta = ((p3.Y - p1.Y) * (point.X - p3.X) + (p1.X - p3.X) * (point.Y - p3.Y)) /
                         ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float gamma = 1.0f - alpha - beta;

            if ((alpha > 0) && (beta > 0) & (gamma > 0)) return true;

            return false;
        }

        public static bool CheckCollisionLines(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2, ref Vector2 collisionPoint) {
            float div = (endPos2.Y - startPos2.Y) * (endPos1.X - startPos1.X) - (endPos2.X - startPos2.X) * (endPos1.Y - startPos1.Y);

            if (div == 0.0f) return false;      // WARNING: This check could not work due to float precision rounding issues...

            float xi = ((startPos2.X - endPos2.X) * (startPos1.X * endPos1.Y - startPos1.Y * endPos1.X) - (startPos1.X - endPos1.X) * (startPos2.X * endPos2.Y - startPos2.Y * endPos2.X)) / div;
            float yi = ((startPos2.Y - endPos2.Y) * (startPos1.X * endPos1.Y - startPos1.Y * endPos1.X) - (startPos1.Y - endPos1.Y) * (startPos2.X * endPos2.Y - startPos2.Y * endPos2.X)) / div;

            if (xi < Math.Min(startPos1.X, endPos1.X) || xi > Math.Max(startPos1.X, endPos1.X)) return false;
            if (xi < Math.Min(startPos2.X, endPos2.X) || xi > Math.Max(startPos2.X, endPos2.X)) return false;
            if (yi < Math.Min(startPos1.Y, endPos1.Y) || yi > Math.Max(startPos1.Y, endPos1.Y)) return false;
            if (yi < Math.Min(startPos2.Y, endPos2.Y) || yi > Math.Max(startPos2.Y, endPos2.Y)) return false;

            collisionPoint.X = xi;
            collisionPoint.Y = yi;

            return true;
        }
        public static bool CheckCollisionPointLine(Vector2 point, Vector2 p1, Vector2 p2, int threshold) {
            bool collision = false;
            float dxc = point.X - p1.X;
            float dyc = point.Y - p1.Y;
            float dxl = p2.X - p1.X;
            float dyl = p2.Y - p1.Y;
            float cross = dxc * dyl - dyc * dxl;

            if (Math.Abs(cross) < (threshold * Math.Max(Math.Abs(dxl), Math.Abs(dyl)))) {
                if (Math.Abs(dxl) >= Math.Abs(dyl)) collision = (dxl > 0) ? ((p1.X <= point.X) && (point.X <= p2.X)) : ((p2.X <= point.X) && (point.X <= p1.X));
                else collision = (dyl > 0) ? ((p1.Y <= point.Y) && (point.Y <= p2.Y)) : ((p2.Y <= point.Y) && (point.Y <= p1.Y));
            }

            return collision;
        }

    }


    /*   How to use:
    *   The four inputs t,b,c,d are defined as follows:
    *   t = current time ( any unit measure, but same unit as duration)
    *   b = starting value to interpolate
    *   c = the total change  value of b that needs to occur
    *   d = total time it should take to complete (duration)
    *
    *   Example:
    *
    *   int currentTime = 0;
    *   int duration = 100;
    *   float startPositionX = 0.0f;
    *   float finalPositionX = 30.0f;
    *   float currentPositionX = startPositionX;
    *
    *   while (currentPositionX < finalPositionX)
    *   {
    *       currentPositionX = EaseSineIn(currentTime, startPositionX, finalPositionX - startPositionX, duration);
    *       currentTime++;
    *   }
    */
    public static class Easings {

        public static float EaseLinearNone(float t, float b, float c, float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearIn(float t, float b, float c, float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearOut(float t, float b, float c, float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearInOut(float t, float b, float c, float d) {
            return (c * t / d + b);
        }

        // Sine Easing functions
        public static float EaseSineIn(float t, float b, float c, float d) {
            return (-c * (float)Math.Cos(t / d * ((float)Math.PI / 2)) + c + b);
        }

        public static float EaseSineOut(float t, float b, float c, float d) {
            return (c * (float)Math.Sin(t / d * ((float)Math.PI / 2)) + b);
        }

        public static float EaseSineInOut(float t, float b, float c, float d) {
            return (-c / 2 * ((float)Math.Cos((float)Math.PI * t / d) - 1) + b);
        }

        // Circular Easing functions
        public static float EaseCircIn(float t, float b, float c, float d) {
            return (-c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b);
        }

        public static float EaseCircOut(float t, float b, float c, float d) {
            return (c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b);
        }

        public static float EaseCircInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (-c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b);
            }
            return (c / 2 * ((float)Math.Sqrt(1 - t * (t -= 2)) + 1) + b);
        }

        // Cubic Easing functions
        public static float EaseCubicIn(float t, float b, float c, float d) {
            return (c * (t /= d) * t * t + b);
        }

        public static float EaseCubicOut(float t, float b, float c, float d) {
            return (c * ((t = t / d - 1) * t * t + 1) + b);
        }

        public static float EaseCubicInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (c / 2 * t * t * t + b);
            }
            return (c / 2 * ((t -= 2) * t * t + 2) + b);
        }

        // Quadratic Easing functions
        public static float EaseQuadIn(float t, float b, float c, float d) {
            return (c * (t /= d) * t + b);
        }

        public static float EaseQuadOut(float t, float b, float c, float d) {
            return (-c * (t /= d) * (t - 2) + b);
        }

        public static float EaseQuadInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (((c / 2) * (t * t)) + b);
            }
            return (-c / 2 * (((t - 2) * (--t)) - 1) + b);
        }

        // Exponential Easing functions
        public static float EaseExpoIn(float t, float b, float c, float d) {
            return (t == 0) ? b : (c * (float)Math.Pow(2, 10 * (t / d - 1)) + b);
        }

        public static float EaseExpoOut(float t, float b, float c, float d) {
            return (t == d) ? (b + c) : (c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b);
        }

        public static float EaseExpoInOut(float t, float b, float c, float d) {
            if (t == 0) {
                return b;
            }
            if (t == d) {
                return (b + c);
            }
            if ((t /= d / 2) < 1) {
                return (c / 2 * (float)Math.Pow(2, 10 * (t - 1)) + b);
            }
            return (c / 2 * (-(float)Math.Pow(2, -10 * --t) + 2) + b);
        }

        // Back Easing functions
        public static float EaseBackIn(float t, float b, float c, float d) {
            float s = 1.70158f;
            float postFix = t /= d;
            return (c * (postFix) * t * ((s + 1) * t - s) + b);
        }

        public static float EaseBackOut(float t, float b, float c, float d) {
            float s = 1.70158f;
            return (c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b);
        }

        public static float EaseBackInOut(float t, float b, float c, float d) {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) {
                return (c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b);
            }

            float postFix = t -= 2;
            return (c / 2 * ((postFix) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b);
        }

        // Bounce Easing functions
        public static float EaseBounceOut(float t, float b, float c, float d) {
            if ((t /= d) < (1 / 2.75f)) {
                return (c * (7.5625f * t * t) + b);
            } else if (t < (2 / 2.75f)) {
                float postFix = t -= (1.5f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.75f) + b);
            } else if (t < (2.5 / 2.75)) {
                float postFix = t -= (2.25f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.9375f) + b);
            } else {
                float postFix = t -= (2.625f / 2.75f);
                return (c * (7.5625f * (postFix) * t + 0.984375f) + b);
            }
        }

        public static float EaseBounceIn(float t, float b, float c, float d) {
            return (c - EaseBounceOut(d - t, 0, c, d) + b);
        }

        public static float EaseBounceInOut(float t, float b, float c, float d) {
            if (t < d / 2) {
                return (EaseBounceIn(t * 2, 0, c, d) * 0.5f + b);
            } else {
                return (EaseBounceOut(t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b);
            }
        }

        // Elastic Easing functions
        public static float EaseElasticIn(float t, float b, float c, float d) {
            if (t == 0) {
                return b;
            }
            if ((t /= d) == 1) {
                return (b + c);
            }

            float p = d * 0.3f;
            float a = c;
            float s = p / 4;
            float postFix = a * (float)Math.Pow(2, 10 * (t -= 1));

            return (-(postFix * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b);
        }

        public static float EaseElasticOut(float t, float b, float c, float d) {
            if (t == 0) {
                return b;
            }
            if ((t /= d) == 1) {
                return (b + c);
            }

            float p = d * 0.3f;
            float a = c;
            float s = p / 4;

            return (a * (float)Math.Pow(2, -10 * t) * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) + c + b);
        }

        public static float EaseElasticInOut(float t, float b, float c, float d) {
            if (t == 0) {
                return b;
            }
            if ((t /= d / 2) == 2) {
                return (b + c);
            }

            float p = d * (0.3f * 1.5f);
            float a = c;
            float s = p / 4;

            float postFix = 0f;
            if (t < 1) {
                postFix = a * (float)Math.Pow(2, 10 * (t -= 1));
                return -0.5f * (postFix * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p)) + b;
            }

            postFix = a * (float)Math.Pow(2, -10 * (t -= 1));

            return (postFix * (float)Math.Sin((t * d - s) * (2 * (float)Math.PI) / p) * 0.5f + c + b);
        }


    }
}
