using System.Collections;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf
{
    /// <summary>
    /// Выполняет отложенное отключение или уничтожение объекта.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGDelayDestruction))]
    public class HGDelayDestruction : HGMonoBehaviour
    {
#if ODIN_INSPECTOR
        [MinValue(0)]
#endif
        [HGShowInSettings]
        public float DelayBeforeDestruction;

        [HGShowInSettings] public bool DisableAfterDelayCompleted;
        [HGShowInSettings] public bool DestroyAfterDelayCompleted;

        protected virtual void OnEnable()
        {
            StartCoroutine(DelayCoroutine());
        }

        protected void OnDisable()
        {
            StopCoroutine(DelayCoroutine());
        }

        protected IEnumerator DelayCoroutine()
        {
            if (DelayBeforeDestruction > 0)
                yield return new WaitForSeconds(DelayBeforeDestruction);

            yield return null;

            if (DestroyAfterDelayCompleted)
                Destroy(gameObject);
            else if (DisableAfterDelayCompleted)
                gameObject.HGSetActive(false);
        }
    }
}