using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;



public class EyeData
{
    public int totalSignboards;
    public int correctInputs;
    public int incorrectInputs;
    public int unansweredInputs;
    public float averageResponseTime;
}


public class GameSessionData
{
    public int id;
    public string userId;
    public string gameStartTime;
    public string gameEndTime;
    public int gameTime;
    public string speedLimitScale;
    public bool motionSicknessAlertness;
    public EyeData bothEyes;
    public EyeData leftEye;
    public EyeData rightEye;
    public string createdDatetime;
}


public class GameDataSender : MonoBehaviour
{
    [SerializeField] private string endpoint = "https://your-endpoint.com/api/game-data"; // Replace with your real endpoint

    public void SendData()
    {

        // Populate data
        GameSessionData data = new GameSessionData
        {
            id = 0,
            userId = "test_user_001",
            gameStartTime = System.DateTime.UtcNow.ToString("o"),
            gameEndTime = System.DateTime.UtcNow.AddMinutes(5).ToString("o"),
            gameTime = 300, // seconds
            speedLimitScale = "Normal",
            motionSicknessAlertness = false,
            createdDatetime = System.DateTime.UtcNow.ToString("o"),
            bothEyes = new EyeData(),
            leftEye = new EyeData(),
            rightEye = new EyeData()
        };

        string json = JsonUtility.ToJson(data);
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
    }
}
