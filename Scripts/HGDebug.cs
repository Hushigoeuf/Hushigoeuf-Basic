using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// Класс-оболочка для стандартных Debug-методов Unity.
    /// </summary>
    public partial class HGDebug
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }

        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.Log(message, context);
#endif
        }

        public static void LogError(object message)
        {
#if UNITY_EDITOR
            Debug.LogError(message);
#endif
        }

        public static void LogError(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.LogError(message, context);
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message);
#endif
        }

        public static void LogWarning(object message, Object context)
        {
#if UNITY_EDITOR
            Debug.LogWarning(message, context);
#endif
        }
    }
}