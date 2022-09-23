using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Специальный менеджер, который работает с заданным прогрессом и
    /// выполняет глобальные действия в зависимости от статуса прогресса.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGProgressManager))]
    public class HGProgressManager : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        [HGShowInSettings] [HGRequired] public string ProgressID;

        [HGShowInSettings] public bool FinishLevelOnCompleted = true;

        protected bool _completed;

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        protected virtual void OnProgressCompleted()
        {
            if (_completed) return;
            _completed = true;
            if (FinishLevelOnCompleted)
                HGGameEvent.Trigger(HGGameEventTypes.FinishLevelRequest);
        }

        public void OnHGEvent(HGProgressEvent e)
        {
            if (e.ProgressID != ProgressID) return;

            switch (e.EventType)
            {
                case HGProgressEventTypes.ValueCompleted:
                    OnProgressCompleted();
                    break;
            }
        }
    }
}