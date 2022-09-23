using System.Collections.Generic;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    public partial class HGRandom
    {
        public static float Range(float min, float max) => Random.Range(min, max);

        public static int Range(int min, int max, bool inclusive) =>
            min != max ? Random.Range(min, max + (inclusive ? 1 : 0)) : min;

        public static float Range(Vector2 range) => Range(range.x, range.y);

        public static int Range(Vector2Int range, bool inclusive) => Range(range.x, range.y, inclusive);

        public static float GetValue() => Random.value;

        public static bool IsChance(float criteria) => Mathf.Clamp01(criteria) >= Range(.0f, 1f);

        public static int GetIndex(int count) => Range(0, count, false);

        public static T Choose<T>(params T[] values) => values[GetIndex(values.Length)];

        public static void Shuffle<T>(T[] values)
        {
            var random = new System.Random();
            for (int i = values.Length - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                (values[j], values[i]) = (values[i], values[j]);
            }
        }

        public static void Shuffle<T>(List<T> values)
        {
            var random = new System.Random();
            for (int i = values.Count - 1; i >= 1; i--)
            {
                int j = random.Next(i + 1);
                (values[j], values[i]) = (values[i], values[j]);
            }
        }

        public static Vector2 GetRandomPositionInsideCircle(Vector2 center, float radius) =>
            center + Random.insideUnitCircle * radius;
    }
}