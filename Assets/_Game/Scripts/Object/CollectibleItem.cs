using UnityEngine;
using DG.Tweening;

namespace _Main._Objects
{
    public class CollectibleItem : MonoBehaviour
    {
        [Header("Collection Animation Settings")]
        [SerializeField] private float _collectDuration = 1f;
        [SerializeField] private float _spiralRadius = 0.5f;
        [SerializeField] private float _spiralRotations = 1.5f;
        [SerializeField] private float _fallHeight = 0.5f;

        [Header("Object Settings")]
        [SerializeField] private CollectibleType _type;


        private float _size;
        private bool _isCollected;

        public int Score => (int)_type;
        public float Size => _size;
        public bool IsCollected => _isCollected;

        private void Awake()
        {
            CalculateSize();
        }

        private void CalculateSize()
        {
            Bounds bounds = GetComponent<Collider>().bounds;
            // Daha küçük bir boyut hesaplamas? yapal?m
            _size = Mathf.Max(bounds.size.x, bounds.size.z) * 0.5f; // 0.5f çarpan? ekledik

            // Boyutu logla
            Debug.Log($"Object {gameObject.name} calculated size: {_size}");
        }

        public void Collect(Transform holeTransform)
        {
            if (!_isCollected)
            {
                _isCollected = true;
                PlayCollectAnimation(holeTransform);
            }
        }
        private void PlayCollectAnimation(Transform holeTransform)
        {
            // Animasyon ba?lang?c?nda nesnenin ve deli?in pozisyonlar?
            Vector3 startPos = transform.position;
            Vector3 targetPos = holeTransform.position;

            // Sequence olu?tur
            Sequence collectSequence = DOTween.Sequence();

            // Spiral hareket için path olu?tur
            Vector3[] path = CreateSpiralPath(startPos, targetPos);

            // Path boyunca hareket
            collectSequence.Append(transform.DOPath(path, _collectDuration, PathType.CatmullRom)
                .SetEase(Ease.InQuad));

            // Döndürme animasyonu
            collectSequence.Join(transform.DORotate(new Vector3(360f, 360f, 360f), _collectDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.InQuad));

            // Küçülme animasyonu (gecikmeli ba?las?n)
            collectSequence.Join(transform.DOScale(Vector3.zero, _collectDuration * 0.9f)
                .SetEase(Ease.InQuad)
                .SetDelay(_collectDuration * 0.3f));

            // Animasyon bitiminde
            collectSequence.OnComplete(() =>
            {
                gameObject.SetActive(false);
                transform.position = startPos;
                transform.localScale = Vector3.one;
            });
        }
        private Vector3[] CreateSpiralPath(Vector3 start, Vector3 end)
        {
            int pointCount = 20; // Path noktalar?n?n say?s?
            Vector3[] path = new Vector3[pointCount];

            for (int i = 0; i < pointCount; i++)
            {
                float t = i / (float)(pointCount - 1); // 0 ile 1 aras? de?er

                // Spiral hareketi hesapla
                float angle = t * _spiralRotations * Mathf.PI * 2f;
                float radius = _spiralRadius * (1f - t); // Radius giderek azals?n

                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                // Yükseklik hesapla (parabolik dü?ü?)
                float y = _fallHeight * (1f - t * t);

                // Noktay? hesapla
                Vector3 offset = new Vector3(x, -y, z);
                path[i] = Vector3.Lerp(start, end, t) + offset;
            }

            return path;
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