using _Main._Objects;
using System.Collections.Generic;
using UnityEngine;

namespace _Main.Hole
{

    public class HoleObjectTransparency : MonoBehaviour
    {
        [SerializeField] private float _targetAlpha = 0.3f;
        [SerializeField] private LayerMask _objectsLayer;
        [SerializeField] private Transform _holeTransform;

        private Dictionary<Renderer, Material> _originalMaterials = new Dictionary<Renderer, Material>();
        private Dictionary<Renderer, Material> _transparentMaterials = new Dictionary<Renderer, Material>();

        private void Start()
        {
            if (_holeTransform == null)
                _holeTransform = transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _objectsLayer) != 0)
            {
                CollectibleItem collectible = other.GetComponent<CollectibleItem>();
                if (collectible != null)
                {
                    float holeSize = _holeTransform.localScale.x;
                    float objectSize = collectible.Size;

                    // Sadece hole'dan büyük nesneleri saydam yap
                    if (objectSize > holeSize)
                    {
                        MakeObjectTransparent(other.gameObject);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _objectsLayer) != 0)
            {
                RestoreObjectMaterial(other.gameObject);
            }
        }

        private void MakeObjectTransparent(GameObject obj)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null || renderer.material == null) return;

            // Orijinal materyali sakla
            if (!_originalMaterials.ContainsKey(renderer))
            {
                _originalMaterials[renderer] = renderer.material;
            }

            // Transparan materyal olu?tur veya var olan? kullan
            if (!_transparentMaterials.ContainsKey(renderer))
            {
                Material transparentMat = new Material(renderer.material);
                SetupTransparentMaterial(transparentMat);
                _transparentMaterials[renderer] = transparentMat;
            }

            // Transparan materyali uygula
            renderer.material = _transparentMaterials[renderer];
            Color color = renderer.material.color;
            color.a = _targetAlpha;
            renderer.material.color = color;

            Debug.Log($"Made {obj.name} transparent because it's larger than the hole");
        }

        private void RestoreObjectMaterial(GameObject obj)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            if (_originalMaterials.ContainsKey(renderer))
            {
                renderer.material = _originalMaterials[renderer];
                Debug.Log($"Restored {obj.name} to original material");
            }
        }

        private void SetupTransparentMaterial(Material material)
        {
            material.SetFloat("_Mode", 3);
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
            // Tüm nesneleri normal haline getir
            foreach (var renderer in _originalMaterials.Keys)
            {
                if (renderer != null)
                {
                    renderer.material = _originalMaterials[renderer];
                }
            }

            // Olu?turulan transparan materyalleri temizle
            foreach (var material in _transparentMaterials.Values)
            {
                Destroy(material);
            }

            _originalMaterials.Clear();
            _transparentMaterials.Clear();
        }
    }
}
