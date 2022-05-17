using System;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Hushigoeuf
{
    [HGBorders]
    [Serializable]
    public class HGLevelListValue
    {
        /// Целевая сцена, которая будет подгружаться в основной сцене
#if UNITY_EDITOR && ODIN_INSPECTOR
        [OnValueChanged(nameof(EditorInitScene))]
#endif
#if UNITY_EDITOR
        [HGBorders]
        [SerializeField]
        protected SceneAsset _scene;
#endif
        /// Имя целевой сцены (задается автоматически)
        [HGBorders] [HGReadOnly] public string SceneName;

        /// Персональные настройки уровня
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

    /// <summary>
    /// Хранит список уровней для игры с соответствующей системой.
    /// </summary>
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

        /// Возвращает общее кол-во уровней в спике
        public virtual int Count => Levels.Length;

        /// Возвращает первый возможный индекс
        public virtual int FirstIndex => 0;

        /// Возвращает последний возможный индекс
        public virtual int LastIndex => Count != 0 ? Count - 1 : 0;

        /// Возвращает информацию о выбранном уровне
        public HGLevelListValue CurrentLevel => Levels[_currentIndex];

        /// Возвращает имя сцены выбранного уровня
        public virtual string CurrentSceneName => CurrentLevel.SceneName;

        /// Индекс выбранного уровня
        public virtual int CurrentIndex
        {
            get => _currentIndex;
            set => _currentIndex = Mathf.Clamp(value, FirstIndex, LastIndex);
        }
    }

    /// <summary>
    /// Базовый класс для персональных настроек уровня.
    /// </summary>
    public abstract class HGLevelSettings : HGScriptableObject
    {
    }
}