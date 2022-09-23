using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Hushigoeuf.Basic
{
    public sealed class HGResources
    {
        public const string START_PATH = HGEditor.BASE + HGEditor.SEPARATOR;

        public static string GetPath(string path) => START_PATH + path;

        public static Object Load(string path, Type systemTypeInstance) =>
            Resources.Load(GetPath(path), systemTypeInstance);

        public static Object Load(string path) => Load(path, typeof(Object));

        public static T Load<T>(string path) where T : Object => Resources.Load<T>(GetPath(path));

        public static Object[] LoadAll(string path, Type systemTypeInstance) =>
            Resources.LoadAll(GetPath(path), systemTypeInstance);

        public static Object[] LoadAll(string path) => LoadAll(path, typeof(Object));

        public static T[] LoadAll<T>(string path) where T : Object => Resources.LoadAll<T>(GetPath(path));
    }
}