using UnityEngine;
using DG.Tweening;

namespace _Main._Objects
{
    public class CollectibleItem : MonoBehaviour
    {
        [Header("Collection Animation Settings")]
        [SerializeField] private float _collectDuration = 2f; // Düşme süresi
        [SerializeField] private float _fallDepth = 0.5f; // Delik içine düşme mesafesi
        [SerializeField] private float _fadeDuration = 1.5f; // Şeffaflaşma süresi

        [Header("Object Settings")]
        [SerializeField] private CollectibleType _type;

        private Material _material;
        private Color _originalColor;
        private float _size;
        private bool _isCollected;

        public int Score => (int)_type;
        public float Size => _size;
        public bool IsCollected => _isCollected;

        private void Awake()
        {
            CalculateSize();
            InitializeMaterial();
        }

        private void CalculateSize()
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            _size = Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f;

            Debug.Log($"Object {gameObject.name} calculated size: {_size}");
        }

        private void InitializeMaterial()
        {
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                _material = renderer.material;
                _originalColor = _material.color;

                // Materyali transparan moduna ayarla
                _material.SetFloat("_Surface", 1); // Transparent
                _material.SetOverrideTag("RenderType", "Transparent");
               
                _material.SetInt("_ZWrite", 0);
                _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                _material.EnableKeyword("_ALPHABLEND_ON");
            }
        }

        public void Collect(Transform holeTransform)
        {
            if (!_isCollected)
            {
                _isCollected = true;

                // Basit düşme animasyonu
                transform.DOMove(holeTransform.position, _collectDuration)
                    .SetEase(Ease.InQuad);

                // Hafif küçülme
                transform.DOScale(Vector3.zero, _collectDuration)
                    .SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        gameObject.SetActive(false);
                        // Nesneyi orijinal haline getir
                        transform.localScale = Vector3.one;
                    });
            }
        }

        private void PlayCollectAnimation(Transform holeTransform)
        {
            // Hedef pozisyon deliğin biraz altı
            Vector3 targetPos = holeTransform.position - Vector3.up * _fallDepth;

            // Şeffaflaşma animasyonu
            FadeOut();

            // Yavaş bir şekilde deliğe doğru düşme animasyonu
            transform.DOMove(targetPos, _collectDuration)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                });
        }

        private void FadeOut()
        {
            if (_material != null)
            {
                _material.DOFade(0f, _fadeDuration)
                    .OnComplete(() =>
                    {
                        // Nesne tamamen şeffaf hale geldiğinde materyali eski haline döndür
                        _material.color = _originalColor;
                    });
            }
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Vector3 size = new Vector3(_size, 0.1f, _size);
                Gizmos.DrawWireCube(transform.position, size);
            }
        }
    }
}
