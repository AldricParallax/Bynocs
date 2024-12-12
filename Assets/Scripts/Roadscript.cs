using System.Collections;
using UnityEngine;

public class Roadscript : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f; // Speed at which the road moves
    [SerializeField] private float lifespan = 5f; // Time before the road deletes itself
    [SerializeField] Transform endpoint;
    public void SetLifespan(float time)
    {
        lifespan = time;
    }

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        // Move the road constantly in the forward direction
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        //check position of road and destroy
        if(transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }
    }
   

    
}
