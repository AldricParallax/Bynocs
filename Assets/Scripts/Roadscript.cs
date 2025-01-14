using System.Collections;
using UnityEngine;

public class Roadscript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;
    
    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        moveSpeed = GameplayManager.instance.BuildingSpeed;
        // Move the road constantly in the forward direction
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        //check position of road and destroy
        if(transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }
    }
   

    
}
