using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// При нажатии на кнопку снимает паузу с игры.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGUnPauseButtonUI))]
    public class HGUnPauseButtonUI : HGButtonUI
    {
        protected override void OnTrigger()
        {
            base.OnTrigger();

            HGGameEvent.Trigger(HGGameEventTypes.UnPauseRequest);
        }
    }
}