using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Hushigoeuf.Basic
{
    /// <summary>
    /// Простой способ адаптировать камеру под разные разрешения.
    /// </summary>
    [AddComponentMenu(HGEditor.PATH_MENU_COMMON + nameof(HGCameraResolutionFilter))]
    [RequireComponent(typeof(Camera))]
    public class HGCameraResolutionFilter : HGMonoBehaviour, HGEventListener<HGUpdateEvent>
    {
#if ODIN_INSPECTOR
        [MinValue(0)]
#endif
        [HGShowInSettings]
        [HGBorders]
        [SerializeField]
        public Vector2 TargetResolution = new Vector2(1920, 1080);

        [HGShowInSettings] [HGBorders] [Range(0f, 1f)] [SerializeField]
        public float WidthOrHeight = 0;

        [HGShowInSettings] [HGBorders] [SerializeField]
        public bool UpdateOnEveryFrame;

        protected Camera _targetCamera;
        protected float _sizeOnStart;
        protected float _fovOnStart;
        protected float _targetAspect;
        protected float _hFov;

        protected virtual void Start()
        {
            _targetCamera = GetComponent<Camera>();

            _targetAspect = TargetResolution.x / TargetResolution.y;

            UpdateResolution();
        }

        protected virtual void OnEnable()
        {
            this.HGEventStartListening();
        }

        protected virtual void OnDisable()
        {
            this.HGEventStopListening();
        }

        protected virtual void HGUpdate(float dt)
        {
            if (_targetCamera.orthographic)
            {
                float size = _sizeOnStart * (_targetAspect / _targetCamera.aspect);
                _targetCamera.orthographicSize = Mathf.Lerp(size, _sizeOnStart, WidthOrHeight);
            }
            else
            {
                float size = CalculateVerticalFOV(_hFov, _targetCamera.aspect);
                _targetCamera.fieldOfView = Mathf.Lerp(size, _fovOnStart, WidthOrHeight);
            }
        }

        protected virtual float CalculateVerticalFOV(float hFovInDeg, float aspectRatio)
        {
            float hFovInRads = hFovInDeg * Mathf.Deg2Rad;
            float vFovInRads = 2 * Mathf.Atan(Mathf.Tan(hFovInRads / 2) / aspectRatio);

            return vFovInRads * Mathf.Rad2Deg;
        }

        public virtual void UpdateResolution()
        {
            _sizeOnStart = _targetCamera.orthographicSize;
            _fovOnStart = _targetCamera.fieldOfView;

            _hFov = CalculateVerticalFOV(_fovOnStart, 1 / _targetAspect);

            if (!UpdateOnEveryFrame) HGUpdate(0);
        }

        public virtual void OnHGEvent(HGUpdateEvent e)
        {
            if (e.EventType == HGUpdateEventTypes.Update)
                if (UpdateOnEveryFrame)
                    HGUpdate(e.DateTime);
        }
    }
}