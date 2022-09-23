using System;
using System.Collections;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Базовый менеджер, который следит за уровнем, делает рестарт,
    /// завершает уровень, взаимодействует с игровым интерфейсом.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGLevelManager))]
    public class HGLevelManager : HGSingletonMonoBehaviour<HGLevelManager>,
        HGEventListener<HGGameEvent>, HGEventListener<HGLevelLoaderEvent>
    {
        /// Объявить старт уровня вне этого менеджера
        [HGShowInSettings] [HGBorders] public bool StartLevelFromOther;

#if ODIN_INSPECTOR
        [MinValue(0)]
        [DisableIf(nameof(StartLevelFromOther))]
#endif
        [HGShowInSettings]
        [HGBorders]
        public float DelayBeforeStarted;

        /// Выполнить рестарт уровня после проигрыша вместо вывода интерфейса
        [HGShowInSettings] [HGBorders] public bool RestartInsteadGameOver;

        /// Сразу перейти к загрузке следующего уровня вместо вывода интерфейса
        [HGShowInSettings] [HGBorders] public bool NextInsteadFinishScreen;

        /// В качестве следующего уровня выбрать параметры загрузчика, а не брать из общего списка уровней
        [HGShowInSettings] [HGBorders] public bool NextInsteadList;

        [HGShowInSettings] [HGBorders] public bool GameOverOnPlayerDeath;

#if ODIN_INSPECTOR
        [MinValue(0)]
#endif
        [HGShowInSettings]
        [HGBorders]
        public float DelayBeforeGameOver;

#if ODIN_INSPECTOR
        [MinValue(0)]
#endif
        [HGShowInSettings]
        [HGBorders]
        public float DelayBeforeFinished;

        [NonSerialized] public bool IsLoaded;
        [NonSerialized] public bool IsStarted;
        [NonSerialized] public bool IsFailed;
        [NonSerialized] public bool IsFinished;

        protected override void Start()
        {
            base.Start();

            if (HGLevelLoader.Instance == null ||
                HGLevelLoader.Instance.IsLevelLoaded)
                if (!StartLevelFromOther)
                    OnLevelLoaded();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.HGEventStartListening<HGGameEvent>();
            this.HGEventStartListening<HGLevelLoaderEvent>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.HGEventStopListening<HGGameEvent>();
            this.HGEventStopListening<HGLevelLoaderEvent>();
        }

        private IEnumerator DelayBeforeStartCoroutine()
        {
            yield return new WaitForSeconds(DelayBeforeStarted);

            OnLevelStarted();
        }

        /// <summary>
        /// Вызывает когда уровень точно загружен вместе со всеми дочерними сценами если они есть.
        /// </summary>
        protected virtual void OnLevelLoaded()
        {
            if (IsLoaded) return;
            IsLoaded = true;

            HGGameEvent.Trigger(HGGameEventTypes.LevelLoaded);

            if (DelayBeforeStarted > 0)
                StartCoroutine(DelayBeforeStartCoroutine());
            else OnLevelStarted();
        }

        /// <summary>
        /// Выполняет старт уровня.
        /// Можно применять если требуется сделать старт не сразу, а к примеру,
        /// предложить игроку самому нажать на кнопку, чтобы начать игру.
        /// </summary>
        public virtual void StartLevel()
        {
            if (IsStarted) return;
            IsStarted = true;
            OnLevelStarted();
        }

        protected virtual void OnLevelStarted()
        {
            HGGameEvent.Trigger(HGGameEventTypes.LevelStarted);
        }

        /// <summary>
        /// Выполняет действия проигрыша.
        /// Может вызываться, к примеру, при смерти основного персонажа.
        /// </summary>
        public virtual void GameOver()
        {
            if (IsFailed) return;
            IsFailed = true;

            if (DelayBeforeGameOver > 0)
                StartCoroutine(GameOverCoroutine());
            else OnGameOver();
        }

        private IEnumerator GameOverCoroutine()
        {
            yield return new WaitForSeconds(DelayBeforeGameOver);

            OnGameOver();
        }

        protected virtual void OnGameOver()
        {
            HGGameEvent.Trigger(HGGameEventTypes.GameOver);

            if (RestartInsteadGameOver)
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.FadeAndRestartScene);
            else
                HGUIRequestEvent.Trigger(HGUIRequestTypes.ShowPauseView);
        }

        /// <summary>
        /// Выполняет действия для завершения уровня.
        /// Может применятся после выполнения главное задачи на уровне.
        /// </summary>
        public virtual void FinishLevel()
        {
            if (IsFinished) return;
            IsFinished = true;

            if (DelayBeforeFinished > 0)
                StartCoroutine(FinishCoroutine());
            else OnLevelFinished();
        }

        private IEnumerator FinishCoroutine()
        {
            yield return new WaitForSeconds(DelayBeforeFinished);

            OnLevelFinished();
        }

        protected virtual void OnLevelFinished()
        {
            HGGameEvent.Trigger(HGGameEventTypes.LevelFinished);

            if (NextInsteadFinishScreen)
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.FadeAndNextScene);
            else
                HGUIRequestEvent.Trigger(HGUIRequestTypes.ShowFinishView);
        }

        public void OnHGEvent(HGGameEvent e)
        {
            switch (e.EventType)
            {
                case HGGameEventTypes.StartLevelRequest:
                    StartLevel();
                    break;

                case HGGameEventTypes.GameOverRequest:
                    GameOver();
                    break;

                case HGGameEventTypes.FinishLevelRequest:
                    FinishLevel();
                    break;

                case HGGameEventTypes.PlayerDeath:
                    if (GameOverOnPlayerDeath)
                        HGGameEvent.Trigger(HGGameEventTypes.GameOverRequest);
                    break;
            }
        }

        public void OnHGEvent(HGLevelLoaderEvent e)
        {
            if (e.EventType == HGLevelLoaderEventTypes.LevelLoaded)
                if (!StartLevelFromOther)
                    OnLevelLoaded();
        }
    }
}