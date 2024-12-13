using System.Collections;
using UnityEngine;



public class RoadSpawner : MonoBehaviour
{
    [Header("Road Settings")]
    [SerializeField] private GameObject roadPrefab; // Prefab to spawn
    [SerializeField] private Transform spawnPoint; // Where the road will be spawned

    [Header("Spawn Tracking")]
    [SerializeField] private Transform trackingObject; // Object to track for position-based spawn trigger
    [SerializeField] Transform trackpoint; // Z position at which to spawn a new road
    [SerializeField] GameObject Road;
    [SerializeField] GameObject Road1;
    private GameObject currentTrackingRoad; // Current road being tracked
    public float Roadspeed=40; // Speed at which the road moves
    private void Start()
    {
        
        Road.GetComponent<Roadscript>().moveSpeed =Roadspeed;
        Road1.GetComponent<Roadscript>().moveSpeed = Roadspeed;
        // Spawn the initial road and set it as the tracking object
        //SpawnInitialRoad();
    }

    /// <summary>
    /// Spawns the initial road and sets it as the tracking object.
    /// </summary>
    //private void SpawnInitialRoad()
    //{
    //    currentTrackingRoad = Instantiate(roadPrefab, spawnPoint.position, spawnPoint.rotation);
    //    trackingObject = currentTrackingRoad.transform;
    //}

    private void Update()
    {
        
        TrackRoadPosition();
    }

    /// <summary>
    /// Tracks the position of the current tracking object and spawns a new road when it crosses the trigger position.
    /// </summary>
    private void TrackRoadPosition()
    {
        if (trackingObject != null && trackingObject.transform.position.x >= trackpoint.transform.position.x)
        {
            SpawnRoad();
        }
    }

    /// <summary>
    /// Instantiates a new road prefab at the specified position and rotation.
    /// The new road becomes the new tracking object.
    /// </summary>
    private void SpawnRoad()
    {
        if (roadPrefab != null && spawnPoint != null)
        {
            Vector3 newSpawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, trackingObject.position.z);
            currentTrackingRoad = Instantiate(roadPrefab, newSpawnPosition, spawnPoint.rotation);
            trackingObject = currentTrackingRoad.transform;
            currentTrackingRoad.GetComponent<Roadscript>().moveSpeed = Roadspeed;
        }
        else
        {
            Debug.LogWarning("Road prefab or spawn point is not set!");
        }
    }
}
