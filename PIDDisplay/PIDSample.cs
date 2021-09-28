using System;

namespace PIDDisplay
{
    public class PIDSample
    {
        public double p;
        public double i;
        public double d;
        public double total;

        public PIDSample(Autopilot.PID pid)
        {
            p = pid.p;
            i = pid.i;
            d = pid.d;
            total = pid.outputValue;
        }
    }
}
