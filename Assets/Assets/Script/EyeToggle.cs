using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeToggle : MonoBehaviour
{
    public static EyeToggle instance;
    Material mat;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }



    void Start()
    {
        mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    public void UpdateEye(int bLeftEyeEnable)
    {
        switch (bLeftEyeEnable)
        {
            case 1:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 1));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 0));
                break;
            case 0:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 0));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 1));
                break;
            default:
                mat.SetColor("_LeftEyeColor", new Color(0, 0, 0, 0));
                mat.SetColor("_RightEyeColor", new Color(0, 0, 0, 0));
                break;
        }
    }

}
