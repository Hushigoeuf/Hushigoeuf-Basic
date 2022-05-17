using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hushigoeuf
{
    public static class HGTransformExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPosition(this Transform target, Vector2 position)
        {
            target.HGSetPosition(position.x, position.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPosition(this Transform target, float x, float y)
        {
            target.position = new Vector3(x, y, target.position.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPosition(this Transform target, Vector3 position)
        {
            target.position = position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPosition(this Transform target, float x, float y, float z)
        {
            target.HGSetPosition(new Vector3(x, y, z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPositionX(this Transform target, float value)
        {
            target.HGSetPosition(value, target.position.y, target.position.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPositionY(this Transform target, float value)
        {
            target.HGSetPosition(target.position.x, value, target.position.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetPositionZ(this Transform target, float value)
        {
            target.HGSetPosition(target.position.x, target.position.y, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPosition(this Transform target, Vector2 position)
        {
            target.HGSetLocalPosition(position.x, position.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPosition(this Transform target, float x, float y)
        {
            target.localPosition = new Vector3(x, y, target.localPosition.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPosition(this Transform target, Vector3 position)
        {
            target.localPosition = position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPosition(this Transform target, float x, float y, float z)
        {
            target.HGSetLocalPosition(new Vector3(x, y, z));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPositionX(this Transform target, float value)
        {
            target.HGSetLocalPosition(value, target.localPosition.y, target.localPosition.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPositionY(this Transform target, float value)
        {
            target.HGSetLocalPosition(target.localPosition.x, value, target.localPosition.z);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetLocalPositionZ(this Transform target, float value)
        {
            target.HGSetLocalPosition(target.localPosition.x, target.localPosition.y, value);
        }
    }
}