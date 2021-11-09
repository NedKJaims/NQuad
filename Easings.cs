using System;

namespace NQuad {

    /*  https://easings.co/
    *   https://easings.net/
    *   How to use:
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
        // Linear Easing function
        public static float Linear(float t, float b, float c, float d) {
            return (c * t / d + b);
        }

        // Sine Easing functions
        public static float SineIn(float t, float b, float c, float d) {
            return (-c * (float)Math.Cos(t / d * ((float)Math.PI / 2)) + c + b);
        }

        public static float SineOut(float t, float b, float c, float d) {
            return (c * (float)Math.Sin(t / d * ((float)Math.PI / 2)) + b);
        }

        public static float SineInOut(float t, float b, float c, float d) {
            return (-c / 2 * ((float)Math.Cos((float)Math.PI * t / d) - 1) + b);
        }

        // Circular Easing functions
        public static float CircIn(float t, float b, float c, float d) {
            return (-c * ((float)Math.Sqrt(1 - (t /= d) * t) - 1) + b);
        }

        public static float CircOut(float t, float b, float c, float d) {
            return (c * (float)Math.Sqrt(1 - (t = t / d - 1) * t) + b);
        }

        public static float CircInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (-c / 2 * ((float)Math.Sqrt(1 - t * t) - 1) + b);
            }
            return (c / 2 * ((float)Math.Sqrt(1 - t * (t -= 2)) + 1) + b);
        }

        // Cubic Easing functions
        public static float CubicIn(float t, float b, float c, float d) {
            return (c * (t /= d) * t * t + b);
        }

        public static float CubicOut(float t, float b, float c, float d) {
            return (c * ((t = t / d - 1) * t * t + 1) + b);
        }

        public static float CubicInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (c / 2 * t * t * t + b);
            }
            return (c / 2 * ((t -= 2) * t * t + 2) + b);
        }

        // Quadratic Easing functions
        public static float QuadIn(float t, float b, float c, float d) {
            return (c * (t /= d) * t + b);
        }

        public static float QuadOut(float t, float b, float c, float d) {
            return (-c * (t /= d) * (t - 2) + b);
        }

        public static float QuadInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) {
                return (((c / 2) * (t * t)) + b);
            }
            return (-c / 2 * (((t - 2) * (--t)) - 1) + b);
        }

        // Quart Easing functions
        public static float QuartIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t * t + b;
        }

        public static float QuartOut(float t, float b, float c, float d) {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        public static float QuartInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t + b;
            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;

        }

        // Quint Easing functions
        public static float QuintIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t * t * t + b;
        }

        public static float QuintOut(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        public static float QuintInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1) return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;

        }
        
        // Exponential Easing functions
        public static float ExpoIn(float t, float b, float c, float d) {
            return (t == 0) ? b : (c * (float)Math.Pow(2, 10 * (t / d - 1)) + b);
        }

        public static float ExpoOut(float t, float b, float c, float d) {
            return (t == d) ? (b + c) : (c * (-(float)Math.Pow(2, -10 * t / d) + 1) + b);
        }

        public static float ExpoInOut(float t, float b, float c, float d) {
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
        public static float BackIn(float t, float b, float c, float d) {
            const float s = 1.70158f;
            float postFix = t /= d;
            return (c * (postFix) * t * ((s + 1) * t - s) + b);
        }

        public static float BackOut(float t, float b, float c, float d) {
            const float s = 1.70158f;
            return (c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b);
        }

        public static float BackInOut(float t, float b, float c, float d) {
            float s = 1.70158f;
            if ((t /= d / 2) < 1) {
                return (c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b);
            }

            float postFix = t -= 2;
            return (c / 2 * ((postFix) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b);
        }

        // Bounce Easing functions
        public static float BounceOut(float t, float b, float c, float d) {
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

        public static float BounceIn(float t, float b, float c, float d) {
            return (c - BounceOut(d - t, 0, c, d) + b);
        }

        public static float BounceInOut(float t, float b, float c, float d) {
            if (t < d / 2) {
                return (BounceIn(t * 2, 0, c, d) * 0.5f + b);
            } else {
                return (BounceOut(t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b);
            }
        }

        // Elastic Easing functions
        public static float ElasticIn(float t, float b, float c, float d) {
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

        public static float ElasticOut(float t, float b, float c, float d) {
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

        public static float ElasticInOut(float t, float b, float c, float d) {
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
