using UnityEngine;
    
public class PrefabScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;

    public bool isMovingUp = false;

    private void Start()
    {
        // Start moving down by default
        MoveDown();
    }

    private void FixedUpdate()
    {
        moveSpeed=FindAnyObjectByType<SpeedLimitScript>().speedLimit;
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
