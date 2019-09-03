using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeWorldGenerator.Geometry
{


    class Curve
    {
        List<Segment> segments;

        public Curve(List<Vector2> pointlist, bool relative)
        {
            if (pointlist.Count % 3 != 1)
                throw new ArgumentException("pointlist invalid length", "pointlist");
            segments = new List<Segment>();

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

        public double lenght => segments.Sum(s => s.Length);

        public void Paint(DrawParams drawParams, Pen pen, System.Windows.Forms.PaintEventArgs e)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                e.Graphics.DrawBezier(pen, segments[i].p1.ToPoint(drawParams), segments[i].c1.ToPoint(drawParams), segments[i].c2.ToPoint(drawParams), segments[i].p2.ToPoint(drawParams));
                for (double t = 0; t < 1; t+= 0.2)
                {
                    e.Graphics.DrawEllipse(pen, new RectangleF(segments[i].GetPointAt(t).ToPoint(drawParams), new SizeF(4, 4)));
                }
            }
        }

        struct Segment
        {
            public Vector2 p1;
            public Vector2 c1;
            public Vector2 p2;
            public Vector2 c2;

            public Vector2 GetPointAt(double t)
            {
                if (t < 0 || t > 1)
                    throw new ArgumentOutOfRangeException("t must be between 0 and 1. t is: " + t);
                double x = ((1 - t) * (1 - t) * (1 - t)) * p1.x
                           + 3 * ((1 - t) * (1 - t)) * t * c1.x
                                 + 3 * (1 - t) * (t * t) * c2.x
                                           + (t * t * t) * p2.x;
                double y = ((1 - t) * (1 - t) * (1 - t)) * p1.y
                           + 3 * ((1 - t) * (1 - t)) * t * c1.y
                                 + 3 * (1 - t) * (t * t) * c2.y
                                           + (t * t * t) * p2.y;
                return new Vector2(x, y);
            }

            public double Length { get
                {
                    double sum = 0;
                    for(double t = 0; t < 1; t+= 0.1)
                    {
                        sum += (GetPointAt(t + .1) - GetPointAt(t)).Length;
                    }
                    return sum;
                } }
        }
    }
}
