using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeWorldGenerator
{
    static class FileOpener
    {
        public static List<Curve> GetCurves()
        {
            const string Path = @"../../../curve.txt";
            string[] lines = File.ReadAllLines(Path);
            List<Curve> curvesList = new List<Curve>();
            for (int j = 0; j < lines.Length; j++)
            {
                string line = lines[j];
                string[] points = line.Split(' ');
                List<Vector2> pointlist = new List<Vector2>();
                bool relative = points[2] == "c";
                for (int i = 0; i < points.Length; i++)
                {
                    if (i == 0 || i == 2)
                        continue;
                    string[] coords = points[i].Split(',');
                    double x = double.Parse(coords[0]);
                    double y = double.Parse(coords[1]);
                    pointlist.Add(new Vector2(x, y));
                }
                curvesList.Add(new Curve(pointlist, relative));
            }
            return curvesList;
        }
    }

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
                Vector2 c1 = relative? p1:Vector2.ZERO + pointlist[1 + i * 3];
                Vector2 c2 = relative ? p1 : Vector2.ZERO + pointlist[2 + i * 3];
                Vector2 p2 = relative ? p1 : Vector2.ZERO + pointlist[3 + i * 3];
                segments.Add(new Segment() { p1 = p1.ToPoint(scale), c1 = c1.ToPoint(scale), p2 = p2.ToPoint(scale), c2 = c2.ToPoint(scale) });
                p1 = p2;
            }
        }

        public void Paint(System.Windows.Forms.PaintEventArgs e, Pen pen)
        {
            for (int i = 0; i < segments.Count; i++)
            {
                e.Graphics.DrawBezier(pen, segments[i].p1, segments[i].c1, segments[i].c2, segments[i].p2);
            }
        }

        struct Segment
        {
            public Point p1;
            public Point c1;
            public Point p2;
            public Point c2;
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

        public static Vector2 ZERO => new Vector2(0, 0);

        public Point ToPoint(double scale = 1) => new Point((int)(x * scale), (int)(y * scale));
    }
}
