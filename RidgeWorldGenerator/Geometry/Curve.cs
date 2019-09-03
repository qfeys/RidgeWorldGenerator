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

        public double Lenght => segments.Sum(s => s.Length);
        private List<Bez_Point> GetPointsOfDelta(double delta)
        {
            // Grabs segments; Get the points of each of them; move into single list; remove duplicates;
            return segments.ConvertAll(seg => seg.GetPointsOfDelta(delta)).SelectMany(x => x).Distinct().ToList();
        }



        public void Paint(DrawParams drawParams, Pen pen, System.Windows.Forms.PaintEventArgs e)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                e.Graphics.DrawBezier(pen, segments[i].p1.ToPoint(drawParams), segments[i].c1.ToPoint(drawParams), segments[i].c2.ToPoint(drawParams), segments[i].p2.ToPoint(drawParams));
            }
            List<Vector2> points = GetPointsOfDelta(20).ConvertAll(p=>(Vector2)p);
            points.ForEach(p => e.Graphics.DrawEllipse(pen, new RectangleF(p.ToPoint(drawParams), new SizeF(4, 4))));
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

            public double Length
            {
                get
                {
                    double delta = 0.1;
                    double sum = 0;
                    for (double t = 0; t <= 1 - delta; t += delta)
                    {
                        sum += (GetPointAt(t + delta) - GetPointAt(t)).Length;
                    }
                    return sum;
                }
            }

            public List<Bez_Point> GetPointsOfDelta(double delta)
            {
                List<Bez_Point> vectors = new List<Bez_Point>();
                int segments = (int)(Length / delta);
                if (segments == 0) segments = 1;
                double dt = 1.0 / segments;
                for (double t = 0; t < 1; t += dt)
                    vectors.Add(new Bez_Point(this, t));
                return vectors;
            }
        }

        struct Bez_Point
        {
            readonly Segment segment;
            readonly double t;

            public Bez_Point(Segment segment, double t)
            {
                this.segment = segment;
                this.t = t;
            }

            public static implicit operator Vector2(Bez_Point b) => b.segment.GetPointAt(b.t);
        }
    }
}
