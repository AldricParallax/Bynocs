using System.Collections;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint; // Position where new objects will be spawned
    [SerializeField] private float spawnTriggerZPosition = -10f; // Position where a new spawn is triggered
    [SerializeField] private float groupSpacing = 10f; // Distance between building groups
    [SerializeField] private int minBuildingsPerGroup = 3; // Minimum number of buildings in a group
    [SerializeField] private int maxBuildingsPerGroup = 6; // Maximum number of buildings in a group

    [Header("References")]
    [SerializeField] private PrefabController prefabController; // Reference to the PrefabController

    private Transform currentTrackingObject; // Tracks the current prefab to know when to spawn the next one

    private void Start()
    {
        StartCoroutine(spawndelay());
    }

    private void Update()
    {
        
    }

    /// <summary>
    /// Spawns the initial group and sets it as the tracking object.
    /// </summary>
    //private void SpawnInitialGroup()
    //{
    //    SpawnBuildingGroup();
    //}

    /// <summary>
    /// Tracks the position of the current tracking object to decide when to spawn the next group.
    /// </summary>
   
    IEnumerator spawndelay()
    {
        while (true)
        {
           
            DecideNextSpawn();
            yield return new WaitForSeconds(10);
        }
    }

    /// <summary>
    /// Decides whether to spawn a building group or a flyover.
    /// </summary>
    private void DecideNextSpawn()
    {
        // 70% chance to spawn buildings, 30% chance to spawn a flyover
        float randomValue = Random.value;
        if (randomValue < 0.7f)
        {
            Spawnbuildings();
        }
        else
        {
            SpawnFlyover();
        }
    }
    private void Spawnbuildings() 
    {
        int random = Random.Range(0, 3);
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        if (buildingPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0, spawnPoint.position.y, spawnPoint.position.z + groupSpacing);
            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition,transform.rotation);

            
        }
    }
    /// <summary>
    /// Spawns a group of buildings and sets the last building as the new tracking object.
    /// </summary>
    //private void SpawnBuildingGroup()
    //{
    //    int buildingCount = Random.Range(minBuildingsPerGroup, maxBuildingsPerGroup + 1);
    //    Vector3 spawnPosition = spawnPoint.position;

    //    for (int i = 0; i < buildingCount; i++)
    //    {
    //        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
    //        if (buildingPrefab != null)
    //        {
    //            Vector3 positionOffset = new Vector3(Random.Range(-5f, 5f), 0, i * groupSpacing);
    //            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition + positionOffset, Quaternion.identity);

    //            if (i == buildingCount - 1) // Last building in the group
    //            {
    //                currentTrackingObject = newBuilding.transform;
    //            }
    //        }
    //    }
    //}

    /// <summary>
    /// Spawns a flyover and sets it as the new tracking object.
    /// </summary>
    private void SpawnFlyover()
    {
        GameObject flyoverPrefab = prefabController.GetRandomFlyoverPrefab();
        if (flyoverPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0, spawnPoint.position.y,spawnPoint.position.z + groupSpacing);
            GameObject newFlyover = Instantiate(flyoverPrefab, spawnPosition, Quaternion.identity);

            currentTrackingObject = newFlyover.transform;
        }
    }
}
