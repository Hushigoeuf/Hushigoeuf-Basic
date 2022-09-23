using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hushigoeuf.Basic
{
    [HGBorders]
    [Serializable]
    public class HGLevelListValue
    {
#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnValueChanged(nameof(EditorInitScene))]
#endif
#if UNITY_EDITOR
        [HGBorders]
        [SerializeField]
        protected SceneAsset _scene;
#endif

        [HGBorders] [HGReadOnly] public string SceneName;

        [HGBorders] public HGLevelSettings Settings;

#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnInspectorInit]
        protected virtual void EditorInitScene()
        {
            SceneName = string.Empty;
            if (_scene != null)
                SceneName = _scene.name;
        }
#endif
    }

    [CreateAssetMenu(menuName = HGEditor.PATH_MENU_COMMON + nameof(HGLevelList))]
    public class HGLevelList : HGScriptableObject
    {
        [HGShowInBindings] [HGListDrawerSettings] [SerializeField]
        public HGLevelListValue[] Levels = new HGLevelListValue[0];

#if ODIN_INSPECTOR
        [MinValue(nameof(FirstIndex))]
        [MaxValue(nameof(LastIndex))]
#endif
        [HGShowInDebug]
        [HGBorders]
        [SerializeField]
        private int _currentIndex;

        public virtual int Count => Levels.Length;
        public virtual int FirstIndex => 0;
        public virtual int LastIndex => Count != 0 ? Count - 1 : 0;
        public HGLevelListValue CurrentLevel => Levels[_currentIndex];
        public virtual string CurrentSceneName => CurrentLevel.SceneName;

        public virtual int CurrentIndex
        {
            get => _currentIndex;
            set => _currentIndex = Mathf.Clamp(value, FirstIndex, LastIndex);
        }
    }

    public abstract class HGLevelSettings : HGScriptableObject
    {
    }
}