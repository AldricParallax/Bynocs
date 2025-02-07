using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIHandler : MonoBehaviour
{
    [SerializeField] public GameObject[] resultPanel;
    [SerializeField] private GameObject result;
    [SerializeField] GameObject[] Stats;
    [SerializeField] private GameObject RCOntent;
    [SerializeField] private GameObject LContent;
    [SerializeField] private GameObject ReyeDataTemplate;
    [SerializeField] private GameObject LeyeDataTemplate;
    public TextMeshProUGUI RighteyeAverageResponseTime;
    public TextMeshProUGUI LefteyeAverageResponseTime;
    public TextMeshProUGUI total;
    public TextMeshProUGUI score;
    private void Start()
    {
        UpdateScores();
    }
    public void UpdateScores()
    {
        // Update all "Total" text fields
        
        
        total.GetComponent<TextMeshProUGUI>().text = total.text;
        

        // Update all "Score" text fields
        
        score.GetComponent<TextMeshProUGUI>().text = score.text;
        
    }
    public void NextResultPanel(bool yes)
    {
        if (resultPanel.Length == 0) return;

        // Find currently active panel
        int currentIndex = -1;
        for (int i = 0; i < resultPanel.Length; i++)
        {
            if (resultPanel[i].activeSelf)
            {
                currentIndex = i;
                break;
            }
        }

        // Determine next index
        if (yes)
            currentIndex = (currentIndex + 1) % resultPanel.Length; // Move forward
        else
            currentIndex = (currentIndex - 1 + resultPanel.Length) % resultPanel.Length; // Move backward

        // Disable all panels
        foreach (var panel in resultPanel)
        {
            panel.SetActive(false);
        }

        // Enable next panel
        resultPanel[currentIndex].SetActive(true);
    }

    public void FillGeneralStats(int gameduration,int totalShown,int correctResponses,int incorrectResponses,int unansweredResponses)
    {
        // Fill general stats
        Stats[0].transform.Find("TotalGameDuration").GetComponent<TextMeshProUGUI>().text = gameduration.ToString() + " Mins";
        Stats[0].transform.Find("Signboards_Shown").GetComponent<TextMeshProUGUI>().text = totalShown.ToString();
        Stats[0].transform.Find("Correct_Response").GetComponent<TextMeshProUGUI>().text = correctResponses.ToString();
        Stats[0].transform.Find("Incorrect_Responses").GetComponent<TextMeshProUGUI>().text = incorrectResponses.ToString();
        Stats[0].transform.Find("Unanswered_Responses").GetComponent<TextMeshProUGUI>().text = unansweredResponses.ToString();

    }
    public void LeftEyeStats(int totalShown,int correctResponses,int incorrectResponses,int unansweredResponses)
    {
        // Fill general stats
        
        Stats[1].transform.Find("Signboards_Shown").GetComponent<TextMeshProUGUI>().text = totalShown.ToString();
        Stats[1].transform.Find("Correct_Response").GetComponent<TextMeshProUGUI>().text = correctResponses.ToString();
        Stats[1].transform.Find("Incorrect_Responses").GetComponent<TextMeshProUGUI>().text = incorrectResponses.ToString();
        Stats[1].transform.Find("Unanswered_Responses").GetComponent<TextMeshProUGUI>().text = unansweredResponses.ToString();

    }
    public void RightEyeStats(int totalShown,int correctResponses,int incorrectResponses,int unansweredResponses)
    {
        // Fill general stats
        
        Stats[2].transform.Find("Signboards_Shown").GetComponent<TextMeshProUGUI>().text = totalShown.ToString();
        Stats[2].transform.Find("Correct_Response").GetComponent<TextMeshProUGUI>().text = correctResponses.ToString();
        Stats[2].transform.Find("Incorrect_Responses").GetComponent<TextMeshProUGUI>().text = incorrectResponses.ToString();
        Stats[2].transform.Find("Unanswered_Responses").GetComponent<TextMeshProUGUI>().text = unansweredResponses.ToString();

    }
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        float milliseconds = (time * 100) % 100; // Two decimal places

        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
    public void RighEyeTimeStamps()
    { 
        
        List<ResponseData> Reyedata= new List<ResponseData>();
        foreach (ResponseData response in TimerManager.instance.GetResponseRecords()) {
            if (response.OpenedEye == "R")
            {
                Reyedata.Add(response);
            }
        }
        int i =0;
        foreach (ResponseData record in Reyedata)
        {
            i++;
            Debug.Log($"Spawn: {record.spawnTime}, Response: {record.responseTime}, Duration: {record.responseDuration}s, " +
                      $"Correct: {record.isCorrect}, Opened Eye: {record.OpenedEye}");
            GameObject go = Instantiate(ReyeDataTemplate, RCOntent.transform);
            go.transform.Find("No.").GetComponent<TextMeshProUGUI>().text = i.ToString();
            go.transform.Find("SpawnTime").GetComponent<TextMeshProUGUI>().text = FormatTime(record.spawnTime);
            go.transform.Find("ResponseTime").GetComponent<TextMeshProUGUI>().text = FormatTime(record.responseTime);
            go.transform.Find("ResponseDuration").GetComponent<TextMeshProUGUI>().text = Math.Round(record.responseDuration,2).ToString() + " Sec";
        }
    }
    public void LeftEyeTimeStamps()
    {
        int i = 0;
        List<ResponseData> Leyedata= new List<ResponseData>();
        foreach (ResponseData response in TimerManager.instance.GetResponseRecords()) {
            if (response.OpenedEye == "L")
            {
                Leyedata.Add(response);
            }
        }
        foreach (ResponseData record in Leyedata)
        {
            i++;
            Debug.Log($"Spawn: {record.spawnTime}, Response: {record.responseTime}, Duration: {record.responseDuration}s, " +
                      $"Correct: {record.isCorrect}, Opened Eye: {record.OpenedEye}");
            GameObject go = Instantiate(LeyeDataTemplate, LContent.transform);
            go.transform.Find("No.").GetComponent<TextMeshProUGUI>().text = i.ToString();
            go.transform.Find("SpawnTime").GetComponent<TextMeshProUGUI>().text = FormatTime(record.spawnTime);
            go.transform.Find("ResponseTime").GetComponent<TextMeshProUGUI>().text = FormatTime(record.responseTime);
            go.transform.Find("ResponseDuration").GetComponent<TextMeshProUGUI>().text = Math.Round(record.responseDuration,2).ToString()+ " Sec";
        }
    }
    public void clearUI()
    {
        foreach(Transform child in RCOntent.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in LContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
