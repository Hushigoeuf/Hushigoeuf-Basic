using System;
using UnityEngine.Events;

namespace Hushigoeuf
{
    /// <summary>
    /// Базовый класс для UI-представления.
    /// Класс выполнен в том виде, чтобы его можно было адаптировать под разные решение, к примеру, DoozyUI.
    /// </summary>
    public abstract class HGSimpleUIView : HGMonoBehaviour, HGEventListener<HGUIRequestEvent>
    {
        /// Имя представления
        [HGShowInSettings] [HGBorders] [HGRequired]
        public string ViewName;

        [HGShowInEvents] public UnityEvent OnShowFinished;
        [HGShowInEvents] public UnityEvent OnHideFinished;

        /// Последний ID запроса, который привязан к действиям над представлением
        [NonSerialized] public string CurrentRequestID;

        [NonSerialized] public bool CurrentInstant;

        public virtual bool IsHidden { get; }
        public virtual bool IsHiding { get; }
        public virtual bool IsShowing { get; }
        public virtual bool IsVisible { get; }

        public bool IsHiddenOrHiding => IsHidden || IsHiding;
        public bool IsShowingOrVisible => IsShowing || IsVisible;

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        /// <summary>
        /// Показать представление.
        /// </summary>
        public virtual void Show(bool instant = false)
        {
            CurrentInstant = instant;
        }

        public void Show(string requestID, bool instant = false)
        {
            CurrentRequestID = requestID;
            Show(instant);
        }

        protected virtual void ShowOnFinished()
        {
            OnShowFinished.Invoke();

            Callback(HGUICallbackTypes.ShowViewFinished);
        }

        /// <summary>
        /// Скрыть представление.
        /// </summary>
        public virtual void Hide(bool instant = false)
        {
            CurrentInstant = instant;
        }

        public void Hide(string requestID, bool instant = false)
        {
            CurrentRequestID = requestID;
            Hide(instant);
        }

        protected virtual void HideOnFinished()
        {
            OnHideFinished.Invoke();

            Callback(HGUICallbackTypes.HideViewFinished);
        }

        /// <summary>
        /// Сбросить представление.
        /// </summary>
        public virtual void Reset(bool visible)
        {
            this.HGSetActive(visible);

            if (visible) Callback(HGUICallbackTypes.ResetShowViewFinished);
            else Callback(HGUICallbackTypes.ResetHideViewFinished);
        }

        protected virtual void Callback(HGUICallbackTypes eventType)
        {
            HGUICallbackEvent.Trigger(CurrentRequestID, eventType);
        }

        public void OnHGEvent(HGUIRequestEvent e)
        {
            if (e.ViewName == ViewName) return;

            switch (e.EventType)
            {
                case HGUIRequestTypes.ShowView:
                    Show(e.RequestID, e.Instant);
                    break;

                case HGUIRequestTypes.HideView:
                    Hide(e.RequestID, e.Instant);
                    break;

                case HGUIRequestTypes.ResetShowView:
                    CurrentRequestID = e.RequestID;
                    Reset(true);
                    break;

                case HGUIRequestTypes.ResetHideView:
                    CurrentRequestID = e.RequestID;
                    Reset(false);
                    break;
            }
        }
    }
}