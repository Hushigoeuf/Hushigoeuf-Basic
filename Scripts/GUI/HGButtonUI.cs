using System;
using System.Collections;
using UnityEngine.UI;

namespace Hushigoeuf
{
    /// <summary>
    /// Базовый класс, которая прослушивает события кнопки в UI и выполняет заданные действия.
    /// </summary>
    public abstract class HGButtonUI : HGMonoBehaviour
    {
        /// Автоматически прослушивать события кнопки
        [HGShowInSettings] public bool ButtonListening = true;

        /// Разрешить вызывать триггер несколько раз
        [HGShowInSettings] public bool AllowMultipleTriggers;

        /// Разрешить ли вызывать триггер
        [NonSerialized] public bool Interactable;

        protected virtual void OnEnable()
        {
            Interactable = true;

            // Автоматически добавляем прослушиватель для кнопки
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

        /// <summary>
        /// Попробывать вызвать триггер.
        /// </summary>
        public virtual void Trigger()
        {
            if (!Interactable) return;
            if (!AllowMultipleTriggers)
                Interactable = false;
            StartCoroutine(TriggerCoroutine());
        }

        /// <summary>
        /// Здесь выполняются все действия после вызова триггера когда пройдены все условия.
        /// </summary>
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