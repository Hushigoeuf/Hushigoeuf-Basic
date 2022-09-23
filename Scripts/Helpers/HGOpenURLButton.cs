using UnityEngine;
using UnityEngine.UI;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Позволяет перейти по заданной ссылке при нажатии на кнопке.
    /// Может "подслушивать" нажатие сам или можно самому использовать метод OpenURL.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_GUI + nameof(HGOpenURLButton))]
    public class HGOpenURLButton : HGMonoBehaviour
    {
        [HGShowInSettings] [HGBorders] [HGRequired] [SerializeField]
        public string URL;

        [HGShowInSettings] [HGBorders] [SerializeField]
        public bool AutomaticAddListener = true;

        private Button _button;

        protected virtual void Awake()
        {
            if (AutomaticAddListener)
                _button = GetComponent<Button>();
        }

        protected virtual void OnEnable()
        {
            if (AutomaticAddListener)
                if (_button != null)
                    _button.onClick.AddListener(OpenURL);
        }

        protected virtual void OnDisable()
        {
            if (AutomaticAddListener)
                if (_button != null)
                    _button.onClick.RemoveListener(OpenURL);
        }

        public virtual void OpenURL()
        {
            if (!string.IsNullOrEmpty(URL))
                Application.OpenURL(URL);
        }
    }
}