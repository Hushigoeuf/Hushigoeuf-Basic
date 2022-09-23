using UnityEngine;
using UnityEngine.UI;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Сбрасывает ползунок ScrollRect при активации объекта.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGScrollRectResetFilter))]
    [RequireComponent(typeof(ScrollRect))]
    public sealed class HGScrollRectResetFilter : MonoBehaviour
    {
        private void OnEnable()
        {
            var target = GetComponent<ScrollRect>();

            target.content.anchoredPosition = new Vector2(target.content.anchoredPosition.x, 0);
        }
    }
}