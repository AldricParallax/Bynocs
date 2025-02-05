using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;



public class UIHandler : MonoBehaviour
{

    public static UIHandler instance;
    Material Centralmat;
    Material LeftMAt;
    public List<GameObject> ButtonsList = new List<GameObject>();
    public List<Texture> IntroImages = new List<Texture>();
    public List<Texture> TutorialImages = new List<Texture>();
    public List<Texture> LeftScreenTexture = new List<Texture>();
    public float FillProgress = 1f;
    public Image Fill;
    public event Action OnSignBoardExit;
    [SerializeField] GameObject Settings_canvas;
    [SerializeField] public Image Settings_Screen;
    [SerializeField] GameObject prefabparent;
    [SerializeField] public TextMeshProUGUI[] SettingsText;
    public int bannertextFont=36;
    public AudioSource SFX;
    public AudioSource GameplayMusic;
    [SerializeField] private AudioClip ButtonCLick;
    [SerializeField] private AudioClip Correct;
    [SerializeField] private AudioClip Wrong;
    [SerializeField] private AudioClip Page;
    [SerializeField] private AudioClip SpeedChange;
    [SerializeField] public AudioClip Countdown;
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
        SFX.PlayOneShot(Page);
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

            case 1:
                UpdateButton(0);
                UpdateCenterScreen(IntroImages[2]);
                break;

            default:
                break;
        }
    }
    public void updateLeftScreen(bool Mainmenu)
    {

        LeftMAt = GetComponent<MeshRenderer>().materials[2];
        if (Mainmenu)
        {
            LeftMAt.SetTexture("_MainTex", LeftScreenTexture[0]);
            LeftMAt.SetTexture("_EmissionMap", LeftScreenTexture[0]);
        }
        else {
            LeftMAt.SetTexture("_MainTex", LeftScreenTexture[1]);
            LeftMAt.SetTexture("_EmissionMap", LeftScreenTexture[1]);
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
        
        if (!hasInvoked && FillProgress <= 0.01f)
        {
            //UIHandler.instance.playOneshotAnswerButton(false);
            hasInvoked = true;
        }
    }
    public void EnableSettings()
    {
        
        UpdateCenterScreen(Settings_Screen.mainTexture);
        Settings_canvas.SetActive(true);
        Time.timeScale = 0;

    }
    public void DisableSettings()
    {
        Time.timeScale = 1;
        Settings_canvas.SetActive(false);
        UpdateCenterScreen(IntroImages[2]);
    }
    public void MotionSicknessAssist(bool enable)
    {
        if (enable)
        {

            prefabparent.SetActive(false);
            SettingsText[2].text = "Enabled";
        }
        else
        {
            prefabparent.SetActive(true);
            SettingsText[2].text = "Disabled";
        }
    }
   
    void SetShaderAlpha(float alpha)
    {
        // Replace "YourShaderName" with the actual name of your shader
        Material shaderMaterial = GetComponent<MeshRenderer>().materials[0];
        shaderMaterial.SetFloat("_Alpha", alpha);
    }
   
    public void EXittoMainMenu()
    {
        EyeToggle.instance.StartEyeFadeLoop(1, 0f, 1f, 1.5f); // Left Eye fades in & out
        EyeToggle.instance.StartEyeFadeLoop(0, 0f, 1f, 1.5f); // Right Eye fades in & out
        GameplayManager.instance.StopAllCoroutines();
        TimerManager.instance.ResetTimer();
        TimerManager.instance.responseRecords.Clear();
        //StopAllCoroutines();
        //StopCoroutine(GameplayManager.instance.TutorialLoop());
        //StopCoroutine(GameplayManager.instance.GameCountdown());
        GameObject targetObject = GameObject.Find("Sign Board Ui");
        if(targetObject != null) { Destroy(targetObject); }
        if (GameplayManager.instance.Signbanner != null)
        {
            Destroy(GameplayManager.instance.Signbanner.gameObject);
        }
       
        updateLeftScreen(true);
        VehicleSpeedHandler.instance.Canvas.SetActive(false);
        UpdateScreen(1);
        GameplayMusic.Stop();

    }
    public void ScaleSetting(bool increase)
    {
        if (increase)
        {
            ////GameplayManager.instance.SignbannerPrefab.transform.localScale= new Vector3(0.00593353016f, 0.00720383693f, 0.00593353016f);
            //GameplayManager.instance.SignbannerPrefab.GetComponent<SignBoard>().speedText.fontSize=50;
            //GameplayManager.instance.SignbannerPrefab.GetComponent<SignBoard>().speedText.alignment=TextAlignmentOptions.Top;
            SettingsText[1].text = "Large";
            bannertextFont = 50;
        }
        else
        {
            //GameplayManager.instance.SignbannerPrefab.transform.localScale = new Vector3(0.00593353016f, 0.00702470634f, 0.00593353016f);
            //GameplayManager.instance.SignbannerPrefab.GetComponent<SignBoard>().speedText.fontSize = 36;
            //GameplayManager.instance.SignbannerPrefab.GetComponent<SignBoard>().speedText.alignment = TextAlignmentOptions.Midline;
            SettingsText[1].text = "Standard";
            bannertextFont = 36;
        }
    }

    public void playOneshotButton(AudioClip ButtonCLick)
    {
        SFX.PlayOneShot(ButtonCLick);
    }
    public void playOneshotAnswerButton(bool correct)
    {
        if(correct)
        {
            SFX.PlayOneShot(Correct);
        }
        else
            {
            SFX.PlayOneShot(Wrong);
        }
        
    }
    
}
