using UnityEngine;

namespace _Main._Objects
{

    public class CollectibleItem : MonoBehaviour
    {
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
            // Collider'?n gerçek boyutunu al
            Bounds bounds = GetComponent<Collider>().bounds;
            _size = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

            // Debug için boyutu logla
            Debug.Log($"Object {gameObject.name} size calculated: {_size}");
        }

        public void Collect()
        {
            if (!_isCollected)
            {
                _isCollected = true;
                gameObject.SetActive(false);
                Debug.Log($"Object {gameObject.name} collected with score: {Score}");
            }
        }
        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(transform.position, Vector3.one * _size);
            }
        }
    }
}