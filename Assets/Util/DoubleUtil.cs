using System;

namespace Assets.Util
{
    public static class DoubleUtil
    {
        public static bool EqualDoubles( double first, double second )
        {
            const double e = 0.01;
            return Math.Abs( first - second ) < e;
        }
    }
}
