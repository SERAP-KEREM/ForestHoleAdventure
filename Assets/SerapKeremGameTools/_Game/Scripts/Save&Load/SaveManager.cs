using UnityEngine;

namespace SerapKeremGameTools._Game._SaveLoadSystem
{
    /// <summary>
    /// Manages saving and loading data using PlayerPrefs.
    /// </summary>
    public class SaveManager
    {
        /// <summary>
        /// Saves data of any type using PlayerPrefs.
        /// </summary>
        /// <typeparam name="T">The data type to save (string, int, float, bool)</typeparam>
        /// <param name="key">The key for the saved data</param>
        /// <param name="value">The value to save</param>
        public void SaveData<T>(string key, T value)
        {
            if (value == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Cannot save null value.");
#endif
                return;
            }

            if (value is string)
            {
                PlayerPrefs.SetString(key, value.ToString());
            }
            else if (value is int)
            {
                PlayerPrefs.SetInt(key, (int)(object)value);
            }
            else if (value is float)
            {
                PlayerPrefs.SetFloat(key, (float)(object)value);
            }
            else if (value is bool)
            {
                PlayerPrefs.SetInt(key, (bool)(object)value ? 1 : 0);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Unsupported type: {typeof(T)}");
#endif
                return;
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// Saves a string value using PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <param name="value">The string value to save</param>
        public void SaveData_String(string key, string value)
        {
            SaveData(key, value);
        }

        /// <summary>
        /// Saves an integer value using PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <param name="value">The integer value to save</param>
        public void SaveData_Int(string key, int value)
        {
            SaveData(key, value);
        }

        /// <summary>
        /// Saves a float value using PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <param name="value">The float value to save</param>
        public void SaveData_Float(string key, float value)
        {
            SaveData(key, value);
        }

        /// <summary>
        /// Saves a boolean value using PlayerPrefs.
        /// </summary>
        /// <param name="key">The key for the saved data</param>
        /// <param name="value">The boolean value to save</param>
        public void SaveData_Bool(string key, bool value)
        {
            SaveData(key, value);
        }

        /// <summary>
        /// Clears saved data by a given key.
        /// </summary>
        /// <param name="key">The key for the saved data to delete</param>
        public void ClearData(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.DeleteKey(key);
                PlayerPrefs.Save();
#if UNITY_EDITOR
                Debug.Log($"Data cleared for key: {key}");
#endif
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogWarning($"No data found for key: {key}");
#endif
            }
        }

        /// <summary>
        /// Clears all saved PlayerPrefs data.
        /// </summary>
        public void ClearAllData()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
#if UNITY_EDITOR
            Debug.Log("All PlayerPrefs data cleared.");
#endif
        }
    }
}