using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    public enum HGGameEventTypes
    {
        Custom,
        LevelLoaded,
        StartLevelRequest,
        LevelStarted,
        GameOverRequest,
        GameOver,
        FinishLevelRequest,
        LevelFinished,
        PauseRequest,
        Paused,
        UnPauseRequest,
        UnPaused,
        PlayerDeath,
    }

    /// <summary>
    /// HGGameEvent может использоваться для общих событий (начало игры, потеря жизней и т.д.)
    /// </summary>
    public struct HGGameEvent
    {
        public HGGameEventTypes EventType;
        public string EventName;

        public HGGameEvent(string eventName)
        {
            EventType = HGGameEventTypes.Custom;
            EventName = eventName;
        }

        public HGGameEvent(HGGameEventTypes eventType)
        {
            EventType = eventType;
            EventName = null;
        }

        private static HGGameEvent e;

        public static void Trigger(HGGameEventTypes eventType, string eventName = null)
        {
            e.EventType = eventType;
            e.EventName = eventName;

            HGEventManager.TriggerEvent(e);
        }
    }

    /// <summary>
    /// Этот класс отвечает за управление событиями и может использоваться для трансляции событий по всей игре,
    /// чтобы сообщить одному классу (или нескольким) о том, что что-то произошло.
    ///
    /// События — это структуры, вы можете определить любой тип событий, какой захотите.
    /// Этот менеджер поставляется с HGGameEvent, который в основном просто состоит из строки,
    /// но вы можете работать с более сложными типами, если хотите.
    ///
    /// Чтобы вызвать новое событие из любого места, выполните YOUR_EVENT.Trigger(YOUR_PARAMETERS)
    /// Итак, HGGameEvent.Trigger("GameStart"), будет транслировать HGGameEvent со строкой GameStart всем слушателям.
    ///
    /// Чтобы начать слушать событие из любого класса, необходимо сделать 3 вещи:
    ///
    /// 1 - сообщите, что ваш класс реализует интерфейс HGEventListener для такого рода событий.
    /// Например: public class EventExample : MonoBehaviour, HGEventListener<HGGameEvent>
    /// У вас может быть более одного из них (по одному на тип события).
    ///
    /// 2 - OnEnable и OnDisable, соответственно, запускает и останавливает прослушивание события:
    /// 
    /// void OnEnable()
    /// {
    /// 	this.HGEventStartListening<HGGameEvent>();
    /// }
    /// 
    /// void OnDisable()
    /// {
    /// 	this.HGEventStopListening<HGGameEvent>();
    /// }
    ///
    /// 3 - Реализовать интерфейс HGEventListener для этого события. Например:
    /// 
    /// public void OnHGEvent(HGGameEvent e)
    /// {
    /// 	if (e.EventName == "GameStart")
    ///		{
    ///			// DO SOMETHING
    ///		}
    /// }
    ///
    /// Пример выше будет перехватывать все события типа HGGameEvent, генерируемые из любого места в игре.
    /// </summary>
    [ExecuteAlways]
    public static class HGEventManager
    {
        private static Dictionary<Type, List<HGEventListenerBase>> _subscribers;

        static HGEventManager()
        {
            _subscribers = new Dictionary<Type, List<HGEventListenerBase>>();
        }

        /// <summary>
        /// Добавляет нового подписчика на заданное событие.
        /// </summary>
        public static void AddListener<TEvent>(HGEventListener<TEvent> listener) where TEvent : struct
        {
            var eventType = typeof(TEvent);

            if (!_subscribers.ContainsKey(eventType))
                _subscribers[eventType] = new List<HGEventListenerBase>();

            if (!SubscriptionExists(eventType, listener))
                _subscribers[eventType].Add(listener);
        }

        /// <summary>
        /// Удаляет подписчика с заданного события.
        /// </summary>
        public static void RemoveListener<TEvent>(HGEventListener<TEvent> listener) where TEvent : struct
        {
            var eventType = typeof(TEvent);

            if (!_subscribers.ContainsKey(eventType)) return;

            var subscriberList = _subscribers[eventType];

            for (var i = 0; i < subscriberList.Count; i++)
                if (subscriberList[i] == listener)
                {
                    subscriberList.Remove(subscriberList[i]);

                    if (subscriberList.Count == 0) _subscribers.Remove(eventType);

                    return;
                }
        }

        /// <summary>
        /// Запускает событие. Все экземпляры, которые подписаны на него, получат запрос на действие.
        /// </summary>
        public static void TriggerEvent<TEvent>(TEvent e) where TEvent : struct
        {
            List<HGEventListenerBase> list;

            if (!_subscribers.TryGetValue(typeof(TEvent), out list)) return;

            for (var i = 0; i < list.Count; i++) (list[i] as HGEventListener<TEvent>).OnHGEvent(e);
        }

        /// <summary>
        /// Проверяет, есть ли подписчики у события заданного типа.
        /// </summary>
        private static bool SubscriptionExists(Type type, HGEventListenerBase receiver)
        {
            List<HGEventListenerBase> receivers;

            if (!_subscribers.TryGetValue(type, out receivers)) return false;

            var exists = false;

            for (var i = 0; i < receivers.Count; i++)
                if (receivers[i] == receiver)
                {
                    exists = true;
                    break;
                }

            return exists;
        }
    }

    /// <summary>
    /// Статический класс, который позволяет любому классу запускать или останавливать прослушивание событий.
    /// </summary>
    public static class EventRegister
    {
        public delegate void Delegate<TEvent>(TEvent eventType);

        public static void HGEventStartListening<TEvent>(this HGEventListener<TEvent> caller) where TEvent : struct
        {
            HGEventManager.AddListener<TEvent>(caller);
        }

        public static void HGEventStopListening<TEvent>(this HGEventListener<TEvent> caller) where TEvent : struct
        {
            HGEventManager.RemoveListener<TEvent>(caller);
        }
    }

    public interface HGEventListenerBase
    {
    }

    /// <summary>
    /// Открытый интерфейс, который необходимо реализовать для каждого типа событий, которые надо прослушивать.
    /// </summary>
    public interface HGEventListener<TEvent> : HGEventListenerBase
    {
        void OnHGEvent(TEvent e);
    }
}