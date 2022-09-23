using UnityEditor;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    public class HGEditorSeparatorMenuItem
    {
        [MenuItem(HGEditor.MENU_ITEM_PATH + "Create Separator", false, HGEditor.MENU_ITEM_PRIORITY + 0)]
        public static void MenuItem(MenuCommand command)
        {
            var name = "";
            for (var i = 0; i < 32; i++)
                name += "-";
            name += "";

            var obj = new GameObject(name);
            obj.tag = "EditorOnly";
            obj.SetActive(false);
        }
    }
}