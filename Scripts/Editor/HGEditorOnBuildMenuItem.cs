using UnityEditor;
using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// Находит компоненты с IHGEditorOnBuild, чтобы вызвать метод OnHGEditorBuild.
    /// </summary>
    public class HGEditorOnBuildMenuItem
    {
        [MenuItem(HGEditor.MENU_ITEM_PATH + nameof(IHGEditorOnBuild.OnHGEditorBuild), false,
            HGEditor.MENU_ITEM_PRIORITY + 1)]
        public static void MenuItem(MenuCommand command)
        {
            foreach (var t in Object.FindObjectsOfType<MonoBehaviour>())
                if (t is IHGEditorOnBuild builder)
                    builder.OnHGEditorBuild();
        }
    }
}