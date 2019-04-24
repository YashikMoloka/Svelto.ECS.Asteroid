using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
namespace Code.Utils
{
    public class AsteroidSpawnUtil
    {
        private const int N_MIN = 8;
        private const int N_MAX = 10;
//        private const double R_MIN = 0.3;
                                           //        private const double R_MAX = 1.0;
        
        public static List<Vector2> GenerateRandom(Vector2 size, Random rnd = null)
        {
            if (rnd == null)
                rnd = new Random(Guid.NewGuid().GetHashCode());

            var n = rnd.Next(N_MIN, N_MAX);

            var angles = GenerateAngles(n, rnd);
            var radius = Enumerable.Range(0, n).Select(x => (rnd.NextDouble() * (size.y - size.x)) + size.x).ToList();

            var points = Enumerable.Range(0, n).Select(x => new Vector2((float)angles[x], (float)radius[x])).ToList();
            return points;
        }

        private static List<double> GenerateAngles(int n, Random rng)
        {
            var angles = Enumerable.Range(0, n).Select(x => rng.NextDouble() + 0.2).ToList();
            var norm = Math.PI * 2 / angles.Sum();
            var normalized = angles.Select(a => a * norm).ToList();

            var add = new List<double>();
            var sum = 0.0;
            foreach (var a in normalized)
            {
                sum += a;
                add.Add(sum);
            }

            return add;
        }
        
        public static Vector2 Polar2Cartesian(Vector2 point)
        {
            return new Vector2((float) (point.y * Math.Cos(point.x)), (float) (point.y * -Math.Sin(point.x)));
        }
    }
}