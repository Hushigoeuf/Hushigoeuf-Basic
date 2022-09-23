using UnityEngine;

namespace Hushigoeuf.Basic
{
    public static class HGDebugExtensions
    {
        public static void HGLog(this object message)
        {
            HGDebug.Log(message);
        }

        public static void HGLog(this object message, Object context)
        {
            HGDebug.Log(message, context);
        }

        public static void HGLogError(this object message)
        {
            HGDebug.LogError(message);
        }

        public static void HGLogError(this object message, Object context)
        {
            HGDebug.LogError(message, context);
        }

        public static void HGLogWarning(this object message)
        {
            HGDebug.LogWarning(message);
        }

        public static void HGLogWarning(this object message, Object context)
        {
            HGDebug.LogWarning(message, context);
        }
    }
}