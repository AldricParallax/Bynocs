using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIHandler : MonoBehaviour
{

    public static UIHandler instance;
    Material Centralmat;


    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Centralmat = GetComponent<MeshRenderer>().materials[1];
    }

    public void UpdateScreen(int ScreenValue)
    {
       
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
