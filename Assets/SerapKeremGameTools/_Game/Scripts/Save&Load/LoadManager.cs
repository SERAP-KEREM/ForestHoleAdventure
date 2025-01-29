using UnityEngine;

namespace SerapKeremGameTools._Game._SaveLoadSystem
{
    /// <summary>
    /// Manages loading data using PlayerPrefs.
    /// </summary>
    public class LoadManager
    {
        /// <summary>
        /// Loads data of any type from PlayerPrefs.
        /// </summary>
        /// <typeparam name="T">The data type to load (string, int, float, bool)</typeparam>
        /// <param name="key">The key for the saved data</param>
        /// <param name="defaultValue">The default value to return if the data does not exist</param>
        /// <returns>The loaded data of type T</returns>
        public T LoadData<T>(string key, T defaultValue = default)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)(PlayerPrefs.GetInt(key, 0) == 1); // 0 -> false, 1 -> true
            }

#if UNITY_EDITOR
            Debug.LogWarning($"Unsupported type: {typeof(T)}");
#endif
            return defaultValue;
        }

        /// <summary>
        /// Loads a string value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded string value</returns>
        public string LoadData_String(string key)
        {
            return LoadData(key, "");
        }

        /// <summary>
        /// Loads an integer value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded integer value</returns>
        public int LoadData_Int(string key)
        {
            return LoadData(key, 0);
        }

        /// <summary>
        /// Loads a float value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded float value</returns>
        public float LoadData_Float(string key)
        {
            return LoadData(key, 0f);
        }

        /// <summary>
        /// Loads a boolean value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded boolean value</returns>
        public bool LoadData_Bool(string key)
        {
            return LoadData(key, false);
        }
    }
}