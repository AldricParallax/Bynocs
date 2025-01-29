using UnityEngine;
    
public class PrefabScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;

    public bool isMovingUp = false;

    // Array to store child rigidbodies

    private void Start()
    {
        // Start moving down by default
        MoveDown();

        // Get all child rigidbodies
        
    }

    private void FixedUpdate()
    {
        moveSpeed = GameplayManager.instance.BuildingSpeed * GameplayManager.instance.Multiplier;

        if (isMovingUp)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

        // Check position of road and destroy
        if (transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }

        // Freeze position of child rigidbodies after 5 seconds
        if (Time.time >= 5f)
        {
            GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.fixedDeltaTime);
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveSpeed * Time.fixedDeltaTime);
    }

    public void SwitchMovement()
    {
        isMovingUp = !isMovingUp;
    }
}
