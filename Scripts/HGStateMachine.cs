using System;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    public struct HGStateEvent<T> where T : struct, IComparable, IConvertible, IFormattable
    {
        public GameObject Target;
        public HGStateMachine<T> TargetStateMachine;
        public T NewState;
        public T PreviousState;

        public HGStateEvent(HGStateMachine<T> stateMachine)
        {
            Target = stateMachine.Target;
            TargetStateMachine = stateMachine;
            NewState = stateMachine.CurrentState;
            PreviousState = stateMachine.PreviousState;
        }
    }

    public interface HGIStateMachine
    {
        bool TriggerEvents { get; set; }
    }

    public class HGStateMachine<T> : HGIStateMachine where T : struct, IComparable, IConvertible, IFormattable
    {
        public delegate void OnStateChangeDelegate();

        public GameObject Target;
        public OnStateChangeDelegate OnStateChange;

        public bool TriggerEvents { get; set; }
        public T CurrentState { get; protected set; }
        public T PreviousState { get; protected set; }


        public HGStateMachine(GameObject target, bool triggerEvents)
        {
            Target = target;
            TriggerEvents = triggerEvents;
        }

        public virtual void ChangeState(T newState)
        {
            if (newState.Equals(CurrentState)) return;

            PreviousState = CurrentState;
            CurrentState = newState;

            OnStateChange?.Invoke();

            if (TriggerEvents) HGEventManager.TriggerEvent(new HGStateEvent<T>(this));
        }

        public virtual void RestorePreviousState()
        {
            CurrentState = PreviousState;
            OnStateChange?.Invoke();
            if (TriggerEvents) HGEventManager.TriggerEvent(new HGStateEvent<T>(this));
        }
    }
}