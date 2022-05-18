using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Hushigoeuf
{
    public enum HGUIRequestTypes
    {
        Fade,
        UnFade,
        Flash,
        ShowView,
        HideView,
        ResetShowView,
        ResetHideView,
        ShowHUDView,
        HideHUDView,
        ShowPauseView,
        HidePauseView,
        ShowGameOverView,
        HideGameOverView,
        ShowFinishView,
        HideFinishView,
    }

    public enum HGUICallbackTypes
    {
        FadeFinished,
        UnFadeFinished,
        FlashCompleted,
        ShowViewFinished,
        HideViewFinished,
        ResetShowViewFinished,
        ResetHideViewFinished,
    }

    /// <summary>
    /// Событие, которое обрабатывает как запрос к менеджеру для выполнения действий с интерфейсом.
    /// </summary>
    public struct HGUIRequestEvent
    {
        public string RequestID;
        public HGUIRequestTypes EventType;
        public string ViewName;
        public bool Instant;

        public HGUIRequestEvent(HGUIRequestTypes eventType, string viewName = null, bool instant = false)
        {
            RequestID = GenerateRequestID();
            EventType = eventType;
            ViewName = viewName;
            Instant = instant;
        }

        public string Trigger()
        {
            HGEventManager.TriggerEvent(e);

            return e.RequestID;
        }

        private static HGUIRequestEvent e;
        private static int CurrentRequestID = int.MinValue;

        public static string Trigger(HGUIRequestTypes eventType, string viewName = null, bool instant = false)
        {
            e.RequestID = GenerateRequestID();
            e.EventType = eventType;
            e.ViewName = viewName;
            e.Instant = instant;

            return e.Trigger();
        }

        private static string GenerateRequestID()
        {
            if (CurrentRequestID < int.MaxValue - 1)
                CurrentRequestID++;
            else CurrentRequestID = int.MinValue;
            return CurrentRequestID.ToString();
        }
    }

    /// <summary>
    /// Событие, которое срабатывает как обратная связь за действия с интерфейсом.
    /// </summary>
    public struct HGUICallbackEvent
    {
        public string RequestID;
        public HGUICallbackTypes EventType;

        public HGUICallbackEvent(string requestID, HGUICallbackTypes eventType)
        {
            RequestID = requestID;
            EventType = eventType;
        }

        public string Trigger()
        {
            HGEventManager.TriggerEvent(e);

            return e.RequestID;
        }

        private static HGUICallbackEvent e;
        private static int CurrentRequestID = int.MinValue;

        public static void Trigger(string requestID, HGUICallbackTypes eventType)
        {
            e.RequestID = requestID;
            e.EventType = eventType;

            e.Trigger();
        }
    }

    /// <summary>
    /// Параметры кастомного представления.
    /// </summary>
    [HGBorders]
    [Serializable]
    public class HGUIManagerTarget
    {
        /// Показать на старте сцены
        public bool ShowOnStart;

        /// Воспроизвести после исчезновения затемнения
        public bool ShowOnStartUnFade;

        [HGRequired] public HGSimpleUIView View;
    }

    /// <summary>
    /// Достаточно гибкий менеджер интерфейса, который отслеживает общие события,
    /// но способен работать и с новыми решениями.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGSimpleUIManager))]
    public class HGSimpleUIManager : HGMonoBehaviour, HGEventListener<HGUIRequestEvent>, HGEventListener<HGGameEvent>
    {
        /// Снять ли эффект затемнения при загрузке сцены
        [HGShowInSettings] public bool UnFadeOnEnter = true;

        /// Снять ли эффект затемнения при загрузке уровня
        [HGShowInSettings] public bool UnFadeOnLevelLoaded;

        /// Воспроизвести базовый интерфейс после исчезновения затемнения
        [HGShowInSettings] public bool HUDOnUnFade = true;

        [HGShowInBindings] public HGSimpleUIView FaderView;
        [HGShowInBindings] public HGSimpleUIView FlashView;
        [HGShowInBindings] public HGSimpleUIView HUDView;
        [HGShowInBindings] public HGSimpleUIView PauseView;
        [HGShowInBindings] public HGSimpleUIView GameOverView;
        [HGShowInBindings] public HGSimpleUIView FinishView;

        /// Список представлений, которые выходят за рамки базовых решений
        [HGShowInBindings] [HGListDrawerSettings]
        public HGUIManagerTarget[] OtherTargets = new HGUIManagerTarget[0];

        [HGShowInEvents] public UnityEvent OnFadeFinished;
        [HGShowInEvents] public UnityEvent OnUnfadeFinished;
        [HGShowInEvents] public UnityEvent OnFlash;

        protected bool _initialized;
        protected bool _blocked;
        protected int _currentSceneBuildIndex;
        protected string _currentSceneName;
        protected List<HGUIManagerTarget> _targets;
        protected bool _unFadeOnStartCompleted;
        protected bool _showOnUnFadeCompleted;

        protected virtual void Awake()
        {
            _targets = new List<HGUIManagerTarget>();

            if (HUDView != null)
                _targets.Add(
                    new HGUIManagerTarget
                        {ShowOnStart = !HUDOnUnFade, ShowOnStartUnFade = HUDOnUnFade, View = HUDView});
            if (PauseView != null)
                _targets.Add(new HGUIManagerTarget {View = PauseView});
            if (GameOverView != null)
                _targets.Add(new HGUIManagerTarget {View = GameOverView});
            if (FinishView != null)
                _targets.Add(new HGUIManagerTarget {View = FinishView});

            _targets.AddRange(OtherTargets);
        }

        protected virtual void Start()
        {
            Initialization();

            if (!_unFadeOnStartCompleted)
                if (UnFadeOnEnter)
                {
                    _unFadeOnStartCompleted = true;
                    UnFade();
                }
        }

        protected virtual void Initialization()
        {
            if (_initialized) return;

            if (FaderView != null)
                FaderView.Reset(UnFadeOnEnter || UnFadeOnLevelLoaded);
            if (FlashView != null)
                FlashView.Reset(false);

            for (var i = 0; i < _targets.Count; i++)
                _targets[i].View.Reset(_targets[i].ShowOnStart);

            _initialized = true;
            _unFadeOnStartCompleted = !UnFadeOnEnter && !UnFadeOnLevelLoaded;
            _showOnUnFadeCompleted = _unFadeOnStartCompleted;
        }

        protected virtual void OnEnable()
        {
            this.HGEventStartListening<HGUIRequestEvent>();
            this.HGEventStartListening<HGGameEvent>();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening<HGUIRequestEvent>();
            this.HGEventStopListening<HGGameEvent>();
        }

        /// <summary>
        /// Воспроизвести эффект затемнения.
        /// </summary>
        public virtual void Fade(string requestID, bool instant = false)
        {
            if (FaderView == null) return;

            ResetFader(false);

            FaderView.OnShowFinished.AddListener(FadeOnFinished);
            FaderView.Show(requestID, instant);
        }

        public void Fade(bool instant = false)
        {
            Fade(null, instant);
        }

        /// <summary>
        /// Вызывается когда эффект затемнения воспроизведен.
        /// </summary>
        protected virtual void FadeOnFinished()
        {
            FaderView.OnShowFinished.RemoveListener(FadeOnFinished);

            OnFadeFinished.Invoke();

            HGUICallbackEvent.Trigger(FaderView.CurrentRequestID, HGUICallbackTypes.FadeFinished);
        }

        /// <summary>
        /// Сбрасывает параметры фейдера.
        /// </summary>
        protected virtual void ResetFader(bool visible)
        {
            FaderView.OnShowFinished.RemoveListener(FadeOnFinished);
            FaderView.OnHideFinished.RemoveListener(UnFadeOnFinished);
            FaderView.Reset(visible);
        }

        /// <summary>
        /// Остановить эффект затемнения.
        /// </summary>
        public virtual void UnFade(string requestID, bool instant = false)
        {
            if (FaderView == null) return;

            ResetFader(true);

            FaderView.OnHideFinished.AddListener(UnFadeOnFinished);

            FaderView.Hide(requestID, instant);
        }

        public void UnFade(bool instant = false)
        {
            UnFade(null, instant);
        }

        /// <summary>
        /// Вызывается когда эффект затемнения остановлен.
        /// </summary>
        protected virtual void UnFadeOnFinished()
        {
            FaderView.OnHideFinished.RemoveListener(UnFadeOnFinished);

            if (!_showOnUnFadeCompleted)
            {
                _showOnUnFadeCompleted = true;
                for (var i = 0; i < _targets.Count; i++)
                    if (!_targets[i].ShowOnStart)
                        if (_targets[i].ShowOnStartUnFade)
                            _targets[i].View.Show();
            }

            OnUnfadeFinished.Invoke();

            HGUICallbackEvent.Trigger(FaderView.CurrentRequestID, HGUICallbackTypes.UnFadeFinished);
        }

        /// <summary>
        /// Выполнить вспышку (быстрое затемнение и возврат).
        /// </summary>
        public virtual void Flash(string requestID = null)
        {
            if (FlashView == null) return;

            FlashView.OnShowFinished.RemoveListener(FlashOnShowFinished);
            FlashView.OnHideFinished.RemoveListener(FlashOnHideFinished);

            FlashView.Reset(false);
            FlashView.OnShowFinished.AddListener(FlashOnShowFinished);
            FlashView.Show(requestID);
        }

        protected virtual void FlashOnShowFinished()
        {
            FlashView.OnShowFinished.RemoveListener(FlashOnShowFinished);
            FlashView.OnHideFinished.AddListener(FlashOnHideFinished);
            FlashView.Hide();

            OnFlash.Invoke();

            HGUICallbackEvent.Trigger(FlashView.CurrentRequestID, HGUICallbackTypes.FlashCompleted);
        }

        protected virtual void FlashOnHideFinished()
        {
            FlashView.OnHideFinished.RemoveListener(FlashOnHideFinished);
            FlashView.Reset(false);
        }

        /// <summary>
        /// Найти представление по имени (сюда же входят базовые представления вроде фейдера).
        /// </summary>
        public virtual HGSimpleUIView GetView(string viewName)
        {
            for (var i = 0; i < _targets.Count; i++)
                if (_targets[i].View.ViewName == viewName)
                    return _targets[i].View;
            return null;
        }
        
        /// <summary>
        /// Показать заданное представление.
        /// </summary>
        public virtual void ShowView(string viewName, string requestID, bool instant = false)
        {
            var view = GetView(viewName);
            if (view == null) return;
            view.Show(requestID, instant);
        }

        public void ShowView(string viewName, bool instant = false)
        {
            ShowView(viewName, null, instant);
        }
        
        /// <summary>
        /// Скрыть заданное представление.
        /// </summary>
        public virtual void HideView(string viewName, string requestID, bool instant = false)
        {
            var view = GetView(viewName);
            if (view == null) return;
            view.Hide(requestID, instant);
        }

        public void HideView(string viewName, bool instant = false)
        {
            HideView(viewName, null, instant);
        }

        public void OnHGEvent(HGUIRequestEvent e)
        {
            switch (e.EventType)
            {
                case HGUIRequestTypes.Fade:
                    Fade(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.UnFade:
                    UnFade(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.Flash:
                    Flash(e.RequestID);
                    break;
                case HGUIRequestTypes.ShowView:
                    ShowView(e.ViewName, e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.HideView:
                    HideView(e.ViewName, e.RequestID, e.Instant);
                    break;

                case HGUIRequestTypes.ShowHUDView:
                    if (HUDView != null)
                        HUDView.Show(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.HideHUDView:
                    if (HUDView != null)
                        HUDView.Hide(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.ShowPauseView:
                    if (PauseView != null)
                        PauseView.Show(e.RequestID, e.Instant);
                    return;
                case HGUIRequestTypes.HidePauseView:
                    if (PauseView != null)
                        PauseView.Hide(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.ShowGameOverView:
                    if (GameOverView != null)
                        GameOverView.Show(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.HideGameOverView:
                    if (GameOverView != null)
                        GameOverView.Hide(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.ShowFinishView:
                    if (FinishView != null)
                        FinishView.Show(e.RequestID, e.Instant);
                    break;
                case HGUIRequestTypes.HideFinishView:
                    if (FinishView != null)
                        FinishView.Hide(e.RequestID, e.Instant);
                    break;
            }
        }

        public void OnHGEvent(HGGameEvent e)
        {
            switch (e.EventType)
            {
                case HGGameEventTypes.LevelLoaded:
                    if (_initialized)
                        if (!_unFadeOnStartCompleted)
                            if (UnFadeOnLevelLoaded)
                            {
                                _unFadeOnStartCompleted = true;
                                UnFade();
                            }

                    break;
            }
        }
    }
}