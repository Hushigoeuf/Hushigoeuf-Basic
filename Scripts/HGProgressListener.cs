﻿using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
    /// <summary>
    /// Параметры триггера.
    /// </summary>
    [HGBorders]
    [Serializable]
    public class HGProgressTrigger
    {
        /// Тип триггера/действия над прогрессом
        public HGProgressTriggerTypes TriggerType;

        /// Значение которое будет привязано к вызовам
        public float TargetValue;

        /// Ограничить кол-во вызовов
        public bool LimitNumberOfCalls = true;

        /// Максимальное кол-во вызовов
#if ODIN_INSPECTOR
        [EnableIf(nameof(LimitNumberOfCalls))] [MinValue(1)]
#endif
        public int NumberOfCalls = 1;

        [NonSerialized] public int CurrentNumberOfCalls;

        /// <summary>
        /// Вызвать триггер (отправит событие-запрос для контроллеров прогресса).
        /// </summary>
        public virtual void CallTrigger(string progressID)
        {
            if (string.IsNullOrEmpty(progressID)) return;
            if (LimitNumberOfCalls)
                if (CurrentNumberOfCalls >= NumberOfCalls)
                    return;
            CurrentNumberOfCalls++;

            switch (TriggerType)
            {
                // Установить значение
                case HGProgressTriggerTypes.SetValue:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.SetValueRequest, TargetValue);
                    break;

                // Расширить значение
                case HGProgressTriggerTypes.ExtendValue:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.ExtendValueRequest, TargetValue);
                    break;

                // Установить лимит для значения
                case HGProgressTriggerTypes.SetLimit:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.SetLimitRequest, TargetValue);
                    break;

                // Расширить лимит для значения
                case HGProgressTriggerTypes.ExtendLimit:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.ExtendLimitRequest, TargetValue);
                    break;
            }
        }
    }

    /// <summary>
    /// Параметры триггера для инспектора.
    /// </summary>
    [Serializable]
    public class HGProgressToggleTrigger : HGProgressTrigger
    {
        public const string TOGGLE_NAME = nameof(IsEnabled);

        public bool IsEnabled;

        public virtual bool TryTrigger(string progressID)
        {
            if (IsEnabled)
            {
                CallTrigger(progressID);
                return true;
            }

            return false;
        }
    }

    public enum HGProgressTriggerTypes
    {
        SetValue,
        ExtendValue,
        SetLimit,
        ExtendLimit,
    }

    /// <summary>
    /// Изменяет параметры прогресса по заданным критериям.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGProgressListener))]
    public class HGProgressListener : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        /// Целевой ID прогресса
        [HGBorders] [HGRequired] public string ProgressID;

        /// Сбрасывать ли кол-во вызовов при активации объекта
        [HGBorders] [HGRequired] public bool ResetNumbersOfCallsOnEnable = true;

        /// Может быть вызван только один триггер за все существование объекта
        [HGBorders] public bool OnlyOneTriggerAtOnce;

        /// Может быть вызван только один триггер за все существование объекта
        [HGBorders] public bool OnlyOneTriggerAtOnce2;

        /// Параметры основного триггера
#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger MainTrigger;

        /// Параметры стартового триггера
#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger InitializeTrigger;

        /// Параметры триггера при активации объекта
#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger EnableTrigger;

        /// Параметры триггера при деактивации объекта
#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger DisableTrigger;

        /// Параметры триггера при уничтожении объекта
#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger DestroyTrigger;

        protected bool _initialized;

        protected virtual void Start()
        {
            if (!_initialized) HGProgressEvent.Trigger(ProgressID, HGProgressEventTypes.ResponseRequest);
        }

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();

            if (!_initialized)
            {
                HGProgressEvent.Trigger(ProgressID, HGProgressEventTypes.ResponseRequest);
                return;
            }

            if (ResetNumbersOfCallsOnEnable)
            {
                if (MainTrigger != null) MainTrigger.NumberOfCalls = 0;
                if (InitializeTrigger != null) InitializeTrigger.NumberOfCalls = 0;
                if (EnableTrigger != null) EnableTrigger.NumberOfCalls = 0;
                if (DisableTrigger != null) DisableTrigger.NumberOfCalls = 0;
                if (DestroyTrigger != null) DestroyTrigger.NumberOfCalls = 0;
            }

            CallEnableTrigger();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();

            CallDisableTrigger();
        }

        protected virtual void OnDestroy()
        {
            CallDestroyTrigger();
        }

        /// <summary>
        /// Вызывает основной триггер.
        /// </summary>
        public virtual void CallMainTrigger()
        {
            if (MainTrigger != null)
                MainTrigger.TryTrigger(ProgressID);
        }

        /// <summary>
        /// Вызывает стартовый триггер.
        /// </summary>
        public virtual void CallInitializeTrigger()
        {
            if (InitializeTrigger != null)
                InitializeTrigger.TryTrigger(ProgressID);
        }

        /// <summary>
        /// Вызывает триггер при активации объекта.
        /// </summary>
        public virtual void CallEnableTrigger()
        {
            if (EnableTrigger != null)
                EnableTrigger.TryTrigger(ProgressID);
        }

        /// <summary>
        /// Вызывает триггер при деактивации объекта.
        /// </summary>
        public virtual void CallDisableTrigger()
        {
            if (DisableTrigger != null)
                DisableTrigger.TryTrigger(ProgressID);
        }

        /// <summary>
        /// Вызывает триггер при уничтожении объекта.
        /// </summary>
        public virtual void CallDestroyTrigger()
        {
            if (DestroyTrigger != null)
                DestroyTrigger.TryTrigger(ProgressID);
        }

        public virtual void OnHGEvent(HGProgressEvent e)
        {
            if (e.ProgressID != ProgressID) return;

            switch (e.EventType)
            {
                case HGProgressEventTypes.Responsed:
                    if (!_initialized)
                    {
                        CallInitializeTrigger();
                        CallEnableTrigger();
                    }

                    _initialized = true;
                    break;
            }
        }
    }
}