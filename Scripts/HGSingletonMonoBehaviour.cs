using UnityEngine;

namespace Hushigoeuf
{
    /// <summary>
    /// Класс-синглетон для MonoBehaviour в Unity.
    /// </summary>
    public abstract class HGSingletonMonoBehaviour<T> : HGFullyMonoBehaviour where T : Component
    {
        protected static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying) return;

            if (_instance != null)
            {
                Destroy(this);
                return;
            }

            _instance = this as T;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (this == _instance)
                _instance = null;
        }
    }
}