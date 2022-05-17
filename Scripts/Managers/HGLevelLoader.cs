using System;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hushigoeuf
{
    public enum HGLevelLoaderEventTypes
    {
        LevelLoaded,
        BackScene,
        NextScene,
        RestartScene,
        FadeAndBackScene,
        FadeAndNextScene,
        FadeAndRestartScene,
    }

    public struct HGLevelLoaderEvent
    {
        public HGLevelLoaderEventTypes EventType;

        public HGLevelLoaderEvent(HGLevelLoaderEventTypes eventType)
        {
            EventType = eventType;
        }

        public void Trigger()
        {
            HGEventManager.TriggerEvent(this);
        }

        private static HGLevelLoaderEvent e;

        public static void Trigger(HGLevelLoaderEventTypes eventType)
        {
            e.EventType = eventType;

            e.Trigger();
        }
    }

    /// <summary>
    /// Этот менеджер занимается исключительно переходом между сценами и подгрузкой дочерних сцен.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGLevelLoader))]
    public class HGLevelLoader : HGSingletonMonoBehaviour<HGLevelLoader>,
        HGEventListener<HGLevelLoaderEvent>, HGEventListener<HGUICallbackEvent>
    {
        /// Целевая сцена на которую можно вернуться в случае соответствующего запроса
#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnValueChanged(nameof(EditorOnInitScenes))]
#endif
#if UNITY_EDITOR
        [HGShowInSettings]
        public SceneAsset TargetBackScene;
#endif

        /// Целевая сцена на которую можно перейти в случае соответствующего запроса
#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnValueChanged(nameof(EditorOnInitScenes))]
#endif
#if UNITY_EDITOR
        [HGShowInSettings]
        public SceneAsset TargetNextScene;
#endif

        /// Отключить асинхронный режим загрузки дочерней сцены
        [HGShowInSettings] public bool Instant;

        /// Целевой список уровней (можно использовать HGLevelListManager и не трогать это)
        [HGShowInBindings] public HGLevelList TargetList;

        [HGShowInDebug] [HGReadOnly] public string TargetBackSceneName;
        [HGShowInDebug] [HGReadOnly] public string TargetNextSceneName;
        [NonSerialized] public bool IsLevelLoaded;

        protected int _sceneBuildIndex;
        protected string _currentRequestID;
        protected string _currentSceneName;
        protected HGLevelLoaderEventTypes _currentEventOnFadeFinished;

        public virtual HGLevelList CurrentList
        {
            get
            {
                if (TargetList == null)
                    if (HGLevelListManager.Instance != null)
                        return HGLevelListManager.Instance.TargetList;
                return TargetList;
            }
        }

        public virtual HGLevelSettings CurrentSettings
        {
            get
            {
                if (CurrentList != null) return CurrentList.CurrentLevel.Settings;

                return null;
            }
        }

        protected override void Start()
        {
            base.Start();

            if (CurrentList != null)
            {
                var scene = SceneManager.GetSceneByName(CurrentList.CurrentSceneName);
                if (!scene.isLoaded)
                {
                    _sceneBuildIndex = scene.buildIndex;
                    SceneManager.sceneLoaded += LevelOnLoaded;
                    SceneManager.LoadSceneAsync(CurrentList.CurrentSceneName, LoadSceneMode.Additive);
                    return;
                }
            }

            LevelOnLoaded();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            this.HGEventStartListening<HGLevelLoaderEvent>();
            this.HGEventStartListening<HGUICallbackEvent>();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            this.HGEventStopListening<HGLevelLoaderEvent>();
            this.HGEventStopListening<HGUICallbackEvent>();
        }

        protected virtual void LevelOnLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != _sceneBuildIndex) return;

            SceneManager.sceneLoaded -= LevelOnLoaded;

            LevelOnLoaded();
        }

        protected virtual void LevelOnLoaded()
        {
            IsLevelLoaded = true;

            HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.LevelLoaded);
        }

        public virtual void LoadScene(string sceneName)
        {
            if (Instant)
                SceneManager.LoadScene(sceneName);
            else SceneManager.LoadSceneAsync(sceneName);
        }

        public virtual void FadeAndLoadScene(string sceneName)
        {
            _currentSceneName = sceneName;
            _currentRequestID = HGUIRequestEvent.Trigger(HGUIRequestTypes.Fade);
        }

        public void OnHGEvent(HGLevelLoaderEvent e)
        {
            switch (e.EventType)
            {
                case HGLevelLoaderEventTypes.BackScene:
                    LoadScene(TargetBackSceneName);
                    break;

                case HGLevelLoaderEventTypes.NextScene:
                    if (CurrentList != null)
                        if (CurrentList.CurrentIndex < CurrentList.Count - 1)
                        {
                            CurrentList.CurrentIndex--;
                            LoadScene(CurrentList.CurrentSceneName);
                            break;
                        }

                    LoadScene(TargetNextSceneName);
                    break;

                case HGLevelLoaderEventTypes.RestartScene:
                    LoadScene(gameObject.scene.name);
                    break;

                case HGLevelLoaderEventTypes.FadeAndBackScene:
                    _currentEventOnFadeFinished = HGLevelLoaderEventTypes.BackScene;
                    FadeAndLoadScene(null);
                    break;

                case HGLevelLoaderEventTypes.FadeAndNextScene:
                    _currentEventOnFadeFinished = HGLevelLoaderEventTypes.NextScene;
                    FadeAndLoadScene(null);
                    break;

                case HGLevelLoaderEventTypes.FadeAndRestartScene:
                    _currentEventOnFadeFinished = HGLevelLoaderEventTypes.RestartScene;
                    FadeAndLoadScene(null);
                    break;
            }
        }

        public void OnHGEvent(HGUICallbackEvent e)
        {
            if (string.IsNullOrEmpty(_currentRequestID)) return;
            if (_currentRequestID != e.RequestID) return;

            if (e.EventType == HGUICallbackTypes.FadeFinished)
            {
                if (!string.IsNullOrEmpty(_currentSceneName))
                    LoadScene(_currentSceneName);
                else HGLevelLoaderEvent.Trigger(_currentEventOnFadeFinished);
            }
        }

#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnInspectorInit]
        protected virtual void EditorOnInitScenes()
        {
            TargetBackSceneName = string.Empty;
            TargetNextSceneName = string.Empty;
            if (TargetBackScene != null)
                TargetBackSceneName = TargetBackScene.name;
            if (TargetNextScene != null)
                TargetNextSceneName = TargetNextScene.name;
        }
#endif
    }
}