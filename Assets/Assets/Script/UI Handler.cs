using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;


public class UIHandler : MonoBehaviour
{

    public static UIHandler instance;
    Material Centralmat;
    public List<GameObject> ButtonsList = new List<GameObject>();
    public List<Texture> IntroImages = new List<Texture>();
    public List<Texture> TutorialImages = new List<Texture>();
    public float FillProgress = 1f;
    public Image Fill;
    public event Action OnSignBoardExit;


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
        //Fill = GameObject.Find("ProgressBar").GetComponent<Image>();


    }





    public void UpdateCenterScreen(Texture Value)
    {




        if(!Centralmat) Centralmat = GetComponent<MeshRenderer>().materials[1];
        Centralmat.SetTexture("_MainTex", Value);
        Centralmat.SetTexture("_EmissionMap", Value);

    }

    public void UpdateButton(int Value)
    {
        for (int i = 0; i < ButtonsList.Count; i++)
        {
            ButtonsList[i].SetActive(i == Value);
        }

    }


    public void UpdateScreen(int ScreenValue)
    {
        StartCoroutine(UpdateData(ScreenValue));
    }

    IEnumerator UpdateData(int ScreenValue)
    {

        switch (ScreenValue)
        {
            case 0:
                UpdateCenterScreen(IntroImages[0]);
                yield return new WaitForSeconds(2);
                UpdateCenterScreen(IntroImages[1]);
                yield return new WaitForSeconds(2);
                UpdateButton(0);
                UpdateCenterScreen(IntroImages[2]);
                //GameplayManager.instance.SpawnBridge();
                break;
                

            default:
                break;
        }
    }

    IEnumerator BlinkButtons()
    {
        int blinkCount = 3; // Number of times to blink
        float blinkDuration = 0.5f; // Duration of each blink

        for (int i = 0; i < blinkCount; i++)
        {
            // Set all buttons to red
            foreach (var button in ButtonsList)
            {
                button.GetComponent<Button>().colors = new ColorBlock
                {
                    normalColor = Color.red,

                };
            }
            yield return new WaitForSeconds(blinkDuration);

            // Reset buttons to their original color
            foreach (var button in ButtonsList)
            {
                button.GetComponent<Button>().colors = new ColorBlock
                {
                    normalColor = Color.white,

                };
            }
            yield return new WaitForSeconds(blinkDuration);
        }
    }
    int i = 0;
    public bool hasInvoked = false;
    // Update is called once per frame
    void  Update()
    {
        
        if (Fill.gameObject.activeInHierarchy)
        {
            Fill.fillAmount = GameplayManager.instance.Signbanner ? 1f - FillProgress : 0f;
            
        }
        // Check if FillProgress is approximately 1 (with a small tolerance)
        //float tolerance = 0.00002f;
        // Trigger the event when FillProgress drops below the threshold
        if (!hasInvoked && FillProgress <= 0.05f)
        {
            Debug.Log("Invoking event, FillProgress = " + FillProgress);
            OnSignBoardExit?.Invoke();
            hasInvoked = true;
        }
    }

}
