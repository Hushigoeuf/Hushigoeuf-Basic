using System.Collections.Generic;
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

    public abstract class HGSingletonListMonoBehaviour<T> : HGFullyMonoBehaviour where T : Component
    {
        public static Dictionary<string, T> Instances = new Dictionary<string, T>();

        protected abstract string InstanceID { get; }

        protected override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying) return;
            if (InstanceID == null) return;

            if (Instances.ContainsKey(InstanceID))
            {
                Destroy(this);
                return;
            }

            Instances.Add(InstanceID, this as T);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (InstanceID == null) return;

            if (Instances.ContainsKey(InstanceID))
                Instances.Remove(InstanceID);
        }

        public static T GetInstance(string instanceID, bool findIfNotContains = true)
        {
            if (Instances.ContainsKey(instanceID))
                return Instances[instanceID];
            if (findIfNotContains)
                foreach (var instance in FindObjectsOfType<InputManager>())
                    if (instance.InstanceID == instanceID)
                        return instance as T;
            return null;
        }
    }
}