using System;
using System.ComponentModel;
using System.Net.NetworkInformation;
using Microsoft.Xna.Framework;

namespace ConsoleApp12
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using var game = new Game1();
            game.Run();
        }

        private static float GetY(float x, float ax, float ay, float bx, float by)
        {
            return (GetM(ax, ay, bx, by) * (x - ax) + ay);
        }

        public static (float, float) GetCoords(float x, float ax, float ay, float bx, float by)
        {
            x = Math.Clamp(x, 0f, 1f);
            var (dirX, dirY) = GetDirection(ax, ay, bx, by);
            return (ax + dirX * x, ay + dirY * x);
        }
        
        public static Vector2 GetCoords(float amount, Vector2 a, Vector2 b)
        {
            var (ax, ay) = a;
            var (bx, by) = b;
            var (x, y) = GetCoords(amount, ax, ay, bx, by);
            return new Vector2(x, y);
        }
        
        private static (float, float) Normalise(float x, float y)
        {
            var length = GetLength(0, 0, x, y);
            return (x / length, y / length);
        }
        
        private static (float, float) GetDirection(float ax, float ay, float bx, float by)
        {
            return (bx - ax, by - ay);
        }

        private static float GetLength(float ax, float ay, float bx, float by)
        {
            var (x, y) = GetDirection(ax, ay, bx, by);
            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        private static float GetM(float ax, float ay, float bx, float by)
        {
            return (by - ay) / (bx - ax);
        }

        private static float GetAlpha(float ax, float ay, float bx, float by)
        {
            return (float)Math.Atan(GetM(ax, ay, bx, @by));
        }

        private static float ToDegree(this float a)
        {
            return (float)(a * (180 / Math.PI));
        }

        private static float ToRadiant(this float a)
        {
            return (float)(a * (Math.PI / 180));
        }
    }
}