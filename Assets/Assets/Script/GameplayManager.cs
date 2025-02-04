using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public int TutorialSemaphore = 0;
    public float Multiplier = 3.6f;
    public int BuildingSpeed = 10;
    public GameObject SignbannerPrefab;
    public SignBoard Signbanner;
    public int[] ActualSpeedlist = new int[] { 10, 15, 20, 25, 30 };
    Transform StartLoc;
    [SerializeField] Transform endpoint;
    public Dictionary<int, int> SpeedValues = new Dictionary<int, int>{
        { 25, 10 },
        { 40, 15 },
        { 45 ,20 },
        { 60, 25 },
        { 75, 30 },
    };
    public bool RightEyeBlock = false;
    int LoopCount = 0;
    public GameObject Result;
    public TMPro.TMP_Text ResultText;
    public TMPro.TMP_Text SpeedMeter;
    public KeyValuePair<int, int> selectedpair= new KeyValuePair<int, int>(0,0);
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
        endpoint = GameObject.Find("SignPoint").transform;
        //StartLoc.position = new Vector3(-767, 0.692149878f, -41.6f);
    }

    private void Update()
    {
        foreach (var item in SpeedValues)
        {
            if (item.Value == BuildingSpeed)
            {
                SpeedMeter.text = item.Key.ToString();
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BeginTutorial() {

        UIHandler.instance.updateLeftScreen(false);
        if(TutorialSemaphore !=-1)
        {
            StartCoroutine(TutorialLoop());
        }
        else if(TutorialSemaphore == -1)
        {
            StartGameCountDown();
        }
        
        
    }

    public IEnumerator TutorialLoop()
    {
        
        //Debug.LogError(TutorialSemaphore);
        VehicleSpeedHandler.instance.IsGivingAnswerAllowed = true;
        GameObject targetObject = GameObject.Find("Sign Board Ui");
        switch (TutorialSemaphore)
        {
            case 0:
                //EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[0]);
                yield return new WaitForSeconds(3f);
                
                if (targetObject != null) { Destroy(targetObject); }
                yield return new WaitForSeconds(2);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[1]);
                VehicleSpeedHandler.instance.Canvas.SetActive(true);
                SpawnBridge();
                UIHandler.instance.UpdateButton(1);
               
                break;
            case 1:
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[2]); // Start The Challenge
                yield return new WaitForSeconds(2);
                //EyeToggle.instance.UpdateEye(0);
                yield return new WaitForSeconds(2);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[3]);
                SpawnBridge();
                break;

            case 2:
                //EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[4]); // Start The Challenge
                yield return new WaitForSeconds(2);
                //EyeToggle.instance.UpdateEye(1);
                yield return new WaitForSeconds(2);
                SpawnBridge();
                break;

            default:
                Debug.LogError("OutOf Bound");
                break;
        }
    
    }
    public KeyValuePair<int,int> GetRandomNumberFromList(/*bool Display, bool Actualvalue*/)
    {
        
        // Get a list of keys from the dictionary
        List<int> keys = new List<int>(SpeedValues.Keys);

        // Generate a random index
        int randomIndex = UnityEngine.Random.Range(0, keys.Count);

        // Get the random key
        int randomKey = keys[randomIndex];
        return new KeyValuePair<int, int>(randomKey, SpeedValues[randomKey]);
        
    }

    public List<int> GetRandomFourElementList(int value)
    {
        List<int> newArr = new List<int>();
        int Val;
        while (newArr.Count != 4) {
            Val = GetRandomNumberFromList().Key;
            if (newArr.Contains(Val)) continue;
            newArr.Add(Val);
        }
        if (!newArr.Contains(selectedpair.Key))
            newArr[UnityEngine.Random.Range(0, newArr.Count)] = value;
        Debug.Log(newArr.ToString());
        return newArr;

    }

    public void SpawnBridge()
    {

        
        if (Signbanner) {  Destroy(Signbanner.gameObject);  }
        UIHandler.instance.FillProgress = 1;
        UIHandler.instance.hasInvoked = false;
        //obj.transform.localScale *= 2f;
        selectedpair = GetRandomNumberFromList();
        VehicleSpeedHandler.instance.SelectedSpeed = selectedpair.Value;
        GameObject obj = Instantiate(SignbannerPrefab, StartLoc.position, new Quaternion(-0.707106829f, 0, 0, 0.707106829f));
        TimerManager.instance.spawnTime=TimerManager.instance.GetCurrentTime();
        Signbanner = obj.GetComponent<SignBoard>();
        Signbanner.SetSpeed(selectedpair.Key);
        VehicleSpeedHandler.instance.SetButtonData(GetRandomFourElementList(selectedpair.Key));
        Debug.Log("Subscribed to SignBoard Event");
    }
    public float returntimebetweenspawnandfinish()
    {
        Vector3 startPosition = StartLoc.position; // Assuming StartLoc is a valid position
        Vector3 finishPosition =endpoint.position /* Provide the finish position here */;
        float distance = Vector3.Distance(startPosition, finishPosition);
        float time = distance / BuildingSpeed;
        return time;
    }

    
    public void StartGameCountDown()
    {
        StartCoroutine(GameCountdown());
    }

    public IEnumerator GameCountdown()
    {
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[6]);
        UIHandler.instance.playOneshotButton(UIHandler.instance.Countdown);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[7]);
        UIHandler.instance.playOneshotButton(UIHandler.instance.Countdown);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[8]);
        UIHandler.instance.playOneshotButton(UIHandler.instance.Countdown);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[9]);
        UIHandler.instance.playOneshotButton(UIHandler.instance.Countdown);
        yield return new WaitForSeconds(1);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[10]);
        UIHandler.instance.UpdateButton(1);
        UIHandler.instance.ButtonsList[1].transform.GetChild(0).gameObject.SetActive(false);
        UIHandler.instance.GameplayMusic.Play();
        TimerManager.instance.StartTimer();
        Debug.Log("Game timer Started");
        ActualGameLoop();
    }


    void ActualGameLoop()
    {
        Debug.Log("Time remaining: " + TimerManager.instance.gettimeremaining() + " Time Between Spawn and finsh: " + returntimebetweenspawnandfinish());
        if (TimerManager.instance.gettimeremaining() > returntimebetweenspawnandfinish())
        {
            VehicleSpeedHandler.instance.Canvas.SetActive(true);
            TimerManager.instance.Answered=false;
            //EyeToggle.instance.UpdateEye(RightEyeBlock ? 1 : 0);
            RightEyeBlock = !RightEyeBlock;
            
           
            
            SpawnBridge();
            
            
        }

        else
        {
            Result.SetActive(true);
            //EyeToggle.instance.UpdateEye(-1);
            VehicleSpeedHandler.instance.Canvas.SetActive(false);
            TimerManager.instance.CalculateResponseStats();
            // Add Total Score
            
        }
    }


    // Fix later
    public void OnSignBannerEnd(bool Correct)
    {
        
        if (Signbanner) { Destroy(Signbanner.gameObject); }
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
                //EyeToggle.instance.UpdateEye(-1);
                UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[5]);
                VehicleSpeedHandler.instance.Canvas.SetActive(false);
                UIHandler.instance.UpdateButton(2);

            }
        }

        else
        {
            

            //EyeToggle.instance.UpdateEye(-1);
            VehicleSpeedHandler.instance.Canvas.SetActive(false);
            StartCoroutine(ExecuteWithDelay(()=>ActualGameLoop(), 1f));
            
            
        }


    }

    IEnumerator ExecuteWithDelay(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action.Invoke();
    }
    //IEnumerator BlinkButtons()
    //{
    //    int blinkCount = 3; // Number of times to blink
    //    float blinkDuration = 0.5f; // Duration of each blink
        
    //    for (int i = 0; i < blinkCount; i++)
    //    {
    //        // Set all buttons to red
    //        foreach (var button in VehicleSpeedHandler.instance.Buttons)
    //        {
    //            button.GetComponent<Button>().image.color = Color.red;
    //            Debug.LogError("color changed to red");
    //        }
    //        yield return new WaitForSeconds(blinkDuration);

    //        // Reset buttons to their original color
    //        foreach (var button in VehicleSpeedHandler.instance.Buttons)
    //        {
    //            button.GetComponent<Button>().image.color = Color.white;
    //            Debug.LogError("color changed to white");
    //        }
    //        yield return new WaitForSeconds(blinkDuration);
    //    }
    //}

    //public void BlinkAnswerButtons()
    //{
    //    StartCoroutine(BlinkButtons());
    //}
}
