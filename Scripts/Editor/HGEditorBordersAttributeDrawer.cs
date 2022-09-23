#if ODIN_INSPECTOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Hushigoeuf.Basic
{
    [DrawerPriority(1)]
    public class HGEditorBordersAttributeDrawer : OdinAttributeDrawer<HGBordersAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            SirenixEditorGUI.BeginBox();
            CallNextDrawer(label);
            SirenixEditorGUI.EndBox();
        }
    }
}
#endif