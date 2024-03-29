﻿using UnityEngine;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// При нажатии на кнопку игра возвращается на предыдущую сцену.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGBackButtonUI))]
    public class HGBackButtonUI : HGButtonUI
    {
        [HGShowInSettings] public bool UseFadeEffect = true;

        protected override void OnTrigger()
        {
            base.OnTrigger();

            if (UseFadeEffect)
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.FadeAndBackScene);
            else
                HGLevelLoaderEvent.Trigger(HGLevelLoaderEventTypes.BackScene);
        }
    }
}