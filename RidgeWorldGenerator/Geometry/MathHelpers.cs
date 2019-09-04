using System;

namespace RidgeWorldGenerator.Geometry
{
    static class MathHelpers{

        // based on https://www.particleincell.com/2013/cubic-line-intersection/
        // based on http://mysite.verizon.net/res148h4j/javascript/script_exact_cubic.html#the%20source%20code*/
        public static double[] cubicRoots(double a, double b, double c, double d){
            double A = b/a;
            double B = c/a;
            double C = d/a;

            double Q = (3*B - Math.pow(A, 2))/9;
            double R = (9*A*B - 27*C - 2*Math.pow(A, 3))/54;
            double D = Math.pow(Q, 3) + Math.pow(R, 2);    // polynomial discriminant

            double[] t = new double[3];

            if (D >= 0)                                 // complex or duplicate roots
            {
                double S = sgn(R + Math.sqrt(D))*Math.pow(Math.abs(R + Math.sqrt(D)),(1/3));
                double T = sgn(R - Math.sqrt(D))*Math.pow(Math.abs(R - Math.sqrt(D)),(1/3));
        
                t[0] = -A/3 + (S + T);                    // real root
                t[1] = -A/3 - (S + T)/2;                  // real part of complex root
                t[2] = -A/3 - (S + T)/2;                  // real part of complex root
                Im = Math.abs(Math.sqrt(3)*(S - T)/2);    // complex part of root pair   
        
                /*discard complex roots*/
                if (Im!=0)
                {
                    t[1]=-1;
                    t[2]=-1;
                }
        
            }
            else                                          // distinct real roots
            {
                double th = Math.acos(R/Math.sqrt(-Math.pow(Q, 3)));
        
                t[0] = 2*Math.sqrt(-Q)*Math.cos(th/3) - A/3;
                t[1] = 2*Math.sqrt(-Q)*Math.cos((th + 2*Math.PI)/3) - A/3;
                t[2] = 2*Math.sqrt(-Q)*Math.cos((th + 4*Math.PI)/3) - A/3;
                Im = 0.0;
            }
        }
    }
}