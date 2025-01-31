using _Main._Level;
using _Main._Objects;
using UnityEngine;

namespace _Main.Hole
{
    public class HoleManager : MonoBehaviour
    {
        [SerializeField] private Transform _holeTransform;
        [SerializeField] private LevelManager _levelManager; // Reference to LevelManager

        private void OnTriggerEnter(Collider other)
        {
            CollectibleItem collectible = other.GetComponent<CollectibleItem>();
            if (collectible != null && !collectible.IsCollected)
            {
                float holeSize = _holeTransform.localScale.x;
                float objectSize = collectible.Size;

                if (holeSize >= objectSize)
                {
                    // Puanı LevelManager'a bildir
                    _levelManager.AddScore(collectible.Score);
                    collectible.Collect();

                    Debug.Log($"Collected: {other.gameObject.name}, Score: {collectible.Score}");
                }
                else
                {
                    Debug.Log($"Object too big: Hole size {holeSize}, Object size {objectSize}");
                }
            }
        }

        // Deliği büyütmek için public metod
        public void SetHoleSize(float size)
        {
            Vector3 newScale = Vector3.one * size;
            // Y scale'i 0 olarak tut
            newScale.y = _holeTransform.localScale.y;
            _holeTransform.localScale = newScale;

            // Pozisyonu zemine sabitle
            Vector3 position = _holeTransform.position;
            position.y = 0f; // veya başlangıç Y pozisyonu
            _holeTransform.position = position;
        }
    }
}