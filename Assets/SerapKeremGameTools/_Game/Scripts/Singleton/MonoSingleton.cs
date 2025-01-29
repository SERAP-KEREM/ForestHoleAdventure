using UnityEngine;

namespace SerapKeremGameTools._Game._Singleton
{
    /// <summary>
    /// A MonoBehaviour-based Singleton class.
    /// Ensures that only one instance of the class exists.
    /// </summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        /// <summary>
        /// Gets the instance of the singleton.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    // Try to find the singleton instance in the scene
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        // If not found, create a new GameObject with the MonoSingleton
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        _instance = singletonObject.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Ensures the instance persists across scenes.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject); // Keeps the singleton alive across scene loads
            }
            else if (_instance != this)
            {
                Destroy(gameObject); // Destroys the new instance if there's already one
            }
        }
    }
}
