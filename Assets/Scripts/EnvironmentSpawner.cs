using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public float SpeedValue;
    [SerializeField] private RoadSpawner RoadSpawner;
    [SerializeField] private Transform spawnPoint; // Position where new objects will be spawned
    [SerializeField] private int minRightOffset = 20; // Minimum offset to the right of the spawn point
    [SerializeField] private int maxRightOffset = 60; // Maximum offset to the right of the spawn point
    [SerializeField] private int minLeftOffset = -40; // Minimum offset forward from the spawn point
    [SerializeField] private int maxLeftOffset = -80; // Maximum offset forward from the spawn point
    [SerializeField] private int spawnBatchCount = 4; // Number of buildings to spawn at once
    [SerializeField] private int positionQueueSize = 3; // Number of recently used positions to track
    [SerializeField] private int maxValidationAttempts = 10; // Maximum attempts to find a valid position
    [Header("References")]
    [SerializeField] private PrefabController prefabController; // Reference to the PrefabController

    [SerializeField]private List<Vector3> recentZPositions = new List<Vector3>(); // List to store recent Z positions
    public GameObject ScenePrefabParent;
    private void Start()
    {
        StartCoroutine(SpawnDelay());
    }

    private void Update()
    {
        SpeedValue = RoadSpawner.Roadspeed;
    }

    IEnumerator SpawnDelay()
    {
        while (true)
        {
            float randomValue = Random.value;

            if (randomValue < 0.97f)
            {
                // Equal chance to spawn buildings on left or right
                float randomValue2 = Random.value;
                if (randomValue2 < 0.5f)
                {
                    SpawnBuildingsRightBatch(spawnBatchCount);
                }
                else
                {
                    SpawnBuildingsLeftBatch(spawnBatchCount);
                }
            }
            else
            {
                //Debug.Log("Bridge random value: " + randomValue);
                yield return new WaitForSeconds(1);
                SpawnFlyover();
                yield return new WaitForSeconds(1);
            }

            float random = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(random);
        }
    }

    private void SpawnBuildingsRightBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnbuildingsRight();
        }
    }

    private void SpawnBuildingsLeftBatch(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnbuildingsLeft();
        }
    }

    private void SpawnbuildingsRight()
    {
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 0, -90);

        if (buildingPrefab != null)
        {
            Vector3 spawnPosition;

            // Generate a valid spawn position
            int attempts = 0;
            do
            {
                spawnPosition = new Vector3(
                    spawnPoint.position.x + Random.Range(-10, 50),
                    spawnPoint.position.y - 20,
                    spawnPoint.position.z + Random.Range(minRightOffset, maxRightOffset)
                );
                attempts++;
            } while (!IsPositionValid(spawnPosition) && attempts < maxValidationAttempts);

            if (attempts < maxValidationAttempts)
            {
                AddToRecentPositions(spawnPosition);

                GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
                newBuilding.transform.parent = ScenePrefabParent.transform;
                newBuilding.transform.localScale *= 1.5f;
            }
            else
            {
                Debug.LogWarning("Failed to find a valid position for a right building after maximum attempts.");
            }
        }
    }

    private void SpawnbuildingsLeft()
    {
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 90, 0);

        if (buildingPrefab != null)
        {
            Vector3 spawnPosition;

            // Generate a valid spawn position
            int attempts = 0;
            do
            {
                spawnPosition = new Vector3(
                    spawnPoint.position.x + Random.Range(-10, 50),
                    spawnPoint.position.y - 20,
                    spawnPoint.position.z + Random.Range(minLeftOffset, maxLeftOffset)
                );
                attempts++;
            } while (!IsPositionValid(spawnPosition) && attempts < maxValidationAttempts);

            if (attempts < maxValidationAttempts)
            {
                AddToRecentPositions(spawnPosition);

                GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
                newBuilding.GetComponent<PrefabScript>().isMovingUp = false;
                newBuilding.transform.localScale *= 1.5f;
                newBuilding.transform.parent = ScenePrefabParent.transform;
            }
            else
            {
                Debug.LogWarning("Failed to find a valid position for a left building after maximum attempts.");
            }
        }
    }

    private void SpawnFlyover()
    {
        GameObject flyoverPrefab = prefabController.GetRandomFlyoverPrefab();

        if (flyoverPrefab != null)
        {
            Vector3 spawnPosition = spawnPoint.position;
            GameObject newFlyover = Instantiate(flyoverPrefab, spawnPosition, Quaternion.identity);

            newFlyover.GetComponent<BridgeMovement>().moveSpeed = SpeedValue;
        }
    }

    // Validate that the spawn position Z is not overlapping with recent positions
    private bool IsPositionValid(Vector3 newZPosition)
    {
        foreach (Vector3 recentZ in recentZPositions)
        {
            if (Mathf.Abs(recentZ.x - newZPosition.x) < 30 && Mathf.Abs(recentZ.z - newZPosition.z) < 40)
            {
                return false;
            }
        }
        return true;
    }

    // Add a new Z position to the list and remove the oldest if the list exceeds the size
    private void AddToRecentPositions(Vector3 newZPosition)
    {
        recentZPositions.Add(newZPosition);

        if (recentZPositions.Count > positionQueueSize)
        {
            recentZPositions.RemoveAt(0);
        }
    }
}
