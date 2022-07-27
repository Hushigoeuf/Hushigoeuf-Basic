using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Hushigoeuf
{
    public static class HGObjectExtensions
    {
        private static List<Component> _componentCache = new List<Component>();

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

        public static Component HGGetComponentNoAlloc(this GameObject @this, Type componentType)
        {
            @this.GetComponents(componentType, _componentCache);
            var component = _componentCache.Count > 0 ? _componentCache[0] : null;
            _componentCache.Clear();
            return component;
        }

        public static T HGGetComponentNoAlloc<T>(this GameObject @this) where T : Component
        {
            @this.GetComponents(typeof(T), _componentCache);
            var component = _componentCache.Count > 0 ? _componentCache[0] : null;
            _componentCache.Clear();
            return component as T;
        }
    }
}