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

        /// <summary>
        /// Y Wert von der X Position auf der Gerade zwischen A und B
        /// </summary>
        /// <param name="x"></param>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <returns>Y Wert von der X Position auf der Gerade zwischen A und B</returns>
        private static float GetY(float x, float ax, float ay, float bx, float by)
        {
            return (GetM(ax, ay, bx, by) * (x - ax) + ay);
        }

        /// <summary>
        /// Koordinate auf gerade zwischen 0 und 1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static (float, float) GetCoords(float x, float ax, float ay, float bx, float by)
        {
            x = Math.Clamp(x, 0f, 1f);
            var (dirX, dirY) = GetDirection(ax, ay, bx, by);
            return (ax + dirX * x, ay + dirY * x);
        }

        /// <summary>
        /// Koordinate auf gerade zwischen 0 und 1
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Steigung der gerade zwischen a und b
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        private static float GetM(float ax, float ay, float bx, float by)
        {
            return (by - ay) / (bx - ax);
        }

        /// <summary>
        /// Winkel zwischen A und B
        /// </summary>
        /// <param name="ax"></param>
        /// <param name="ay"></param>
        /// <param name="bx"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        private static float GetAlpha(float ax, float ay, float bx, float by)
        {
            return (float)Math.Atan(GetM(ax, ay, bx, @by));
        }

        /// <summary>
        /// Winkel zu Grad
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static float ToDegree(this float a)
        {
            return (float)(a * (180 / Math.PI));
        }

        /// <summary>
        /// Winkel zu Bogengrad
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static float ToRadiant(this float a)
        {
            return (float)(a * (Math.PI / 180));
        }
    }
}