using UnityEngine;

namespace HitAndRun
{
    public abstract class MbSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static bool IsInstance => _instance != null;

        private static string SingletonName => typeof(T).Name;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                var instances = FindObjectsOfType(typeof(T));
                if (instances.Length >= 1)
                {
                    if (Application.isPlaying)
                    {
                        Debug.AssertFormat(
                            instances.Length == 1,
                            "{1} {0} singletons detected. There should only ever be one present",
                            SingletonName,
                            instances.Length);
                    }

                    _instance = (T)instances[0];
                    return _instance;
                }

                var singleton = new GameObject(SingletonName);
                _instance = singleton.AddComponent<T>();

                if (Application.isPlaying)
                {
                    DontDestroyOnLoad(singleton);
                }

                return _instance;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        public static void ClearSingleton()
        {
            if (Application.isPlaying || _instance == null)
            {
                return;
            }

            DestroyImmediate(_instance.gameObject);
            _instance = null;
        }
    }

}
