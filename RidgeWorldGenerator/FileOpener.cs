using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RidgeWorldGenerator.Geometry;

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
}
