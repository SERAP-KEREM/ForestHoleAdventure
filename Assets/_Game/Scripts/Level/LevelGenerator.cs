using UnityEngine;
using System.Collections.Generic;

namespace _Main._Level
{ 
public class LevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class ObjectSpawnInfo
    {
        public GameObject prefab;
        public int count;
        [Range(1f, 5f)] public float minSpacing = 2f; // Nesneler aras? minimum mesafe
    }

    [Header("Spawn Area")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 40f;
    [SerializeField] private float minZ = -20f;
    [SerializeField] private float maxZ = 40f;
    [SerializeField] private float yPosition = 0f;

    [Header("Objects to Spawn")]
    [SerializeField] private ObjectSpawnInfo[] objectsToSpawn;

    [Header("Debug")]
    [SerializeField] private bool showDebugGizmos = true;

    private List<Vector3> occupiedPositions = new List<Vector3>();

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        occupiedPositions.Clear();

        foreach (var objectInfo in objectsToSpawn)
        {
            SpawnObjects(objectInfo);
        }
    }

        private void SpawnObjects(ObjectSpawnInfo objectInfo)
        {
            int attempts = 0;
            int maxAttempts = objectInfo.count * 100; // Sonsuz döngüyü önlemek için
            int spawned = 0;

            while (spawned < objectInfo.count && attempts < maxAttempts)
            {
                Vector3 position = GetRandomPosition();

                if (IsPositionValid(position, objectInfo.minSpacing))
                {
                    // Instantiate s?ras?nda parent'? ayarla
                    GameObject obj = Instantiate(objectInfo.prefab, position, Quaternion.Euler(0, Random.Range(0f, 360f), 0), transform);
                    occupiedPositions.Add(position);
                    spawned++;

                    // Debug log
                    Debug.Log($"Spawned {objectInfo.prefab.name} at {position}");
                }

                attempts++;
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning($"Couldn't spawn all requested {objectInfo.prefab.name}. Space might be too crowded.");
            }
        }

        private Vector3 GetRandomPosition()
    {
        float x = Random.Range(minX, maxX);
        float z = Random.Range(minZ, maxZ);
        return new Vector3(x, yPosition, z);
    }

    private bool IsPositionValid(Vector3 position, float minSpacing)
    {
        foreach (Vector3 occupiedPos in occupiedPositions)
        {
            if (Vector3.Distance(position, occupiedPos) < minSpacing)
            {
                return false;
            }
        }

        // Raycast ile zemin kontrolü
        Ray ray = new Ray(position + Vector3.up * 10f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            if (hit.collider.CompareTag("Ground")) // Plane'e "Ground" tag'i ekleyin
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!showDebugGizmos) return;

        // Spawn alan?n? görselle?tir
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minX + maxX) * 0.5f, yPosition, (minZ + maxZ) * 0.5f);
        Vector3 size = new Vector3(maxX - minX, 0.1f, maxZ - minZ);
        Gizmos.DrawWireCube(center, size);

        // Yerle?tirilmi? nesnelerin pozisyonlar?n? görselle?tir
        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 pos in occupiedPositions)
            {
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }

    // Editor'da test için
    [ContextMenu("Generate Level")]
    public void GenerateLevelFromEditor()
    {
        // Mevcut nesneleri temizle
        foreach (Transform child in transform)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }

        GenerateLevel();
    }
}
}