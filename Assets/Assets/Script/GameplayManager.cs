using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        StartLoc = GameObject.Find("SpawnPoint").transform;
        StartLoc.position = new Vector3(-767, 0.692149878f, -41.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginTutorial() {
        
        StartCoroutine(TutorialLoop());
    }

    IEnumerator TutorialLoop()
    {
        switch (TutorialSemaphore)
        {
            case 0:
                EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[0]);
                yield return new WaitForSeconds(2);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[1]);
                VehicleSpeedHandler.instance.Canvas.SetActive(true);
                SpawnBridge();
                yield return new WaitForSeconds(2);
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

        int randomIndex = Random.Range(0, SpeedValues.Count);
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
            newArr[Random.Range(0, newArr.Count)] = value;

        return newArr;

    }

    public void SpawnBridge()
    {
        if (Signbanner) Destroy(Signbanner.gameObject);

        GameObject obj = Instantiate(SignbannerPrefab, StartLoc.position,new Quaternion(-0.707106829f, 0, 0, 0.707106829f));
        Signbanner = obj.GetComponent<SignBoard>();
        
        VehicleSpeedHandler.instance.SelectedSpeed = GetRandomNumberFromList();
        Signbanner.SetSpeed(VehicleSpeedHandler.instance.SelectedSpeed);
        VehicleSpeedHandler.instance.SetButtonData(GetRandomFourElementList(VehicleSpeedHandler.instance.SelectedSpeed));
    }

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

        }
        else
        {
            // Infinite End
        }

    }

}
