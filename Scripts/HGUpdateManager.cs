using UnityEngine;

namespace Hushigoeuf
{
    public enum HGUpdateEventTypes
    {
        Update,
        FixedUpdate,
        LateUpdate,
    }

    public struct HGUpdateEvent
    {
        public HGUpdateEventTypes EventType;
        public float Time;
        public float DateTime;
        public float FixedDateTime;

        public HGUpdateEvent(HGUpdateEventTypes eventType, float time, float dateTime, float fixedDateTime)
        {
            EventType = eventType;
            Time = time;
            DateTime = dateTime;
            FixedDateTime = fixedDateTime;
        }

        private static HGUpdateEvent e;

        public static void Trigger(HGUpdateEventTypes eventType, float time, float dateTime, float fixedDateTime)
        {
            e.EventType = eventType;
            e.Time = time;
            e.DateTime = dateTime;
            e.FixedDateTime = fixedDateTime;

            HGEventManager.TriggerEvent(e);
        }
    }

    /// <summary>
    /// Менеджер событий для Update, FixedUpdate, LateUpdate.
    /// Пока работает через HGEventManager.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGUpdateManager))]
    public class HGUpdateManager : HGSingletonMonoBehaviour<HGUpdateManager>
    {
        protected virtual void Update()
        {
            HGUpdateEvent.Trigger(HGUpdateEventTypes.Update, Time.time, Time.deltaTime, Time.fixedDeltaTime);
        }

        protected virtual void FixedUpdate()
        {
            HGUpdateEvent.Trigger(HGUpdateEventTypes.FixedUpdate, Time.time, Time.deltaTime, Time.fixedDeltaTime);
        }

        protected virtual void LateUpdate()
        {
            HGUpdateEvent.Trigger(HGUpdateEventTypes.LateUpdate, Time.time, Time.deltaTime, Time.fixedDeltaTime);
        }
    }
}