using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.Utilities.Editor;
#endif

namespace Hushigoeuf
{
    /// <summary>
    /// Класс-оболочка для MonoBehaviour в Unity.
    /// </summary>
    public abstract class HGMonoBehaviour : MonoBehaviour
    {
        protected const int EDITOR_MIN_ORDER = int.MinValue;
        protected const int EDITOR_MAX_ORDER = int.MaxValue;

#if UNITY_EDITOR && ODIN_INSPECTOR
        [PropertyOrder(EDITOR_MIN_ORDER)]
        [OnInspectorGUI]
        private void EditorOnBeginGUI()
        {
            SirenixEditorGUI.BeginBox();
            SirenixEditorGUI.MessageBox("Instance-ID: " + GetInstanceID());
        }

        [PropertyOrder(EDITOR_MAX_ORDER)]
        [OnInspectorGUI]
        private void EditorOnEndGUI()
        {
            SirenixEditorGUI.EndBox();
        }
#endif
    }

    /// <summary>
    /// Класс-оболочка (с Transform) для MonoBehaviour в Unity.
    /// </summary>
    public abstract class HGBaseMonoBehaviour : HGMonoBehaviour
    {
        private Transform _transform;

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                    _transform = transform;
                return _transform;
            }
        }
    }

    /// <summary>
    /// Расширенный класс-оболочка для MonoBehaviour в Unity.
    /// </summary>
    public abstract class HGFullyMonoBehaviour : HGBaseMonoBehaviour
    {
        protected virtual void Reset()
        {
        }

        protected virtual void Awake()
        {
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnDestroy()
        {
        }
    }
}