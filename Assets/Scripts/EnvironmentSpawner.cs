using System.Collections;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private Transform spawnPoint; // Position where new objects will be spawned
    [SerializeField] private float groupSpacing = 10f; // Distance between building groups
    [SerializeField] private int minBuildingsPerGroup = 3; // Minimum number of buildings in a group
    [SerializeField] private int maxBuildingsPerGroup = 6; // Maximum number of buildings in a group
    [SerializeField] private int minRightOffset = 20; // Minimum offset to the right of the spawn point
    [SerializeField] private int maxRightOffset = 60; // Maximum offset to the right of the spawn point
    [SerializeField] private int minLeftOffset = -40; // Minimum offset forward from the spawn point
    [SerializeField] private int maxLeftOffset = -80; // Maximum offset forward from the spawn point
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

   
    IEnumerator spawndelay()
    {
        while (true)
        {
           
            DecideNextSpawn();
            int random = Random.Range(0, 5);
            yield return new WaitForSeconds(random);
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
            //equal chance to spawn buildings on left or right
            float randomValue2 = Random.value;
            if (randomValue2 < 0.5f)
            {
                Spawnbuildingsright();
            }
            else
            {
                SpawnbuildingsLeft();
            }
        }
        else
        {
            //SpawnFlyover();
        }
    }
    private void Spawnbuildingsright() 
    {
        
       
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 90, 0);
        if (buildingPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z + Random.Range(minRightOffset,maxRightOffset));
            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
            newBuilding.GetComponent<PrefabScript>().moveSpeed = 10;


        }
        
        
    }
    private void SpawnbuildingsLeft() 
    {
        
       
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 90, 0);
        if (buildingPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z + Random.Range(minLeftOffset,maxLeftOffset));
            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
            newBuilding.GetComponent<PrefabScript>().moveSpeed = 10;


        }
        
        
    }
    
    private void SpawnFlyover()
    {
        GameObject flyoverPrefab = prefabController.GetRandomFlyoverPrefab();

        Quaternion newQuaternion = Quaternion.Euler(-60, -90, -90);
        if (flyoverPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(0, spawnPoint.position.y,spawnPoint.position.z + groupSpacing);
            GameObject newFlyover = Instantiate(flyoverPrefab, spawnPosition, newQuaternion);

            currentTrackingObject = newFlyover.transform;
        }
    }
}