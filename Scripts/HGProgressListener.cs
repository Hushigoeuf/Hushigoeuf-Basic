using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf.Basic
{
    [HGBorders]
    [Serializable]
    public class HGProgressTrigger
    {
        public HGProgressTriggerTypes TriggerType;

        public float TargetValue;

        public bool LimitNumberOfCalls = true;

#if ODIN_INSPECTOR
        [EnableIf(nameof(LimitNumberOfCalls))] [MinValue(1)]
#endif
        public int NumberOfCalls = 1;

        [NonSerialized] public int CurrentNumberOfCalls;

        public virtual void CallTrigger(string progressID)
        {
            if (string.IsNullOrEmpty(progressID)) return;
            if (LimitNumberOfCalls)
                if (CurrentNumberOfCalls >= NumberOfCalls)
                    return;
            CurrentNumberOfCalls++;

            switch (TriggerType)
            {
                case HGProgressTriggerTypes.SetValue:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.SetValueRequest, TargetValue);
                    break;

                case HGProgressTriggerTypes.ExtendValue:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.ExtendValueRequest, TargetValue);
                    break;

                case HGProgressTriggerTypes.SetLimit:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.SetLimitRequest, TargetValue);
                    break;

                case HGProgressTriggerTypes.ExtendLimit:
                    HGProgressEvent.Trigger(progressID, HGProgressEventTypes.ExtendLimitRequest, TargetValue);
                    break;
            }
        }
    }

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

    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGProgressListener))]
    public class HGProgressListener : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        [HGBorders] [HGRequired] public string ProgressID;
        [HGBorders] [HGRequired] public bool ResetNumbersOfCallsOnEnable = true;

#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger MainTrigger;

#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger InitializeTrigger;

#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger EnableTrigger;

#if ODIN_INSPECTOR
        [Toggle(HGProgressToggleTrigger.TOGGLE_NAME)]
#endif
        public HGProgressToggleTrigger DisableTrigger;

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

        public virtual void CallMainTrigger()
        {
            if (MainTrigger != null)
                MainTrigger.TryTrigger(ProgressID);
        }

        public virtual void CallInitializeTrigger()
        {
            if (InitializeTrigger != null)
                InitializeTrigger.TryTrigger(ProgressID);
        }

        public virtual void CallEnableTrigger()
        {
            if (EnableTrigger != null)
                EnableTrigger.TryTrigger(ProgressID);
        }

        public virtual void CallDisableTrigger()
        {
            if (DisableTrigger != null)
                DisableTrigger.TryTrigger(ProgressID);
        }

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