using System.Collections;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{

    [Header("Spawn Settings")]
    public float SpeedValue;
    [SerializeField] RoadSpawner RoadSpawner;
    [SerializeField] private Transform spawnPoint; // Position where new objects will be spawned
    [SerializeField] private int minRightOffset = 20; // Minimum offset to the right of the spawn point
    [SerializeField] private int maxRightOffset = 60; // Maximum offset to the right of the spawn point
    [SerializeField] private int minLeftOffset = -40; // Minimum offset forward from the spawn point
    [SerializeField] private int maxLeftOffset = -80; // Maximum offset forward from the spawn point
    [Header("References")]
    [SerializeField] private PrefabController prefabController; // Reference to the PrefabController



    private void Start()
    {

        StartCoroutine(spawndelay());
    }
    private void Update()
    {
        SpeedValue = RoadSpawner.Roadspeed;
    }
    IEnumerator spawndelay()
    {
        while (true)
        {

            float randomValue = Random.value;
            Debug.Log(randomValue);
            if (randomValue < 0.97f)
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
                Debug.Log("Bridge random value: " + randomValue);
                yield return new WaitForSeconds(2);
                SpawnFlyover();
                yield return new WaitForSeconds(2);
            }
            int random = Random.Range(1, 3);
            yield return new WaitForSeconds(random);
        }
    }
    private void Spawnbuildingsright()
    {
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 90, 0);
        if (buildingPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 10, spawnPoint.position.z + Random.Range(minRightOffset, maxRightOffset));
            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
            newBuilding.GetComponent<PrefabScript>().moveSpeed = SpeedValue;
        }
    }
    private void SpawnbuildingsLeft()
    {
        GameObject buildingPrefab = prefabController.GetRandomBuildingPrefab();
        Quaternion newQuaternion = Quaternion.Euler(-90, 0, -90);

        if (buildingPrefab != null)
        {
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y - 10, spawnPoint.position.z + Random.Range(minLeftOffset, maxLeftOffset));
            GameObject newBuilding = Instantiate(buildingPrefab, spawnPosition, newQuaternion);
            newBuilding.GetComponent<PrefabScript>().moveSpeed = SpeedValue;
            newBuilding.GetComponent<PrefabScript>().isMovingUp = true;
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
}
