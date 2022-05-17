using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// При нажатии на кнопку выполняет рестарт уровня.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGRestartButtonUI))]
    public class HGRestartButtonUI : HGButtonUI
    {
        /// Использоваться ли эффект затемнения.
        [HGShowInSettings] public bool UseFadeEffect = true;

        protected override void OnTrigger()
        {
            base.OnTrigger();

            if (UseFadeEffect)
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.FadeAndRestartScene);
            else
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.RestartScene);
        }
    }
}