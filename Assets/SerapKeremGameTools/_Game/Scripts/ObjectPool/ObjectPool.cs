using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameTools._Game._objectPool
{
    /// <summary>
    /// A generic object pool to manage reusable MonoBehaviour objects.
    /// </summary>
    /// <typeparam name="T">The type of objects that will be pooled (must be a MonoBehaviour).</typeparam>
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private Stack<T> pool;
        private T prefab;
        private Transform parent;

        /// <summary>
        /// Initializes the object pool with the given prefab, initial size, and parent.
        /// </summary>
        /// <param name="prefab">The prefab to instantiate objects from.</param>
        /// <param name="initialSize">The initial number of objects to create in the pool.</param>
        /// <param name="parent">The parent transform to attach the objects to.</param>
        public ObjectPool(T prefab, int initialSize, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
            pool = new Stack<T>(initialSize);

            // Create initial pool objects
            for (int i = 0; i < initialSize; i++)
            {
                T obj = GameObject.Instantiate(prefab, parent);
                obj.gameObject.SetActive(false); // Disable objects initially
                pool.Push(obj);
            }
        }

        /// <summary>
        /// Gets an object from the pool. If none are available, a new one is created.
        /// </summary>
        /// <returns>An object from the pool or a newly instantiated object.</returns>
        public T GetObject()
        {
            if (pool.Count > 0)
            {
                T obj = pool.Pop();
                obj.gameObject.SetActive(true); // Activate object
                return obj;
            }
            else
            {
                T obj = GameObject.Instantiate(prefab, parent);
                return obj;
            }
        }

        /// <summary>
        /// Returns an object to the pool, disabling it.
        /// </summary>
        /// <param name="obj">The object to be returned to the pool.</param>
        public void ReturnObject(T obj)
        {
            obj.gameObject.SetActive(false); // Disable object
            pool.Push(obj);
        }
    }
}
