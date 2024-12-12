using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    [Header("Prefab Lists")]
    [SerializeField] private List<GameObject> buildingPrefabs; // List of building prefabs
    [SerializeField] private List<GameObject> flyoverPrefabs; // List of flyover prefabs

    /// <summary>
    /// Returns a random building prefab from the list.
    /// </summary>
    public GameObject GetRandomBuildingPrefab()
    {
        if (buildingPrefabs.Count > 0)
        {
            return buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];
        }
        else
        {
            Debug.LogWarning("No building prefabs available.");
            return null;
        }
    }

    /// <summary>
    /// Returns a random flyover prefab from the list.
    /// </summary>
    public GameObject GetRandomFlyoverPrefab()
    {
        if (flyoverPrefabs.Count > 0)
        {
            return flyoverPrefabs[Random.Range(0, flyoverPrefabs.Count)];
        }
        else
        {
            Debug.LogWarning("No flyover prefabs available.");
            return null;
        }
    }
}

