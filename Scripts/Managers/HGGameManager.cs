using System;
using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// Базовый игровой менеджер.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGGameManager))]
    public class HGGameManager : HGSingletonMonoBehaviour<HGGameManager>, HGEventListener<HGGameEvent>
    {
        /// Была ли остановлена игра
        [NonSerialized] public bool IsPaused;

        protected override void OnEnable()
        {
            base.OnEnable();

            this.HGEventStartListening();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.HGEventStopListening();
        }

        /// <summary>
        /// Поставить игру на паузу.
        /// </summary>
        public virtual void Pause()
        {
            if (IsPaused) return;
            IsPaused = true;
            OnPaused();
        }

        protected virtual void OnPaused()
        {
            HGGameEvent.Trigger(HGGameEventTypes.Paused);

            HGUIRequestEvent.Trigger(HGUIRequestTypes.ShowPauseView);
        }

        /// <summary>
        /// Продолжить игру если она была остановлена.
        /// </summary>
        public virtual void UnPause()
        {
            if (!IsPaused) return;
            IsPaused = false;
            OnUnPaused();
        }

        protected virtual void OnUnPaused()
        {
            HGGameEvent.Trigger(HGGameEventTypes.UnPaused);

            HGUIRequestEvent.Trigger(HGUIRequestTypes.HidePauseView);
        }

        public void OnHGEvent(HGGameEvent e)
        {
            switch (e.EventType)
            {
                case HGGameEventTypes.PauseRequest:
                    Pause();
                    break;

                case HGGameEventTypes.UnPauseRequest:
                    UnPause();
                    break;
            }
        }
    }
}