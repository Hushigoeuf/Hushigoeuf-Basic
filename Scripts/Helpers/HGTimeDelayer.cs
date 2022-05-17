using System.Collections;
using UnityEngine;
using UnityEngine.Events;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
    /// <summary>
    /// Выполняет действия с задержкой перед выполнением.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGTimeDelayer))]
    public class HGTimeDelayer : MonoBehaviour
    {
        /// Размер задержки перед выполнением
#if ODIN_INSPECTOR
        [MinValue(0)]
#endif
        [HGShowInSettings]
        [HGBorders]
        [SerializeField]
        public float DelayBeforeExecute;

        /// Запустить ли отчет после старта
        [HGShowInSettings] [HGBorders] public bool AutoExecuteOnStart;

        /// Запустить ли отчет после активации объекта
        [HGShowInSettings] [HGBorders] public bool AutoExecuteOnEnable;

        [HGShowInEvents] public UnityEvent OnExecuteEvent = new UnityEvent();

        protected virtual void Start()
        {
            if (AutoExecuteOnStart) Execute(false);
        }

        protected virtual void OnEnable()
        {
            if (AutoExecuteOnEnable) Execute(false);
        }

        protected IEnumerator ExecuteCoroutine()
        {
            if (DelayBeforeExecute > 0)
                yield return new WaitForSeconds(DelayBeforeExecute);

            Execute(true);
        }

        public virtual void Execute(bool instant = false)
        {
            if (!instant)
            {
                StartCoroutine(ExecuteCoroutine());
                return;
            }

            OnExecute();
        }

        protected virtual void OnExecute()
        {
            OnExecuteEvent.Invoke();
        }
    }
}