using UnityEngine;
using System.Collections.Generic;
using TriInspector;

namespace _Main._Level
{
    public class LevelGenerator : MonoBehaviour
    {
        #region Serialized Classes

        [System.Serializable]
        public class SpawnZone
        {
            [PropertyTooltip("Name of the spawn zone.")]
            [Group("Spawn Zone Settings")]
            public string name;

            [PropertyTooltip("Minimum X position of the zone.")]
            [Group("Spawn Zone Settings")]
            public float minX;

            [PropertyTooltip("Maximum X position of the zone.")]
            [Group("Spawn Zone Settings")]
            public float maxX;

            [PropertyTooltip("Minimum Z position of the zone.")]
            [Group("Spawn Zone Settings")]
            public float minZ;

            [PropertyTooltip("Maximum Z position of the zone.")]
            [Group("Spawn Zone Settings")]
            public float maxZ;

            [PropertyTooltip("Y position of the spawn zone.")]
            [Group("Spawn Zone Settings")]
            public float yPosition = 0f;
        }

        [System.Serializable]
        public class ObjectSpawnInfo
        {
            [PropertyTooltip("Prefab to spawn.")]
            [Group("Object Spawn Settings")]
            public GameObject prefab;

            [PropertyTooltip("Number of objects to spawn.")]
            [Group("Object Spawn Settings")]
            public int count;

            [PropertyTooltip("Minimum spacing between objects.")]
            [Group("Object Spawn Settings")]
            [Range(1f, 5f)] public float minSpacing = 2f;
        }

        [System.Serializable]
        public class ClassifiedSpawnInfo
        {
            [PropertyTooltip("Classification of the spawn area (e.g., 'Forest', 'Desert').")]
            [Group("Classified Spawn Settings")]
            public string classification;

            [PropertyTooltip("The spawn zone for this classification.")]
            [Group("Classified Spawn Settings")]
            public SpawnZone zone;

            [PropertyTooltip("List of objects to spawn in this classification.")]
            [Group("Classified Spawn Settings")]
            public List<ObjectSpawnInfo> objectsToSpawn;
        }

        #endregion

        #region Serialized Fields

        [Group("Classified Spawn Settings")]
        [PropertyTooltip("List of classified spawn zones and their objects.")]
        [SerializeField] private List<ClassifiedSpawnInfo> classifiedObjects;

        [Group("Debug Settings")]
        [PropertyTooltip("Toggle to show gizmos in the editor.")]
        [SerializeField] private bool showDebugGizmos = true;

        private List<Vector3> occupiedPositions = new List<Vector3>();

        #endregion

        #region Unity Lifecycle Methods

        /// <summary>
        /// Starts the level generation process when the game begins.
        /// </summary>
        private void Start()
        {
            GenerateLevel();
        }

        #endregion

        #region Level Generation Methods

        /// <summary>
        /// Generates the level by spawning objects in their classified zones.
        /// Clears the occupied positions before starting the generation process.
        /// </summary>
        public void GenerateLevel()
        {
            occupiedPositions.Clear();

            foreach (var classifiedInfo in classifiedObjects)
            {
                SpawnObjectsInZone(classifiedInfo);
            }
        }

        /// <summary>
        /// Spawns objects in a specific zone based on the classified spawn information.
        /// Tries to spawn objects and ensures proper spacing.
        /// </summary>
        /// <param name="classifiedInfo">The classified spawn information including the zone and objects.</param>
        private void SpawnObjectsInZone(ClassifiedSpawnInfo classifiedInfo)
        {
            foreach (var objectInfo in classifiedInfo.objectsToSpawn)
            {
                int attempts = 0;
                int maxAttempts = objectInfo.count * 100;
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

        /// <summary>
        /// Gets a random position within a specified spawn zone.
        /// </summary>
        /// <param name="zone">The spawn zone where the position will be chosen.</param>
        /// <returns>A random position within the spawn zone.</returns>
        private Vector3 GetRandomPositionInZone(SpawnZone zone)
        {
            float x = Random.Range(zone.minX, zone.maxX);
            float z = Random.Range(zone.minZ, zone.maxZ);
            return new Vector3(x, zone.yPosition, z);
        }

        /// <summary>
        /// Checks if a position is valid by ensuring it has enough spacing from other objects and is on the ground.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="minSpacing">The minimum allowed spacing between objects.</param>
        /// <returns>True if the position is valid; otherwise, false.</returns>
        private bool IsPositionValid(Vector3 position, float minSpacing)
        {
            foreach (Vector3 occupiedPos in occupiedPositions)
            {
                if (Vector3.Distance(position, occupiedPos) < minSpacing)
                {
                    return false;
                }
            }

            Ray ray = new Ray(position + Vector3.up * 10f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 20f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Gizmo Debugging

        /// <summary>
        /// Draws debug gizmos for the spawn zones and occupied positions when the object is selected in the scene view.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            if (!showDebugGizmos) return;

            Gizmos.color = Color.green;

            foreach (var classifiedInfo in classifiedObjects)
            {
                SpawnZone zone = classifiedInfo.zone;
                Vector3 center = new Vector3((zone.minX + zone.maxX) * 0.5f, zone.yPosition, (zone.minZ + zone.maxZ) * 0.5f);
                Vector3 size = new Vector3(zone.maxX - zone.minX, 0.1f, zone.maxZ - zone.minZ);
                Gizmos.DrawWireCube(center, size);
            }

            Gizmos.color = Color.red;
            if (Application.isPlaying)
            {
                foreach (Vector3 pos in occupiedPositions)
                {
                    Gizmos.DrawWireSphere(pos, 0.5f);
                }
            }
        }

        #endregion

        #region Editor Methods

        /// <summary>
        /// Generates the level directly from the editor using the context menu. Destroys existing objects first.
        /// </summary>
        [ContextMenu("Generate Level")]
        public void GenerateLevelFromEditor()
        {
            foreach (Transform child in transform)
            {
                if (Application.isPlaying)
                    Destroy(child.gameObject);
                else
                    DestroyImmediate(child.gameObject);
            }

            GenerateLevel();
        }

        #endregion
    }
}
