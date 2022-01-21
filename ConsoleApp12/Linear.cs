using System;

namespace ConsoleApp12
{
    public static class Linear
    {
        public static (float, float) GetPositionByPercentage(float percentage, float xa, float ya, float xb, float yb)
        {
            percentage = Math.Clamp(percentage, 0f, 1f);
            var (xdir, ydir) = GetDirection(xa, ya, xb, yb);
            return (xa + xdir * percentage, ya + ydir * percentage);
        }
        
        public static (float, float) GetDirection(float xa, float ya, float xb, float yb)
        {
            return (xb - xa, yb - ya);
        }
    }
}