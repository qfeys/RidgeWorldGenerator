using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeWorldGenerator
{


    class Curve
    {
        List<Segment> segments;

        public Curve(List<Vector2> pointlist, bool relative)
        {
            if (pointlist.Count % 3 != 1)
                throw new ArgumentException("pointlist invalid length", "pointlist");
            segments = new List<Segment>();
            float scale = .5f;

            Vector2 p1 = pointlist[0];

            for (int i = 0; i < pointlist.Count / 3; i++)
            {
                Vector2 c1 = relative ? p1 : Vector2.ZERO + pointlist[1 + i * 3];
                Vector2 c2 = relative ? p1 : Vector2.ZERO + pointlist[2 + i * 3];
                Vector2 p2 = relative ? p1 : Vector2.ZERO + pointlist[3 + i * 3];
                segments.Add(new Segment() { p1 = p1, c1 = c1, p2 = p2, c2 = c2 });
                p1 = p2;
            }
        }

        public void Paint(DrawParams drawParams, Pen pen, System.Windows.Forms.PaintEventArgs e)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                e.Graphics.DrawBezier(pen, segments[i].p1.ToPoint(drawParams), segments[i].c1.ToPoint(drawParams), segments[i].c2.ToPoint(drawParams), segments[i].p2.ToPoint(drawParams));
            }
        }

        struct Segment
        {
            public Vector2 p1;
            public Vector2 c1;
            public Vector2 p2;
            public Vector2 c2;
        }
    }

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

        public static Vector2 ZERO => new Vector2(0, 0);

        public Point ToPoint(Vector2 offset, double scale = 1) => new Point((int)((x - offset.x) * scale), (int)((y - offset.y) * scale));
        public Point ToPoint(DrawParams dp) => ToPoint(dp.offset, dp.scale);
    }
}
