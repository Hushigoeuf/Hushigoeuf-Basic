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
    /// Класс-оболочка для ScriptableObject в Unity.
    /// </summary>
    public abstract class HGScriptableObject : ScriptableObject
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
}