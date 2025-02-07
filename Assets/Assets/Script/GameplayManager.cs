using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public int TutorialSemaphore = 0;
    public float Multiplier = 3.6f;
    public float BuildingSpeed = 10f;
    public GameObject SignbannerPrefab;
    public SignBoard Signbanner;
    Transform StartLoc;
    List<int> SpeedValues = new List<int> { 25, 40, 45, 60, 75 };
    bool RightEyeBlock = false;
    int LoopCount = 0;
    int Score = 0;
    public GameObject Result;
    public TMPro.TMP_Text ResultText;
    public TMPro.TMP_Text SpeedMeter;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }



    void Start()
    {
        UIHandler.instance.UpdateButton(-1);
        UIHandler.instance.UpdateScreen(0);
        EyeToggle.instance.UpdateEye(-1);
        StartLoc = GameObject.Find("SignSpawnPoint").transform;
        //StartLoc.position = new Vector3(-767, 0.692149878f, -41.6f);
    }

    private void Update()
    {
        SpeedMeter.text = BuildingSpeed.ToString();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BeginTutorial() {
        
        StartCoroutine(TutorialLoop());
    }

    IEnumerator TutorialLoop()
    {
        Debug.LogError(TutorialSemaphore);
        switch (TutorialSemaphore)
        {
            case 0:
                EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[0]);
                yield return new WaitForSeconds(2);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[1]);
                VehicleSpeedHandler.instance.Canvas.SetActive(true);
                SpawnBridge();
                UIHandler.instance.UpdateButton(1);
               
                break;
            case 1:
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[2]); // Start The Challenge
                yield return new WaitForSeconds(2);
                EyeToggle.instance.UpdateEye(0);
                yield return new WaitForSeconds(2);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[3]);
                SpawnBridge();
                break;

            case 2:
                EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[4]); // Start The Challenge
                yield return new WaitForSeconds(2);
                EyeToggle.instance.UpdateEye(1);
                yield return new WaitForSeconds(2);
                SpawnBridge();
                break;

            default:
                Debug.LogError("OutOf Bound");
                break;
        }
    
    }

    public int GetRandomNumberFromList()
    {

        int randomIndex = UnityEngine.Random.Range(0, SpeedValues.Count);
        return SpeedValues[randomIndex];
    }

    public List<int> GetRandomFourElementList(int value)
    {
        List<int> newArr = new List<int>();
        int Val;
        while (newArr.Count != 4) {
            Val = GetRandomNumberFromList();
            if (newArr.Contains(Val)) continue;
            newArr.Add(Val);
        }
        if (!newArr.Contains(value))
            newArr[UnityEngine.Random.Range(0, newArr.Count)] = value;

        return newArr;

    }

    public void SpawnBridge()
    {
        if (Signbanner) Destroy(Signbanner.gameObject);

        GameObject obj = Instantiate(SignbannerPrefab, StartLoc.position,new Quaternion(-0.707106829f, 0, 0, 0.707106829f));
        Signbanner = obj.GetComponent<SignBoard>();
        obj.transform.localScale *= 2.5f;
        VehicleSpeedHandler.instance.SelectedSpeed = GetRandomNumberFromList();
        Signbanner.SetSpeed(VehicleSpeedHandler.instance.SelectedSpeed);
        VehicleSpeedHandler.instance.SetButtonData(GetRandomFourElementList(VehicleSpeedHandler.instance.SelectedSpeed));
    }

    public void StartGameCountDown()
    {
        StartCoroutine(GameCountdown());
    }

    IEnumerator GameCountdown()
    {
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[6]);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[7]);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[8]);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[9]);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[10]);
        UIHandler.instance.UpdateButton(1);
        UIHandler.instance.ButtonsList[1].transform.GetChild(0).gameObject.SetActive(false);
        ActualGameLoop();
    }


    void ActualGameLoop()
    {
        if (LoopCount != 10)
        {
            VehicleSpeedHandler.instance.Canvas.SetActive(true);
            LoopCount++;
            EyeToggle.instance.UpdateEye(RightEyeBlock ? 1 : 0);
            RightEyeBlock = !RightEyeBlock;
            SpawnBridge();
        }

        else
        {
            EyeToggle.instance.UpdateEye(-1);
            VehicleSpeedHandler.instance.Canvas.SetActive(false);
            // Add Total Score
            Result.SetActive(true);
            ResultText.text = (Score == 10) ? Score.ToString() : "0" + Score.ToString();
        }
    }


    // Fix later
    public void OnSignBannerEnd(bool Correct)
    {
        if (TutorialSemaphore != -1)
        {
            if (Correct) { 
                TutorialSemaphore++;
                if (TutorialSemaphore == 3) TutorialSemaphore = -1; // Last Event goes to -1
            }
            if (TutorialSemaphore != -1)
                BeginTutorial();
            if(TutorialSemaphore == -1)
            {
                EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[5]);
                VehicleSpeedHandler.instance.Canvas.SetActive(false);
                UIHandler.instance.UpdateButton(2);

            }
        }

        else
        {
            EyeToggle.instance.UpdateEye(-1);
            VehicleSpeedHandler.instance.Canvas.SetActive(false);
            StartCoroutine(ExecuteWithDelay(()=>ActualGameLoop(), 3f));
            if (Correct)
            { Score++; }
        }


    }

    IEnumerator ExecuteWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
}
