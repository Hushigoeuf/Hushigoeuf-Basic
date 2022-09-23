using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Этот менеджер просто хранит целевой список уровней.
    /// Может использоваться другими менеджерами, которые работают с этим списком.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGLevelListManager))]
    public class HGLevelListManager : HGSingletonMonoBehaviour<HGLevelListManager>
    {
        [HGShowInBindings] public HGLevelList TargetList;
    }
}