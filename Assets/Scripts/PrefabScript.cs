using UnityEngine;
    
public class PrefabScript: MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Speed at which the road moves
    [SerializeField] Transform endpoint;
    

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        // Move the road constantly in the forward direction
        transform.Translate(Vector3.down * moveSpeed * Time.fixedDeltaTime);

        //check position of road and destroy
        if (transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }
    }

}
