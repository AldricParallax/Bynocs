using UnityEngine;

public class BridgeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;

    private void FixedUpdate()
    {
        moveSpeed = GameplayManager.instance.BuildingSpeed * GameplayManager.instance.Multiplier;
        // Move the road constantly in the forward direction
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);

        //check position of road and destroy
        if (transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }
    }
}
