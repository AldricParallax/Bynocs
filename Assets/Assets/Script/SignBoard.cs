using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.Mathematics;

public class SignBoard : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed; // Speed at which the road moves
    [SerializeField] Transform endpoint;
    [SerializeField] Transform RearCarpoint;
     // Event to notify when the signboard exits
    Vector3 StartLoc;
    public bool isMovingUp = true;

    private void Start()
    {
        endpoint = GameObject.Find("EndPoint").transform;
        RearCarpoint = GameObject.Find("RearPoint").transform;

        // Start moving down by default
        MoveDown();
        StartLoc = transform.position;
    }

    public void SetSpeed(int speed)
    {
        transform.GetChild(0).GetComponent<TMP_Text>().text = speed.ToString();
    }

    private void FixedUpdate()
    {
        UIHandler.instance.FillProgress = InverseLerp(endpoint.position, StartLoc, transform.position);
        moveSpeed = GameplayManager.instance.BuildingSpeed* GameplayManager.instance.Multiplier;
        if (isMovingUp)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

        // Check position of road and destroy
        if (transform.position.x > endpoint.position.x || (transform.position.x > RearCarpoint.position.x && VehicleSpeedHandler.instance.IsGivingAnswerAllowed))
        {
            bool isCorrect = (VehicleSpeedHandler.instance.SelectedSpeed == GameplayManager.instance.BuildingSpeed);
            GameplayManager.instance.OnSignBannerEnd(isCorrect);
            
           
            
            Destroy(gameObject);
        }
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }


    private void MoveUp()
    {
        transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime);
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
