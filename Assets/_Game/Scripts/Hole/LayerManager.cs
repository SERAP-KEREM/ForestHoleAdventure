using UnityEngine;

namespace _Main._Hole
{
    /// <summary>
    /// Manages the layer change of objects when they enter or exit the trigger area, based on their size.
    /// </summary>
    public class LayerManager : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Layer Settings")]
        [SerializeField, Tooltip("List of layers to switch between. Index 0 for default, index 1 for noColl.")]
        private string[] _layers = { "Default", "noColl" };

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object is small enough to trigger layer change
            if (IsObjectSmallEnough(other))
            {
                ChangeLayer(other, 1);  // Change to 'noColl' layer
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Restore layer to default when the object exits the trigger area
            ChangeLayer(other, 0);  // Change back to 'Default' layer
        }

        /// <summary>
        /// Checks if the object is small enough to fit into the hole.
        /// </summary>
        private bool IsObjectSmallEnough(Collider other)
        {
            float objectSize = GetObjectSize(other.gameObject);
            float holeSize = transform.localScale.x;

            return objectSize <= holeSize;
        }

        /// <summary>
        /// Changes the layer of the object.
        /// </summary>
        private void ChangeLayer(Collider other, int index)
        {
            other.gameObject.layer = LayerMask.NameToLayer(_layers[index]);
        }

        /// <summary>
        /// Gets the maximum size of the object based on its collider's bounds.
        /// </summary>
        private float GetObjectSize(GameObject obj)
        {
            Bounds bounds = obj.GetComponent<Collider>().bounds;
            return Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
        }
    }
}
