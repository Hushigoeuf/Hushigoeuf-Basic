using System.Collections.Generic;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
    public enum HGProgressEventTypes
    {
        Initialized,
        ResponseRequest,
        Responsed,
        SetValueRequest,
        ExtendValueRequest,
        ValueChanged,
        ValueIncreased,
        ValueDecreased,
        ValueCompleted,
        ValueReturned,
        SetLimitRequest,
        ExtendLimitRequest,
        LimitChanged,
        StartActionRequest,
        StopActionRequest,
    }

    public struct HGProgressEvent
    {
        public HGProgressEventTypes EventType;
        public string ProgressID;
        public float EventValue;
        public float CurrentValue;
        public float MinValue;
        public float MaxValue;
        public string ActionID;

        public HGProgressEvent(string progressID, HGProgressEventTypes eventType, float eventValue = 0,
            float currentValue = 0, float minValue = 0, float maxValue = 0, string actionID = null)
        {
            ProgressID = progressID;
            EventType = eventType;
            EventValue = eventValue;
            CurrentValue = currentValue;
            MinValue = minValue;
            MaxValue = maxValue;
            ActionID = actionID;
        }

        public void Trigger()
        {
            HGEventManager.TriggerEvent(this);
        }

        private static HGProgressEvent e;

        public static void Trigger(string progressID, HGProgressEventTypes eventType, float eventValue = 0,
            float currentValue = 0, float minValue = 0, float maxValue = 0, string actionID = null)
        {
            e.ProgressID = progressID;
            e.EventType = eventType;
            e.EventValue = eventValue;
            e.CurrentValue = currentValue;
            e.MinValue = minValue;
            e.MaxValue = maxValue;
            e.ActionID = actionID;

            e.Trigger();
        }
    }

    /// <summary>
    /// Класс-контроллер, который занимается обработкой заданного прогресса.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGProgressController))]
    public class HGProgressController : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        /// ID прогресса, который будет обрабатываться
        [HGShowInSettings] [HGRequired] public string ProgressID;

        /// В какую сторону идут вычисления (по умолчанию, в положительную, от 0 до 10 и до бесконечности)
        [HGShowInSettings] [Range(0, 1)] public int LeftOrRight = 1;

        /// Минимальное значение прогресса
#if ODIN_INSPECTOR
        [MaxValue("$" + nameof(MaxValue))]
#endif
        [HGShowInSettings]
        public float MinValue;

        /// Стартовое значение прогресса
#if ODIN_INSPECTOR
        [MinValue("$" + nameof(MinValue))]
        [MaxValue("$" + nameof(MaxValue))]
#endif
        [HGShowInSettings]
        public float InitValue = 2;

        /// Максимальное значение прогресса
#if ODIN_INSPECTOR
        [MinValue("$" + nameof(MinValue))]
#endif
        [HGShowInSettings]
        public float MaxValue = 8;

        /// Заблокировать операции если значение достигло максимального значения
        [HGShowInSettings] public bool BlockOnCompleted = true;

        [HGShowInDebug] [HGReadOnly] [HGShowInInspector]
        protected float _currentValue;

        [HGShowInDebug] [HGReadOnly] [HGShowInInspector]
        protected bool _blocked;

        /// Список с ID и статусом выполнения (суть в том, чтобы повторные действия с этим ID не выполнялись)
        protected Dictionary<string, bool> _actions = new Dictionary<string, bool>();

        protected virtual void Awake()
        {
            _currentValue = InitValue;
        }

        protected virtual void Start()
        {
            TriggerEvent(HGProgressEventTypes.Initialized);
        }

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        /// <summary>
        /// Инициализирует новое действие с контроллером по заданному ID.
        /// </summary>
        protected virtual bool StartAction(string actionID)
        {
            if (!string.IsNullOrEmpty(actionID))
                if (!_actions.ContainsKey(actionID))
                {
                    _actions.Add(actionID, false);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Отмечает, что действие с контроллером завершено.
        /// </summary>
        protected virtual bool UseAction(string actionID)
        {
            if (string.IsNullOrEmpty(actionID) ||
                !_actions.ContainsKey(actionID))
                return true;
            if (_actions[actionID]) return false;

            _actions[actionID] = true;
            return true;
        }

        /// <summary>
        /// Заканчивает действие с контроллером по заданному ID.
        /// </summary>
        protected virtual bool StopAction(string actionID)
        {
            if (!string.IsNullOrEmpty(actionID))
                if (_actions.ContainsKey(actionID))
                {
                    _actions.Remove(actionID);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// Установить значение для прогресса.
        /// </summary>
        public virtual void SetValue(float value)
        {
            if (_blocked) return;

            var oldValue = _currentValue;
            _currentValue = Mathf.Clamp(value, MinValue, MaxValue);

            var leftOrRightChanged = -1;
            if (_currentValue > oldValue)
                leftOrRightChanged = 1;
            else if (_currentValue < oldValue)
                leftOrRightChanged = 0;
            if (leftOrRightChanged == -1) return;

            TriggerEvent(HGProgressEventTypes.ValueChanged);
            if (leftOrRightChanged == LeftOrRight)
                TriggerEvent(HGProgressEventTypes.ValueIncreased, _currentValue - oldValue);
            else
                TriggerEvent(HGProgressEventTypes.ValueDecreased, oldValue - _currentValue);

            var leftOrRightCompleted = -1;
            if (_currentValue <= MinValue)
                leftOrRightCompleted = 0;
            else if (_currentValue >= MaxValue)
                leftOrRightCompleted = 1;

            if (LeftOrRight == leftOrRightCompleted)
                TriggerEvent(HGProgressEventTypes.ValueCompleted);
            else if (leftOrRightCompleted != -1) TriggerEvent(HGProgressEventTypes.ValueReturned);

            if (LeftOrRight == leftOrRightCompleted)
                if (BlockOnCompleted)
                    _blocked = true;
        }

        /// <summary>
        /// Расширить значения прогресса (например, прогресс 10 при задании в +1 установит прогресс в 11).
        /// </summary>
        public virtual void ExtendValue(float value)
        {
            if (LeftOrRight == 1)
                SetValue(_currentValue + value);
            else SetValue(_currentValue - value);
        }

        /// <summary>
        /// Установить лимитное значение для прогресса.
        /// </summary>
        public virtual void SetLimit(float value)
        {
            if (LeftOrRight == 1)
                MaxValue = Mathf.Clamp(value, MinValue, float.MaxValue);
            else
                MinValue = Mathf.Clamp(value, float.MinValue, MaxValue);
            SetValue(Mathf.Clamp(_currentValue, MinValue, MaxValue));

            TriggerEvent(HGProgressEventTypes.LimitChanged);
        }

        /// <summary>
        /// Расширить лимит аналогично расширению значения.
        /// </summary>
        public virtual void ExtendLimit(float value)
        {
            if (LeftOrRight == 1)
                SetLimit(MaxValue + value);
            else SetLimit(MinValue - value);
        }

        protected virtual void TriggerEvent(HGProgressEventTypes eventType, float eventValue = 0)
        {
            HGProgressEvent.Trigger(ProgressID, eventType, eventValue, _currentValue, MinValue, MaxValue);
        }

        public virtual void OnHGEvent(HGProgressEvent e)
        {
            if (e.ProgressID != ProgressID) return;

            switch (e.EventType)
            {
                // Отдаем параметры прогресса в ответ на запрос обратной связи
                case HGProgressEventTypes.ResponseRequest:
                    TriggerEvent(HGProgressEventTypes.Responsed);
                    break;

                // Установить значение
                case HGProgressEventTypes.SetValueRequest:
                    if (UseAction(e.ProgressID))
                        SetValue(e.EventValue);
                    break;

                // Расширить значение
                case HGProgressEventTypes.ExtendValueRequest:
                    if (UseAction(e.ActionID))
                        ExtendValue(e.EventValue);
                    break;

                // Установить лимит на значение
                case HGProgressEventTypes.SetLimitRequest:
                    if (UseAction(e.ActionID))
                        SetLimit(e.EventValue);
                    break;

                // Расширить лимит на значение
                case HGProgressEventTypes.ExtendLimitRequest:
                    if (UseAction(e.ActionID))
                        ExtendLimit(e.EventValue);
                    break;

                // Инициализировать новое действие с контроллером
                case HGProgressEventTypes.StartActionRequest:
                    StartAction(e.ActionID);
                    break;

                // Завершить действие с контроллером
                case HGProgressEventTypes.StopActionRequest:
                    StopAction(e.ActionID);
                    break;
            }
        }
    }
}