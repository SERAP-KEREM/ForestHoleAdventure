namespace SerapKeremGameTools._Game._Singleton
{
    /// <summary>
    /// A non-MonoBehaviour-based Singleton class.
    /// Ensures that only one instance of the class exists.
    /// </summary>
    public class NonMonoSingleton<T> where T : class, new()
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
                    _instance = new T();
                }
                return _instance;
            }
        }
    }
}
