using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [System.Serializable]
    public class SpawnZone
    {
        public string name; // Örne?in: "Trees", "Rocks", "Homes"
        public float minX;
        public float maxX;
        public float minZ;
        public float maxZ;
        public float yPosition = 0f;
    }

    [System.Serializable]
    public class ObjectSpawnInfo
    {
        public GameObject prefab; // Spawn edilecek prefab
        public int count; // Kaç tane spawn edilece?i
        [Range(1f, 5f)] public float minSpacing = 2f; // Minimum mesafe
    }

    [System.Serializable]
    public class ClassifiedSpawnInfo
    {
        public string classification; // Örne?in: "Trees", "Rocks", "Homes"
        public SpawnZone zone; // S?n?fa ait alan (zone)
        public List<ObjectSpawnInfo> objectsToSpawn; // O alandaki nesneler
    }

    [Header("Classified Spawn Settings")]
    [SerializeField] private List<ClassifiedSpawnInfo> classifiedObjects;

    [Header("Debug Settings")]
    [SerializeField] private bool showDebugGizmos = true;

    private List<Vector3> occupiedPositions = new List<Vector3>();

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        occupiedPositions.Clear();

        foreach (var classifiedInfo in classifiedObjects)
        {
            SpawnObjectsInZone(classifiedInfo);
        }
    }

    private void SpawnObjectsInZone(ClassifiedSpawnInfo classifiedInfo)
    {
        foreach (var objectInfo in classifiedInfo.objectsToSpawn)
        {
            int attempts = 0;
            int maxAttempts = objectInfo.count * 100; // Sonsuz döngüyü önlemek için
            int spawned = 0;

            while (spawned < objectInfo.count && attempts < maxAttempts)
            {
                Vector3 position = GetRandomPositionInZone(classifiedInfo.zone);

                if (IsPositionValid(position, objectInfo.minSpacing))
                {
                    GameObject obj = Instantiate(objectInfo.prefab, position, Quaternion.Euler(0, Random.Range(0f, 360f), 0), transform);
                    occupiedPositions.Add(position);
                    spawned++;

                    Debug.Log($"Spawned {objectInfo.prefab.name} in {classifiedInfo.classification} zone at {position}");
                }

                attempts++;
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning($"Couldn't spawn all requested {objectInfo.prefab.name} in zone {classifiedInfo.classification}. Space might be too crowded.");
            }
        }
    }

    private Vector3 GetRandomPositionInZone(SpawnZone zone)
    {
        float x = Random.Range(zone.minX, zone.maxX);
        float z = Random.Range(zone.minZ, zone.maxZ);
        return new Vector3(x, zone.yPosition, z);
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

        // Raycast ile zemin kontrolü (iste?e ba?l?)
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

        Gizmos.color = Color.green;

        // Tüm alanlar? görselle?tir
        foreach (var classifiedInfo in classifiedObjects)
        {
            SpawnZone zone = classifiedInfo.zone;
            Vector3 center = new Vector3((zone.minX + zone.maxX) * 0.5f, zone.yPosition, (zone.minZ + zone.maxZ) * 0.5f);
            Vector3 size = new Vector3(zone.maxX - zone.minX, 0.1f, zone.maxZ - zone.minZ);
            Gizmos.DrawWireCube(center, size);
        }

        // Yerle?tirilen nesnelerin pozisyonlar?n? görselle?tir
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
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
