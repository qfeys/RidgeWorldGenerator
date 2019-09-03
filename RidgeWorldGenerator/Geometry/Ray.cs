using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RidgeWorldGenerator.Geometry
{
    class Ray
    {
        readonly Vector2 origin;
        readonly double direction; // Angel in rad, orientation such that positive x-axis = 0 and positive y-axis = +Pi/2

        public Ray(Vector2 origin, double direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public void Paint(DrawParams drawParams, Pen pen, System.Windows.Forms.PaintEventArgs e)
        {
            Vector2 end = Vector2.FromAngle(direction) * 40 * drawParams.scale + origin;
            e.Graphics.DrawLine(pen, origin.ToPoint(drawParams), end.ToPoint(drawParams));
        }
    }
}
