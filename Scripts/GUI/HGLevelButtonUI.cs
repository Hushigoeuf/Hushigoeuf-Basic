using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// При нажатии на кнопку игра переходит на заданный уровень.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGLevelButtonUI))]
    public class HGLevelButtonUI : HGButtonUI
    {
        /// Целевой индекс уровня
        [HGShowInSettings] public int TargetIndex;

        /// Персональный список уровней если на уровне не задан глобальный
        [HGShowInBindings] public HGLevelList CustomList;

        protected override void OnTrigger()
        {
            base.OnTrigger();

            if (CustomList != null)
                CustomList.CurrentIndex = TargetIndex;
            else if (HGLevelListManager.Instance != null)
                HGLevelListManager.Instance.TargetList.CurrentIndex = TargetIndex;
        }
    }
}