namespace Hushigoeuf
{
    /// <summary>
    /// Базовый класс для работы с заданным прогрессом.
    /// Его можно использоваться, к примеру, для визуализации прогресса.
    /// </summary>
    public abstract class HGProgressHandler : HGMonoBehaviour, HGEventListener<HGProgressEvent>
    {
        /// ID прогресс с которым будет работать обработчик
        [HGShowInSettings] [HGRequired] public string ProgressID;

        /// <summary>
        /// Отправляет запрос для получения результатов по заданному прогрессу.
        /// </summary>
        protected virtual void Start()
        {
            HGProgressEvent.Trigger(ProgressID, HGProgressEventTypes.ResponseRequest);
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
        /// Устанавливает минимальное и максимальное значения.
        /// </summary>
        protected abstract void SetLimit(float min, float max);

        /// <summary>
        /// Устанавливает текущее значение.
        /// </summary>
        protected abstract void SetValue(float value);

        /// <summary>
        /// Принимает изменения в рамках заданного прогресса.
        /// </summary>
        public virtual void OnHGEvent(HGProgressEvent e)
        {
            if (e.ProgressID != ProgressID) return;

            switch (e.EventType)
            {
                case HGProgressEventTypes.Initialized:
                    SetLimit(e.MinValue, e.MaxValue);
                    SetValue(e.CurrentValue);
                    break;

                case HGProgressEventTypes.Responsed:
                    SetLimit(e.MinValue, e.MaxValue);
                    SetValue(e.CurrentValue);
                    break;

                case HGProgressEventTypes.ValueChanged:
                    SetLimit(e.MinValue, e.MaxValue);
                    SetValue(e.CurrentValue);
                    break;

                case HGProgressEventTypes.LimitChanged:
                    SetLimit(e.MinValue, e.MaxValue);
                    SetValue(e.CurrentValue);
                    break;
            }
        }
    }
}