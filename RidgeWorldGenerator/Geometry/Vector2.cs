using System;
using System.Drawing;

namespace RidgeWorldGenerator
{
    struct Vector2
    {
        public readonly double x;
        public readonly double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.x + v2.x, v1.y + v2.y);
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.x - v2.x, v1.y - v2.y);
        public static Vector2 operator *(Vector2 v1, double d) => new Vector2(v1.x * d, v1.y * d);

        public static Vector2 ZERO => new Vector2(0, 0);

        public Point ToPoint(Vector2 offset, double scale = 1) => new Point((int)((x - offset.x) * scale), (int)((y - offset.y) * scale));
        public Point ToPoint(DrawParams dp) => ToPoint(dp.offset, dp.scale);

        public double LengthSq => x * x + y * y;
        public double Length => Math.Sqrt(LengthSq);
        public double Angle => Math.Atan2(y, x);

        public static Vector2 FromAngle(double angle) => new Vector2(Math.Sin(angle), Math.Cos(angle));
    }
}
