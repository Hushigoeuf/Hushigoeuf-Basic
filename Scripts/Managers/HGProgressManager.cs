using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// Специальный менеджер, который работает с заданным прогрессом и
    /// выполняет глобальные действия в зависимости от статуса прогресса.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGProgressManager))]
    public class HGProgressManager : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        /// ID целевого прогресса
        [HGShowInSettings] [HGRequired] public string ProgressID;

        /// Завершить ли уровень при достижение целевого значения
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

        /// <summary>
        /// Вызывается при достижении прогресса целевого значения.
        /// </summary>
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