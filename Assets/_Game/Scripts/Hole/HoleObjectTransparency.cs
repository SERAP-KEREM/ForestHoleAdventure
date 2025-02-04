using _Main._Objects;
using System.Collections.Generic;
using UnityEngine;

namespace _Main._Hole
{
    /// <summary>
    /// Manages the transparency of objects when they enter the hole's trigger area, based on size comparison.
    /// </summary>
    public class HoleObjectTransparency : MonoBehaviour
    {
        #region Serialized Fields

        [Header("Transparency Settings")]
        [SerializeField, Tooltip("Target alpha value for transparency.")]
        private float _targetAlpha = 0.3f;

        [Header("Object Layer Settings")]
        [SerializeField, Tooltip("Layer mask to filter objects that can be made transparent.")]
        private LayerMask _objectsLayer;

        [Header("Hole Settings")]
        [SerializeField, Tooltip("Transform of the hole object.")]
        private Transform _holeTransform;

        #endregion

        #region Private Fields

        private Dictionary<Renderer, Material> _originalMaterials = new Dictionary<Renderer, Material>();
        private Dictionary<Renderer, Material> _transparentMaterials = new Dictionary<Renderer, Material>();

        #endregion

        private void Start()
        {
            // Default to current transform if not assigned
            if (_holeTransform == null)
                _holeTransform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            // Check if the object is in the specified layer
            if (((1 << other.gameObject.layer) & _objectsLayer) != 0)
            {
                CollectibleItem collectible = other.GetComponent<CollectibleItem>();
                if (collectible != null)
                {
                    // Compare the size of the object and the hole
                    float holeSize = _holeTransform.localScale.x;
                    float objectSize = collectible.Size;

                    // If the object is larger than the hole, make it transparent
                    if (objectSize > holeSize)
                    {
                        MakeObjectTransparent(other.gameObject);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Restore the material when the object exits the trigger area
            if (((1 << other.gameObject.layer) & _objectsLayer) != 0)
            {
                RestoreObjectMaterial(other.gameObject);
            }
        }

        /// <summary>
        /// Makes the object transparent when it is larger than the hole.
        /// </summary>
        private void MakeObjectTransparent(GameObject obj)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null || renderer.material == null) return;

            // Store the original material if it's not already stored
            if (!_originalMaterials.ContainsKey(renderer))
            {
                _originalMaterials[renderer] = renderer.material;
            }

            // Create a transparent material if it doesn't already exist
            if (!_transparentMaterials.ContainsKey(renderer))
            {
                Material transparentMat = new Material(renderer.material);
                SetupTransparentMaterial(transparentMat);
                _transparentMaterials[renderer] = transparentMat;
            }

            // Apply the transparent material
            renderer.material = _transparentMaterials[renderer];
            Color color = renderer.material.color;
            color.a = _targetAlpha;
            renderer.material.color = color;

            Debug.Log($"Made {obj.name} transparent because it's larger than the hole");
        }

        /// <summary>
        /// Restores the object's original material when it exits the hole's trigger area.
        /// </summary>
        private void RestoreObjectMaterial(GameObject obj)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            // Restore the original material if it was previously stored
            if (_originalMaterials.ContainsKey(renderer))
            {
                renderer.material = _originalMaterials[renderer];
                Debug.Log($"Restored {obj.name} to original material");
            }
        }

        /// <summary>
        /// Sets up the material properties for transparency.
        /// </summary>
        private void SetupTransparentMaterial(Material material)
        {
            material.SetFloat("_Mode", 3); // Set material mode to transparent
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = 3000;
        }

        private void OnDestroy()
        {
            // Restore original materials and clean up when the script is destroyed
            foreach (var renderer in _originalMaterials.Keys)
            {
                if (renderer != null)
                {
                    renderer.material = _originalMaterials[renderer];
                }
            }

            // Destroy transparent materials to prevent memory leaks
            foreach (var material in _transparentMaterials.Values)
            {
                Destroy(material);
            }

            _originalMaterials.Clear();
            _transparentMaterials.Clear();
        }
    }
}
