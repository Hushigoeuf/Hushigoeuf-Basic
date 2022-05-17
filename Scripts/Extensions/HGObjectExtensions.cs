using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hushigoeuf
{
    public static class HGObjectExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetActive(this GameObject target, bool value)
        {
            if (target != null)
                if (target.activeSelf != value)
                    target.SetActive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void HGSetActive(this Component target, bool value)
        {
            if (target != null)
                target.gameObject.HGSetActive(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HGIsActive(this GameObject target, bool onlySelf = true)
        {
            if (target != null)
            {
                if (onlySelf)
                    return target.activeSelf;
                return target.activeInHierarchy;
            }

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HGIsActive(this Component target, bool onlySelf = true)
        {
            if (target != null)
                return target.gameObject.HGIsActive(onlySelf);
            return false;
        }
    }
}