namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Базовый класс, который следит за статусом загрузки уровня.
    /// Если сцена еще подгружает дочерние сцены, то он продолжит следить за статусом.
    /// </summary>
    public abstract class HGLevelListener : HGMonoBehaviour, HGEventListener<HGLevelLoaderEvent>
    {
        protected bool IsLoaded;

        protected virtual void Start()
        {
            Initialization();
            var settings = HGLevelLoader.Instance.CurrentSettings;
            if (settings != null)
                Initialization(settings);

            if (HGLevelLoader.Instance != null)
                if (HGLevelLoader.Instance.IsLevelLoaded)
                    if (!IsLoaded)
                    {
                        IsLoaded = true;
                        OnLoaded();
                    }
        }

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        /// <summary>
        /// Этот метод вызывает только на старте.
        /// </summary>
        protected virtual void Initialization()
        {
        }

        /// <summary>
        /// Этот метод вызывается только на старте и если имеются персональные настройки уровня.
        /// </summary>
        protected virtual void Initialization(HGLevelSettings settings)
        {
        }

        /// <summary>
        /// Этот метод вызывается в любом случае, но после того как все сцены были загружены.
        /// </summary>
        protected virtual void OnLoaded()
        {
        }

        public virtual void OnHGEvent(HGLevelLoaderEvent e)
        {
            switch (e.EventType)
            {
                case HGLevelLoaderEventTypes.LevelLoaded:
                    if (!IsLoaded)
                    {
                        IsLoaded = true;
                        OnLoaded();
                    }

                    break;
            }
        }
    }
}