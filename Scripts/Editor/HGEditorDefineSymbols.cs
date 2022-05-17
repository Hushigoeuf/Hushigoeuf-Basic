using System.Linq;
using UnityEditor;

namespace Hushigoeuf
{
    [InitializeOnLoad]
    public class HGEditorDefineSymbols
    {
        public static readonly string[] Symbols = new string[]
        {
            "HG_BASIC",
        };

        static HGEditorDefineSymbols()
        {
            var scriptingDefinesString =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var scriptingDefinesStringList = scriptingDefinesString.Split(';').ToList();
            scriptingDefinesStringList.AddRange(Symbols.Except(scriptingDefinesStringList));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", scriptingDefinesStringList.ToArray()));
        }
    }
}