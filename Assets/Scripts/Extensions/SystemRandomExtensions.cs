//using System;
namespace HackedDesign
{
    public static class SystemRandomExtensions
    {
        public static float NextFloat(this System.Random rng, float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        public static float NextFloat(this System.Random rng, float max)
        {
            return (float)(rng.NextDouble() * max);
        }
    }
}
