using System;
using System.Collections;
using UnityEngine.UI;

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Базовый класс, которая прослушивает события кнопки в UI и выполняет заданные действия.
    /// </summary>
    public abstract class HGButtonUI : HGMonoBehaviour
    {
        [HGShowInSettings] public bool ButtonListening = true;

        [HGShowInSettings] public bool AllowMultipleTriggers;

        [NonSerialized] public bool Interactable;

        protected virtual void OnEnable()
        {
            Interactable = true;

            if (ButtonListening)
            {
                var button = GetComponent<Button>();
                if (button != null)
                    button.onClick.AddListener(Trigger);
            }
        }

        protected virtual void OnDisable()
        {
            if (ButtonListening)
            {
                var button = GetComponent<Button>();
                if (button != null)
                    button.onClick.RemoveListener(Trigger);
            }
        }

        public virtual void Trigger()
        {
            if (!Interactable) return;
            if (!AllowMultipleTriggers)
                Interactable = false;
            StartCoroutine(TriggerCoroutine());
        }

        protected virtual void OnTrigger()
        {
        }

        protected virtual IEnumerator TriggerCoroutine()
        {
            yield return null;

            OnTrigger();
        }
    }
}