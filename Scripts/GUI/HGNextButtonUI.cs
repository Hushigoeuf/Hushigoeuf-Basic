using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// При нажатии на кнопку переходит на следующую сцену.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGNextButtonUI))]
    public class HGNextButtonUI : HGButtonUI
    {
        /// Использовать ли эффект затемнения
        [HGShowInSettings] public bool UseFadeEffect = true;

        protected override void OnTrigger()
        {
            base.OnTrigger();

            if (UseFadeEffect)
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.FadeAndNextScene);
            else
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.NextScene);
        }
    }
}