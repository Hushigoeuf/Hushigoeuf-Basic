using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf.Basic
{
    public enum HGRotationTypes
    {
        LocalTransform,
        GlobalTransform,
        Rigidbody,
        Rigidbody2D,
    }

    /// <summary>
    /// Изменяет Transform (Rigidbody) объекта таким образом, чтобы тот вращался с заданной скоростью.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGRotation))]
    public class HGRotation : HGMonoBehaviour, HGEventListener<HGUpdateEvent>
    {
        public HGRotationTypes RotationType = HGRotationTypes.LocalTransform;

#if ODIN_INSPECTOR
        [DisableIf(nameof(RotationType), HGRotationTypes.Rigidbody2D)]
#endif
        public Vector3 Speed;

#if ODIN_INSPECTOR
        [EnableIf(nameof(RotationType), HGRotationTypes.Rigidbody2D)]
#endif
        [HGShowInInspector]
        public float Speed2D
        {
            get => Speed.z;
            set => Speed.z = value;
        }

        public bool ResetOnEnable = true;

        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected Rigidbody2D _rigidbody2D;
        protected Vector3 _startTransformRotation;
        protected Quaternion _startRigidbodyRotation;
        protected Quaternion _updateRigidbodyRotation;
        protected float _startRigidbody2DRotation;

        protected virtual void Awake()
        {
            switch (RotationType)
            {
                case HGRotationTypes.LocalTransform:
                    _transform = transform;
                    _startTransformRotation = _transform.localEulerAngles;
                    break;
                case HGRotationTypes.GlobalTransform:
                    _transform = transform;
                    _startTransformRotation = _transform.eulerAngles;
                    break;
                case HGRotationTypes.Rigidbody:
                    _rigidbody = GetComponent<Rigidbody>();
                    _startRigidbodyRotation = _rigidbody.rotation;
                    break;
                case HGRotationTypes.Rigidbody2D:
                    _rigidbody2D = GetComponent<Rigidbody2D>();
                    _startRigidbody2DRotation = _rigidbody2D.rotation;
                    break;
            }
        }

        protected virtual void OnEnable()
        {
            if (ResetOnEnable)
                switch (RotationType)
                {
                    case HGRotationTypes.LocalTransform:
                        _transform.localEulerAngles = _startTransformRotation;
                        break;
                    case HGRotationTypes.GlobalTransform:
                        _transform.eulerAngles = _startTransformRotation;
                        break;
                    case HGRotationTypes.Rigidbody:
                        _rigidbody.rotation = _startRigidbodyRotation;
                        break;
                    case HGRotationTypes.Rigidbody2D:
                        _rigidbody2D.rotation = _startRigidbody2DRotation;
                        break;
                }

            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        protected virtual void HGUpdate(float dt)
        {
            switch (RotationType)
            {
                case HGRotationTypes.LocalTransform:
                    _transform.localEulerAngles += Speed * dt;
                    break;
                case HGRotationTypes.GlobalTransform:
                    _transform.eulerAngles += Speed * dt;
                    break;
            }
        }

        protected virtual void HGFixedUpdate(float fdt)
        {
            switch (RotationType)
            {
                case HGRotationTypes.Rigidbody:
                    _updateRigidbodyRotation = Quaternion.Euler(Speed * fdt);
                    _rigidbody.MoveRotation(_rigidbody.rotation * _updateRigidbodyRotation);
                    break;
                case HGRotationTypes.Rigidbody2D:
                    _rigidbody2D.MoveRotation(_rigidbody2D.rotation + Speed.z * fdt);
                    break;
            }
        }

        public virtual void OnHGEvent(HGUpdateEvent e)
        {
            switch (e.EventType)
            {
                case HGUpdateEventTypes.Update:
                    HGUpdate(e.DateTime);
                    break;
                case HGUpdateEventTypes.FixedUpdate:
                    HGFixedUpdate(e.FixedDateTime);
                    break;
            }
        }
    }
}