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
    public string blockedEye; // "Left", "Right", or "None"
    
    public ResponseData(float spawnTime, float responseTime, bool isCorrect, string blockedEye)
    {
        this.spawnTime = spawnTime;
        this.responseTime = responseTime;
        this.responseDuration = responseTime - spawnTime;
        this.isCorrect = isCorrect;
        this.blockedEye = blockedEye;
    }
}
public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;

    [SerializeField]private float startTime;
    [SerializeField]private float timerDuration = -1f;
    [SerializeField]private bool isRunning = false;
    [SerializeField]public float responseTime;
    [SerializeField]public bool userWasCorrect = true;  // Example condition
    [SerializeField]public string openedeye = "L";  // Can be "Right" or "None"
    [SerializeField]public List<ResponseData> responseRecords = new List<ResponseData>();
    private Coroutine timerCoroutine;
    
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
        StartTimer();
    }

    // Coroutine for tracking time
    private IEnumerator TrackTime()
    {
        while (isRunning)
        {
            Debug.Log("Current time: " + FormatTime(GetCurrentTime()));

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
    public string CalculateResponseTime(float spawnTime, float responseTime)
    {
        float difference = responseTime - spawnTime;
        return FormatTime(difference);
    }

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
        CalculateResponseTime(startTime, responseTime);
        if (!isRunning) return;

        ResponseData newRecord = new ResponseData(startTime, responseTime, userWasCorrect, openedeye);
        responseRecords.Add(newRecord);

        
    }

    // Get all response records
    public List<ResponseData> GetResponseRecords()
    {
        return responseRecords;
    }

    // Debug Log all recorded data
    public void PrintAllRecords()
    {
        foreach (ResponseData record in responseRecords)
        {
            Debug.Log($"Spawn: {record.spawnTime}, Response: {record.responseTime}, Duration: {record.responseDuration}s, " +
                      $"Correct: {record.isCorrect}, Opened Eye: {record.blockedEye}");
        }
    }
}

