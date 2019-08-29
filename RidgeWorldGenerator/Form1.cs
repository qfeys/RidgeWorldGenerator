using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RidgeWorldGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            drawParams = new DrawParams() { offset = Vector2.ZERO, scale = 1.0f };
        }

        List<Curve> curves;
        DrawParams drawParams;

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (curves == null)
                curves = FileOpener.GetCurves();

            Point p1 = new Point(10, 100);   // Start point
            Point c1 = new Point(100, 10);   // First control point
            Point c2 = new Point(150, 150);  // Second control point
            Point p2 = new Point(200, 100);  // Endpoint

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 255));
            curves.ForEach(c=>c.Paint(drawParams, pen, e));
        }
    }

    struct DrawParams
    {
        public Vector2 offset;
        public float scale;
    }
}
