using System;

namespace CFW.Business
{
    public static class Algorithm
    {

        public static double LowPassFilter(double val, double last, double RC, double dt)
        {
            if (last == 0) // If it's a valid 0 it doesn't make a difference in the filter
                return val;

            double alpha = dt / (RC + dt); // smoothing factor, 0 to 1

            val = val * alpha + last * (1.0 - alpha);

            return val;
        }

        public static double WrapAngle(double ang)
        {
            while (ang > 360) { ang -= 360; }
            while (ang < 0) { ang += 360; }

            return ang;
        }

        public static double WrapQ2toQ4(double ang)
        {
            while (ang > 180) { ang -= 360; }
            while (ang < -180) { ang += 360; }

            return ang;
        }

        public static double Clamp(double val, double min, double max)
        {
            return val <= min ? min : val >= max ? max : val;
        }
    }
}
