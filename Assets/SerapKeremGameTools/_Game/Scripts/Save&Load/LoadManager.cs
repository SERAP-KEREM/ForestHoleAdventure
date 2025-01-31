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
        public static T LoadData<T>(string key,  T defaultValue = default)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)LoadData_String(key);
            }
            else if (typeof(T) == typeof(int))
            {
                return (T)(object)LoadData_Int(key);
            }
            else if (typeof(T) == typeof(float))
            {
                return (T)(object)LoadData_Float(key);
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
        private static string LoadData_String(string key,string defaultValue="")
        {
            return PlayerPrefs.GetString(key);
        }

        /// <summary>
        /// Loads an integer value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded integer value</returns>
        private static int LoadData_Int(string key)
        {
            return PlayerPrefs.GetInt(key);
        }

        /// <summary>
        /// Loads a float value from PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <returns>The loaded float value</returns>
        private static float LoadData_Float(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

      
    }
}