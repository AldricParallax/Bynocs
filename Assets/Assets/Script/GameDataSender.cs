using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.IO;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class EyeData
{
    public int totalSignboards;
    public int correctInputs;
    public int incorrectInputs;
    public int unansweredInputs;
    public float averageResponseTime;
}
[System.Serializable]
public struct EyeDataGetter
{
    public TMP_Text totalSignboards;
    public TMP_Text correctInputs;
    public TMP_Text incorrectInputs;
    public TMP_Text unansweredInputs;
    public TMP_Text averageResponseTime;
}

public class GameSessionData
{
    public string userId;
    public string gameStartTime;
    public string gameEndTime;
    public int gameTime;
    public string speedLimitScale;
    public bool motionSicknessAlertness;
    public EyeData bothEyes;
    public EyeData leftEye;
    public EyeData rightEye;
    //public string createdDatetime;
}


public class GameDataSender : MonoBehaviour
{
    [SerializeField] private string endpoint = "https://your-endpoint.com/api/game-data"; // Replace with your real endpoint
    public string _gameStartTime;
    public string _gameEndTime;
    [SerializeField] EyeDataGetter BothEyeData;
    [SerializeField] EyeDataGetter RightEyeData;
    [SerializeField] EyeDataGetter LeftEyeData;
    public static GameDataSender instance;
    string UserName = "NULL";
    [SerializeField]TMP_Text fjdsfjsd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void readalltextasync()
    {

    }

    private void Start()
    {
        string basePath = "/storage/emulated/0/Bynocs/";
        string fileName = "Id.txt";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);



        if (File.Exists(filePath))
        {
            Debug.Log("exists");


            UserName = File.ReadAllText(filePath);


            //UserName = File.ReadAllText(filePath);
            ////UserName = File.ReadLines(filePath);
            ////text.text = File.ReadAllText(filePath);
            ////text.text = "File found: " + filePath;
           //fjdsfjsd.text = UserName;

        }
        else
        {
            //text.text = "File not found: " + filePath;


        }
    }

    public void CaptureStartTime()
    {
        _gameStartTime = System.DateTime.UtcNow.ToString("o");
    }

    public void CaptureEndTime()
    {
        _gameEndTime = System.DateTime.UtcNow.ToString("o");
    }

    private EyeData ParseEyeData(EyeDataGetter getter)
    {
        return new EyeData
        {
            totalSignboards = ParseInt(getter.totalSignboards),
            correctInputs = ParseInt(getter.correctInputs),
            incorrectInputs = ParseInt(getter.incorrectInputs),
            unansweredInputs = ParseInt(getter.unansweredInputs),
            averageResponseTime = ParseFloat(getter.averageResponseTime)
    };
    }

    private int ParseInt(TMP_Text text)
    {
        return int.TryParse(text.text, out int value) ? value : 0;
    }

    private float ParseFloat(TMP_Text text)
    {
        string newTxt = text.text.Split(" Sec")[0];
        return float.TryParse(newTxt, out float value) ? ((int)(value * 100 + 0.5f)) / 100f : 0f;
    }

    public void SendData()
    {
         
            EyeData bothEyesdata = ParseEyeData(BothEyeData);
            EyeData leftEyedata = ParseEyeData(LeftEyeData);
            EyeData rightEyedata = ParseEyeData(RightEyeData);
            // Populate data
            GameSessionData data = new GameSessionData
            {
                userId = UserName,
                gameStartTime = _gameStartTime,
                gameEndTime = _gameEndTime,
                gameTime = TimerManager.instance.selectedTime, // seconds
                speedLimitScale = !UIHandler.instance.ScaleLarge?"SMALL":"LARGE",
                motionSicknessAlertness = MotionSicknessBool.instance.motionSicknessEnabled,
                //createdDatetime = System.DateTime.UtcNow.ToString("o"),
                bothEyes = bothEyesdata,
                leftEye = leftEyedata,
                rightEye = rightEyedata
            };

            string json = JsonUtility.ToJson(data);
        Debug.LogError(json);
            StartCoroutine(SendGameData(json));


        
    }

    IEnumerator SendGameData(string json)
    {
        using (UnityWebRequest request = new UnityWebRequest(endpoint, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending JSON: " + json);
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isHttpError && !request.isNetworkError)
#endif
            {
                Debug.Log("Success: " + request.downloadHandler.text);

                
            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        
        using (UnityWebRequest request = new UnityWebRequest("https://bynocs-api-1066008910244.europe-west1.run.app/bynocsvr/api/QuickFocus", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log("Sending JSON: " + json);
            yield return request.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
            if (request.result == UnityWebRequest.Result.Success)
#else
            if (!request.isHttpError && !request.isNetworkError)
#endif
            {
                Debug.Log("Success: " + request.downloadHandler.text);
                yield return new WaitForSeconds(1f);

            }
            else
            {
                Debug.LogError("Error: " + request.error);
            }
        }
        
    }
}
