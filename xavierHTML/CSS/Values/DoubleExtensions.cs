using System;

namespace xavierHTML.CSS.Values
{
    public static class DoubleExtensions
    {
        public static double Clamp (this double self, double min, double max)
        {
            return Math.Min (max, Math.Max (self, min));
        }

        public static int Round(this double self)
        {
            return (int) Math.Round(self, 0);
        }
    }
}
