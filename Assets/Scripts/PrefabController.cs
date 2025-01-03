using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabController : MonoBehaviour
{
    [Header("Prefab Lists")]
    public List<GameObject> buildingPrefabs;
    private Queue<int> recentlyUsedIndices = new Queue<int>(); // Tracks recently used indices
    private int maxRecentCount = 5; // Maximum number of recently used indices to track
    [SerializeField] private List<GameObject> flyoverPrefabs; // List of flyover prefabs

    /// <summary>
    /// Returns a random building prefab from the list.
    /// </summary>
    public GameObject GetRandomBuildingPrefab()
    {
        if (buildingPrefabs.Count > 0)
        {
            int randomIndex;

            // Ensure the random index is not recently used
            do
            {
                randomIndex = Random.Range(0, buildingPrefabs.Count);
            } while (recentlyUsedIndices.Contains(randomIndex) && recentlyUsedIndices.Count < buildingPrefabs.Count);

            // Update the queue of recently used indices
            recentlyUsedIndices.Enqueue(randomIndex);
            if (recentlyUsedIndices.Count > maxRecentCount)
            {
                recentlyUsedIndices.Dequeue();
            }

            return buildingPrefabs[randomIndex];
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

