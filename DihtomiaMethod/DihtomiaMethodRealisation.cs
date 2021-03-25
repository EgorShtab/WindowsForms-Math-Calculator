using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.mariuszgromada.math.mxparser;
using System.IO;

namespace DihtomiaMethod
{
    public class DihtomiaMethodRealisation
    {
        string points = "";
        Function _f;
        static int i=0;
        public void CountExpression(double E, double a, double b) {
            long startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            double fa, fb, fx,x;
            bool endCycle = false;
            while (!endCycle && (DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime) < 10000) {
                x = (a + b) / 2;
                fa = _f.calculate(a);
                fb = _f.calculate(b);
                fx = _f.calculate(x);
                if (fa*fb>0)
                {
                    points = "BadGap";
                    endCycle = true;
                }
                i++;
                if (IsExtremumFound(E, fx))
                {
                    points = $"({x}; {fx}); ";
                    endCycle = true;
                }
                else
                {
                    if (i == 1)
                    {
                        if (IsConvergence(fa, fb, i))
                        {
                            points = "BadGap";
                            endCycle = true;
                            break;
                        }
                    }
                    else
                    {
                        if (IsConvergence(fa, fx, i)) a = x;
                        else b = x;
                    }
                }
            }
            if ((DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime) > 10000) {
                points = "Timeout";
            }
        }

        public DihtomiaMethodRealisation(string f)
        {
            _f = new Function($"{f}");
        }

        public bool IsConvergence(double fa, double fx, int i)
        {
            if (i == 1 && fa* fx>0)
            {
                i = 0;
                return false;
            }
            else
            {
                if (fa * fx > 0)
                {
                    return true;
                }
                else return false;
            }
        }

        public bool IsExtremumFound(double E, double fx)
        {
            if (Math.Abs(fx) < E) return true;
            else return false;
        }

        public double ReturnFx(double x)
        {
            return _f.calculate(x);
        }

        public string GetPoints()
        {
            return points;
        }
    }
}
