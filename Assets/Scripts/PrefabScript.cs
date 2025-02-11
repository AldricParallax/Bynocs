using System.Collections;
using UnityEngine;
    
public class PrefabScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;
    [SerializeField] private MeshRenderer[] meshlist;
    public bool isMovingUp = false;
    public bool Isvisible = false;
    // Array to store child rigidbodies

    private void Start()
    {
        if (previousMotionSicknessValue != MotionSicknessBool.instance.motionSicknessEnabled)
        {
            previousMotionSicknessValue = MotionSicknessBool.instance.motionSicknessEnabled;
            Debug.Log("Value changed to " + previousMotionSicknessValue);
            // Pass the bool value to the disablemesh coroutine
            StartCoroutine(disablemesh(previousMotionSicknessValue));
        }
        // Start moving down by default
        MoveDown();
        GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        
        
        // Get all child rigidbodies

    }
    IEnumerator disablemesh(bool Isvisible)
    {
        
        if (Isvisible == true)
        {
            foreach (MeshRenderer mesh in meshlist)
            {
                mesh.enabled = false;
            }
        }
        else
        {
           foreach (MeshRenderer mesh in meshlist)
            {
                mesh.enabled = true;
            }
        }
        yield return null;
    }
    private bool previousMotionSicknessValue = false;
    private void FixedUpdate()
    {
        if (previousMotionSicknessValue != MotionSicknessBool.instance.motionSicknessEnabled)
        {
            previousMotionSicknessValue = MotionSicknessBool.instance.motionSicknessEnabled;
            Debug.Log("Value changed to " + previousMotionSicknessValue);
            // Pass the bool value to the disablemesh coroutine
            StartCoroutine(disablemesh(previousMotionSicknessValue));
        }

        moveSpeed = GameplayManager.instance.BuildingSpeed * GameplayManager.instance.Multiplier;

        if (isMovingUp)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
        // Check if the value of motionsicknessbool has changed
       

        // Check position of road and destroy
        if (transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }

        // Freeze position of child rigidbodies after 5 seconds
        if (Time.time >= 10f)
        {
            GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            
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
