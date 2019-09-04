using System;

namespace RidgeWorldGenerator.Geometry
{
    static class MathHelpers{

        // based on https://www.particleincell.com/2013/cubic-line-intersection/
        // based on http://mysite.verizon.net/res148h4j/javascript/script_exact_cubic.html#the%20source%20code*/
        public static double[] CubicRoots(double a, double b, double c, double d){
            double A = b/a;
            double B = c/a;
            double C = d/a;

            double Q = (3*B - Math.Pow(A, 2))/9;
            double R = (9*A*B - 27*C - 2*Math.Pow(A, 3))/54;
            double D = Math.Pow(Q, 3) + Math.Pow(R, 2);    // polynomial discriminant

            double[] t = new double[3];

            if (D >= 0)                                 // complex or duplicate roots
            {
                double S = Math.Sign(R + Math.Sqrt(D))*Math.Pow(Math.Abs(R + Math.Sqrt(D)),(1/3));
                double T = Math.Sign(R - Math.Sqrt(D))*Math.Pow(Math.Abs(R - Math.Sqrt(D)),(1/3));
        
                t[0] = -A/3 + (S + T);                    // real root
                t[1] = -A/3 - (S + T)/2;                  // real part of complex root
                t[2] = -A/3 - (S + T)/2;                  // real part of complex root
                double Im = Math.Abs(Math.Sqrt(3)*(S - T)/2);    // complex part of root pair   
        
                /*discard complex roots*/
                if (Im!=0)
                {
                    t[1]=-1;
                    t[2]=-1;
                }
        
            }
            else                                          // distinct real roots
            {
                double th = Math.Acos(R/Math.Sqrt(-Math.Pow(Q, 3)));
        
                t[0] = 2*Math.Sqrt(-Q)*Math.Cos(th/3) - A/3;
                t[1] = 2*Math.Sqrt(-Q)*Math.Cos((th + 2*Math.PI)/3) - A/3;
                t[2] = 2*Math.Sqrt(-Q)*Math.Cos((th + 4*Math.PI)/3) - A/3;
                double Im = 0.0;
            }

            return t;
        }
    }
}