using Microsoft.Xna.Framework;
using System;

namespace NQuad {

    public static class Collision {

        public static bool Recs(float rec1X, float rec1Y, float rec1Width, float rec1Height, float rec2X, float rec2Y, float rec2Width, float rec2Height) {
            return (rec1X < (rec2X + rec2Width) && (rec1X + rec1Width) > rec2X) &&
                (rec1Y < (rec2Y + rec2Height) && (rec1Y + rec1Height) > rec2Y);
        }
        public static bool Circles(Vector2 center1, float radius1, Vector2 center2, float radius2) {
            float dx = center2.X - center1.X;      // X distance between centers
            float dy = center2.Y - center1.Y;      // Y distance between centers

            float distance = (float)Math.Sqrt(dx * dx + dy * dy); // Distance between centers

            return (distance <= (radius1 + radius2));
        }

        public static bool CircleRec(Vector2 center, float radius, float recX, float recY, float recWidth, float recHeight) {
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

        public static bool PointRec(Vector2 point, float recX, float recY, float recWidth, float recHeight) {
            return (point.X >= recX) && (point.X <= (recX + recWidth)) && (point.Y >= recY) && (point.Y <= (recY + recHeight));
        }
        public static bool PointCircle(Vector2 point, Vector2 center, float radius) {
            return Circles(point, 0, center, radius);
        }
        public static bool PointTriangle(Vector2 point, Vector2 p1, Vector2 p2, Vector2 p3) {
            float alpha = ((p2.Y - p3.Y) * (point.X - p3.X) + (p3.X - p2.X) * (point.Y - p3.Y)) /
                          ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float beta = ((p3.Y - p1.Y) * (point.X - p3.X) + (p1.X - p3.X) * (point.Y - p3.Y)) /
                         ((p2.Y - p3.Y) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Y - p3.Y));

            float gamma = 1.0f - alpha - beta;

            if ((alpha > 0) && (beta > 0) & (gamma > 0)) return true;

            return false;
        }

        public static bool Lines(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2, ref Vector2 collisionPoint) {
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
        public static bool PointLine(Vector2 point, Vector2 p1, Vector2 p2, int threshold) {
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
}
