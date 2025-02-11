using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MotionSicknessBool : MonoBehaviour
{
   public static MotionSicknessBool instance;
   public  bool motionSicknessEnabled = false;

    public void SetMotionSickness(bool value)
    {
        motionSicknessEnabled = value;
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

}
