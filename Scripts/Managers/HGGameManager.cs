using System;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Базовый игровой менеджер.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGGameManager))]
    public class HGGameManager : HGSingletonMonoBehaviour<HGGameManager>, HGEventListener<HGGameEvent>
    {
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