using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ResponseData
{
    public float spawnTime;
    public float responseTime;
    public float responseDuration;
    public bool isCorrect;
    public string OpenedEye; // "Left", "Right", or "None"
    public bool answered = false;
    public ResponseData(float spawnTime, float responseTime, bool isCorrect, string blockedEye,bool answered)
    {
        this.spawnTime = spawnTime;
        this.responseTime = responseTime;
        this.responseDuration = responseTime - spawnTime;
        this.isCorrect = isCorrect;
        this.OpenedEye = blockedEye;
        this.answered = answered;
    }
}
public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [SerializeField]private float startTime;
    [SerializeField]public float spawnTime;
    [SerializeField]private float timerDuration = -1f;
    [SerializeField]private bool isRunning = false;
    [SerializeField]public float responseTime;
    [SerializeField]public bool userWasCorrect = true;  // Example condition
    [SerializeField]public bool Answered = false;  // Example condition
    [SerializeField]public string openedeye = "L";  // Can be "Right" or "None"
    [SerializeField]public List<ResponseData> responseRecords = new List<ResponseData>();
    private Coroutine timerCoroutine;
    private int currentIndex;
    [SerializeField]private int[] duration=new int[] {1,5,10};
    [SerializeField]public ResultUIHandler resultUIHandler;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Start the timer
    public void StartTimer()
    {
        if (isRunning) return; // Prevent duplicate coroutines
        startTime = Time.time;
        isRunning = true;
        timerCoroutine = StartCoroutine(TrackTime());
    }

    // Reset the timer
    public void ResetTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        startTime = 0f;
        isRunning = false;
    }
    public float gettimeremaining() { 
        return timerDuration*60 - (Time.time - startTime);
    }
    // Get current elapsed time in minutes, seconds, and milliseconds (two decimals)
    public float GetCurrentTime()
    {
        if (!isRunning) return 0;
        return Time.time - startTime;
    }

    // Set a timer duration and stop when it reaches the limit
    public void SetTimerDuration(float duration)
    {
        timerDuration = duration;
        
    }

    // Coroutine for tracking time
    private IEnumerator TrackTime()
    {
        while (isRunning)
        {
            //Debug.Log("Current time: " + FormatTime(GetCurrentTime()));

            if (timerDuration*60 > 0 && Time.time - startTime >= timerDuration*60)
            {
                isRunning = false;
                break;
            }

            yield return new WaitForSeconds(0.1f); // Update every 0.1 seconds
        }

        isRunning = false;
    }

    // Calculate response time difference
    
    // Format time in mm:ss:ms format
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time * 100) % 100; // Two decimal places

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
    public void RecordResponse()
    {
        
        if (!isRunning) return;

        ResponseData newRecord = new ResponseData(spawnTime, responseTime, userWasCorrect, openedeye,Answered);
        responseRecords.Add(newRecord);

        
    }

    // Get all response records
    public List<ResponseData> GetResponseRecords()
    {
        return responseRecords;
    }

    // //Debug Log all recorded data
    public void PrintAllRecords()
    {
        foreach (ResponseData record in responseRecords)
        {
            //Debug.Log($"Spawn: {record.spawnTime}, Response: {record.responseTime}, Duration: {record.responseDuration}s, " +$"Correct: {record.isCorrect}, Opened Eye: {record.OpenedEye}");
        }
    }
    public void CycleTimeValue(bool cycleUp)
    {
        if (cycleUp)
            currentIndex = (currentIndex + duration.Length - 1) % duration.Length;
        else
            currentIndex = (currentIndex + 1) % duration.Length;
        
        float selectedTime = duration[currentIndex];
        UIHandler.instance.SettingsText[0].text = selectedTime.ToString() + "Mins";
        SetTimerDuration(selectedTime);
    }
    public void CalculateResponseStats()
    {
        int totalShown = responseRecords.Count;
        int correctResponses = 0;
        int incorrectResponses = 0;
        int unansweredResponses = 0;

        int totalShownLeft = 0;
        int correctLeft = 0;
        int incorrectLeft = 0;
        int unansweredLeft = 0;
        float totalResponseDurationLeft = 0f; // Variable to store the total response duration for the left eye
        int totalResponsesLeft = 0; // Variable to store the number of responses recorded for the left eye

        int totalShownRight = 0;
        int correctRight = 0;
        int incorrectRight = 0;
        int unansweredRight = 0;
        float totalResponseDurationRight = 0f; // Variable to store the total response duration for the right eye
        int totalResponsesRight = 0; // Variable to store the number of responses recorded for the right eye

        foreach (ResponseData record in responseRecords)
        {
            // General stats
            if (record.answered)
            {
                if (record.isCorrect)
                    correctResponses++;
                else
                    incorrectResponses++;
            }
            else
            {
                unansweredResponses++;
            }

            // Eye-specific stats
            if (record.OpenedEye == "L")
            {
                totalShownLeft++;
                if (record.answered)
                {
                    if (record.isCorrect)
                        correctLeft++;
                    else
                        incorrectLeft++;
                }
                else
                {
                    unansweredLeft++;
                }

                totalResponseDurationLeft += record.responseDuration; // Add the response duration to the total for the left eye
                totalResponsesLeft++; // Increment the number of responses recorded for the left eye
            }
            else if (record.OpenedEye == "R")
            {
                totalShownRight++;
                if (record.answered)
                {
                    if (record.isCorrect)
                        correctRight++;
                    else
                        incorrectRight++;
                }
                else
                {
                    unansweredRight++;
                }

                totalResponseDurationRight += record.responseDuration; // Add the response duration to the total for the right eye
                totalResponsesRight++; // Increment the number of responses recorded for the right eye
            }
        }

        resultUIHandler.total.text = totalShown.ToString();
        resultUIHandler.score.text = correctResponses.ToString();

        // Store in variables
        //Debug.Log($"Total: {totalShown}, Correct: {correctResponses}, Incorrect: {incorrectResponses}, Unanswered: {unansweredResponses}");
        resultUIHandler.FillGeneralStats((int)timerDuration, totalShown, correctResponses, incorrectResponses, unansweredResponses);
        //Debug.Log($"Left Eye -> Total: {totalShownLeft}, Correct: {correctLeft}, Incorrect: {incorrectLeft}, Unanswered: {unansweredLeft}");
        resultUIHandler.LeftEyeStats(totalShownLeft, correctLeft, incorrectLeft, unansweredLeft);
        //Debug.Log($"Right Eye -> Total: {totalShownRight}, Correct: {correctRight}, Incorrect: {incorrectRight}, Unanswered: {unansweredRight}");
        resultUIHandler.RightEyeStats(totalShownRight, correctRight, incorrectRight, unansweredRight);

        // Calculate average response duration for both eyes
        float averageResponseDurationLeft = totalResponseDurationLeft / totalResponsesLeft;
        float averageResponseDurationRight = totalResponseDurationRight / totalResponsesRight;
        resultUIHandler.RighEyeTimeStamps();
        resultUIHandler.LeftEyeTimeStamps();
        resultUIHandler.RighteyeAverageResponseTime.text= Math.Round(averageResponseDurationRight, 2).ToString() + " Sec"; 
        resultUIHandler.LefteyeAverageResponseTime.text=Math.Round(averageResponseDurationLeft,2).ToString()+" Sec";
        
        //Debug.Log($"Average Response Duration - Left Eye: {averageResponseDurationLeft}s, Right Eye: {averageResponseDurationRight}s");
    }

}

