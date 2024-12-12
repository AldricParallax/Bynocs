using System.Collections;
using UnityEngine;



public class RoadSpawner : MonoBehaviour
{
    [Header("Road Settings")]
    [SerializeField] private GameObject roadPrefab; // Prefab to spawn
    [SerializeField] private Transform spawnPoint; // Where the road will be spawned

    [Header("Spawn Timing")]
    [SerializeField] private float spawnInterval = 5f; // Time (in seconds) between spawns

    private Coroutine spawnCoroutine;

    private void Start()
    {
        // Start spawning roads at regular intervals
        StartSpawning();
    }

    /// <summary>
    /// Starts the coroutine to spawn roads at the defined interval.
    /// </summary>
    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoads());
        }
    }

    /// <summary>
    /// Stops the coroutine to prevent further road spawns.
    /// </summary>
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    /// <summary>
    /// Spawns the road prefab at the specified interval.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnRoads()
    {
        while (true) // Infinite loop to keep spawning
        {
            SpawnRoad();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    /// <summary>
    /// Instantiates a new road prefab at the specified position and rotation.
    /// </summary>
    private void SpawnRoad()
    {
        if (roadPrefab != null && spawnPoint != null)
        {
            Instantiate(roadPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogWarning("Road prefab or spawn point is not set!");
        }
    }

    /// <summary>
    /// Sets a new spawn interval dynamically.
    /// </summary>
    /// <param name="newInterval">The new interval in seconds.</param>
    public void SetSpawnInterval(float newInterval)
    {
        spawnInterval = newInterval;
        StopSpawning();
        StartSpawning();
    }
}
