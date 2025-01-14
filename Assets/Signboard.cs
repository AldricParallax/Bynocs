using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Signboard : MonoBehaviour
{
    [SerializeField] private float speedLimit;
    public Transform endpoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedLimit= GameplayManager.instance.BuildingSpeed;
        transform.Translate(Vector3.back * speedLimit * Time.deltaTime);
        if (transform.position.x > endpoint.position.x)
        {
            Destroy(gameObject);
        }
    }
}
