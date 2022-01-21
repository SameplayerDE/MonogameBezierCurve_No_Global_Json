using System.Runtime.CompilerServices;

namespace ConsoleApp12
{
    public static class Bezier
    {
        public static (float, float) LinearBezier(
            float x,
            float xa, float ya,
            float xb, float yb)
        {
            var (xab, yab) = Linear.GetPositionByPercentage(
                x,
                xa, ya,
                xb, yb
            );
            return (xab, yab);
        }
        
        public static (float, float) QuadraticBezier(
            float x,
            float xa, float ya,
            float xb, float yb,
            float xc, float yc)
        {
            var (xab, yab) = Linear.GetPositionByPercentage(
                x,
                xa, ya,
                xb, yb
            );
            
            var (xbc, ybc) = Linear.GetPositionByPercentage(
                x,
                xb, yb,
                xc, yc
            );
            
            var (xabbc, yabbc) = Linear.GetPositionByPercentage(
                x,
                xab, yab,
                xbc, ybc
            );

            return (xabbc, yabbc);
        }
        
        public static (float, float) CubicBezier(
            float x,
            float xa, float ya,
            float xb, float yb,
            float xc, float yc,
            float xd, float yd)
        {
            var (xab, yab) = Linear.GetPositionByPercentage(
                x,
                xa, ya,
                xb, yb
            );
            
            var (xbc, ybc) = Linear.GetPositionByPercentage(
                x,
                xb, yb,
                xc, yc
            );
            
            var (xcd, ycd) = Linear.GetPositionByPercentage(
                x,
                xc, yc,
                xd, yd
            );
            
            var (xabbc, yabbc) = Linear.GetPositionByPercentage(
                x,
                xab, yab,
                xbc, ybc
            );
            
            var (xbccd, ybccd) = Linear.GetPositionByPercentage(
                x,
                xbc, ybc,
                xcd, ycd
            );
            
            var (xabbccd, ybabbccd) = Linear.GetPositionByPercentage(
                x,
                xabbc, yabbc,
                xbccd, ybccd
            );

            return (xabbccd, ybabbccd);
        }
        
    }
}