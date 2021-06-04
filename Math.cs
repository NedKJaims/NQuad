using Microsoft.Xna.Framework;
using System;

namespace NQuad
{
    public static class Collision
    {

        public static bool CheckCollisionRecs(in Rectangle rec1, in Rectangle rec2) {

            return (rec1.X < (rec2.X + rec2.Width) && (rec1.X + rec1.Width) > rec2.X) &&
                (rec1.Y < (rec2.Y + rec2.Height) && (rec1.Y + rec1.Height) > rec2.Y);
        }
        public static bool CheckCollisionCircles(in Vector2 center1, in float radius1, in Vector2 center2, in float radius2) {
            float dx = center2.X - center1.X;      // X distance between centers
            float dy = center2.Y - center1.Y;      // Y distance between centers

            float distance = (float)Math.Sqrt(dx * dx + dy * dy); // Distance between centers

            return (distance <= (radius1 + radius2));
        }

        public static bool CheckCollisionCircleRec(in Vector2 center, in float radius, in Rectangle rec) {
            int recCenterX = (int)(rec.X + rec.Width / 2.0f);
            int recCenterY = (int)(rec.Y + rec.Height / 2.0f);

            float dx = Math.Abs(center.X - recCenterX);
            float dy = Math.Abs(center.Y - recCenterY);

            if (dx > (rec.Width / 2.0f + radius)) { return false; }
            if (dy > (rec.Height / 2.0f + radius)) { return false; }

            if (dx <= (rec.Width / 2.0f)) { return true; }
            if (dy <= (rec.Height / 2.0f)) { return true; }

            float cornerDistanceSq = (dx - rec.Width / 2.0f) * (dx - rec.Width / 2.0f) +
                                     (dy - rec.Height / 2.0f) * (dy - rec.Height / 2.0f);

            return (cornerDistanceSq <= (radius * radius));
        }
        public static Rectangle GetCollisionRec(in Rectangle rec1, in Rectangle rec2) {
            Rectangle retRec = Rectangle.Empty;

            if (CheckCollisionRecs(rec1, rec2)) {
                int dxx = (int)Math.Abs(rec1.Width - rec2.X);
                int dyy = (int)Math.Abs(rec1.Y - rec2.Y);

                if (rec1.X <= rec2.X) {
                    if (rec1.Y <= rec2.Y) {
                        retRec.X = rec2.X;
                        retRec.Y = rec2.Y;
                        retRec.Width = rec1.Width - dxx;
                        retRec.Height = rec1.Height - dyy;
                    } else {
                        retRec.X = rec2.X;
                        retRec.Y = rec1.Y;
                        retRec.Width = rec1.Width - dxx;
                        retRec.Height = rec2.Height - dyy;
                    }
                } else {
                    if (rec1.Y <= rec2.Y) {
                        retRec.X = rec1.X;
                        retRec.Y = rec2.Y;
                        retRec.Width = rec2.Width - dxx;
                        retRec.Height = rec1.Height - dyy;
                    } else {
                        retRec.X = rec1.X;
                        retRec.Y = rec1.Y;
                        retRec.Width = rec2.Width - dxx;
                        retRec.Height = rec2.Height - dyy;
                    }
                }

                if (rec1.Width > rec2.Width) {
                    if (retRec.Width >= rec2.Width) retRec.Width = rec2.Width;
                } else {
                    if (retRec.Width >= rec1.Width) retRec.Width = rec1.Width;
                }

                if (rec1.Height > rec2.Height) {
                    if (retRec.Height >= rec2.Height) retRec.Height = rec2.Height;
                } else {
                    if (retRec.Height >= rec1.Height) retRec.Height = rec1.Height;
                }
            }

            return retRec;
        }

        public static bool CheckCollisionPointRec(in Vector2 point, in Rectangle rec) {

            return (point.X >= rec.X) && (point.X <= (rec.X + rec.Width)) && (point.Y >= rec.Y) && (point.Y <= (rec.Y + rec.Height));
        }
        public static bool CheckCollisionPointCircle(in Vector2 point, in Vector2 center, in float radius) {
            return CheckCollisionCircles(point, 0, center, radius);
        }
        public static bool CheckCollisionPointTriangle(in Vector2 point, in Vector2 p1, in Vector2 p2, in Vector2 p3) {
            float alpha = ((p2.Y - p3.Y) * (point.X - p3.X) + (p3.X - p2.X) * (point.Y - p3.Y)) /
                          ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float beta = ((p3.Y - p1.Y) * (point.X - p3.X) + (p1.X - p3.X) * (point.Y - p3.Y)) /
                         ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float gamma = 1.0f - alpha - beta;

            if ((alpha > 0) && (beta > 0) & (gamma > 0)) return true;

            return false;
        }

        public static bool CheckCollisionLines(in Vector2 startPos1, in Vector2 endPos1, in Vector2 startPos2, in Vector2 endPos2, ref Vector2 collisionPoint) {
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
        public static bool CheckCollisionPointLine(in Vector2 point, in Vector2 p1, in Vector2 p2, in int threshold) {
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
    *   t = current time (in any unit measure, but same unit as duration)
    *   b = starting value to interpolate
    *   c = the total change in value of b that needs to occur
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
    public static class Easings
    {

        public static float EaseLinearNone(in float t, in float b, in float c, in float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearIn(in float t, in float b, in float c, in float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearOut(in float t, in float b, in float c, in float d) {
            return (c * t / d + b);
        }

        public static float EaseLinearInOut(in float t, in float b, in float c, in float d) {
            return (c * t / d + b);
        }

        // Sine Easing functions
        public static float EaseSineIn(in float t, in float b, in float c, in float d) {
            return (-c * (float)Math.Cos(t / d * ((float)Math.PI / 2)) + c + b);
        }

        public static float EaseSineOut(in float t, in float b, in float c, in float d) {
            return (c * (float)Math.Sin(t / d * ((float)Math.PI / 2)) + b);
        }

        public static float EaseSineInOut(in float t, in float b, in float c, in float d) {
            return (-c / 2 * ((float)Math.Cos((float)Math.PI * t / d) - 1) + b);
        }

        // Circular Easing functions
        public static float EaseCircIn(float t, in float b, in float c, in float d) {
            return (-c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b);
        }

        public static float EaseCircOut(float t, in float b, in float c, in float d) {
            return (c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b);
        }

        public static float EaseCircInOut(float t, in float b, in float c, in float d) {
            if ((t /= d / 2) < 1) {
                return (-c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b);
            }
            return (c / 2 * ((float)Math.Sqrt(1 - t * (t -= 2)) + 1) + b);
        }

        // Cubic Easing functions
        public static float EaseCubicIn(float t, in float b, in float c, in float d) {
            return (c * (t /= d) * t * t + b);
        }

        public static float EaseCubicOut(float t, in float b, in float c, in float d) {
            return (c * ((t = t / d - 1) * t * t + 1) + b);
        }

        public static float EaseCubicInOut(float t, in float b, in float c, in float d) {
            if ((t /= d / 2) < 1) {
                return (c / 2 * t * t * t + b);
            }
            return (c / 2 * ((t -= 2) * t * t + 2) + b);
        }

        // Quadratic Easing functions
        public static float EaseQuadIn(float t, in float b, in float c, in float d) {
            return (c * (t /= d) * t + b);
        }

        public static float EaseQuadOut(float t, in float b, in float c, in float d) {
            return (-c * (t /= d) * (t - 2) + b);
        }

        public static float EaseQuadInOut(float t, in float b, in float c, in float d) {
            if ((t /= d / 2) < 1) {
                return (((c / 2) * (t * t)) + b);
            }
            return (-c / 2 * (((t - 2) * (--t)) - 1) + b);
        }

        // Exponential Easing functions
        public static float EaseExpoIn(in float t, in float b, in float c, in float d) {
            return (t == 0) ? b : (c * (float)Math.Pow(2, 10 * (t / d - 1)) + b);
        }

        public static float EaseExpoOut(in float t, in float b, in float c, in float d) {
            return (t == d) ? (b + c) : (c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b);
        }

        public static float EaseExpoInOut(float t, in float b, in float c, in float d) {
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
        public static float EaseBackIn(float t, in float b, in float c, in float d) {
            float s = 1.70158f;
            float postFix = t /= d;
            return (c * (postFix) * t * ((s + 1) * t - s) + b);
        }

        public static float EaseBackOut(float t, in float b, in float c, in float d) {
            float s = 1.70158f;
            return (c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b);
        }

        public static float EaseBackInOut(float t, in float b, in float c, in float d) {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) {
                return (c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b);
            }

            float postFix = t -= 2;
            return (c / 2 * ((postFix) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b);
        }

        // Bounce Easing functions
        public static float EaseBounceOut(float t, in float b, in float c, in float d) {
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

        public static float EaseBounceIn(in float t, in float b, in float c, in float d) {
            return (c - EaseBounceOut(d - t, 0, c, d) + b);
        }

        public static float EaseBounceInOut(in float t, in float b, in float c, in float d) {
            if (t < d / 2) {
                return (EaseBounceIn(t * 2, 0, c, d) * 0.5f + b);
            } else {
                return (EaseBounceOut(t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b);
            }
        }

        // Elastic Easing functions
        public static float EaseElasticIn(float t, in float b, in float c, in float d) {
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

        public static float EaseElasticOut(float t, in float b, in float c, in float d) {
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

        public static float EaseElasticInOut(float t, in float b, in float c, in float d) {
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
