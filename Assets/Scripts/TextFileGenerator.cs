using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextFileGenerator : MonoBehaviour
{
    private string filePath;

    void Start()
    {
 
        filePath = "Assets/Scripts/EyeTrackingDummyLog.txt";
        Debug.Log("Logging eye tracking data to: " + filePath);
    }

    void Update()
    {
        // Simulated dummy data
        float time = Time.time;
        float velocityLeft = UnityEngine.Random.Range(0.5f, 10.0f);
        float velocityRight = UnityEngine.Random.Range(0.5f, 10.0f);
        float fixationLeft = UnityEngine.Random.Range(0, 2);
        float fixationRight = UnityEngine.Random.Range(0, 2);
        float saccadeLeft = UnityEngine.Random.Range(0, 2);
        float saccadeRight = UnityEngine.Random.Range(0, 2);
        float pupilLeft = UnityEngine.Random.Range(2.5f, 4.5f); // Typical pupil size in mm
        float pupilRight = UnityEngine.Random.Range(2.5f, 4.5f);
        Vector3 gazeOriginLeft = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
        Vector3 gazeDirectionLeft = new Vector3(0, UnityEngine.Random.Range(-1f, 1f), 1);
        Vector3 gazeOriginRight = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, 0);
        Vector3 gazeDirectionRight = new Vector3(0, UnityEngine.Random.Range(-1f, 1f), 1);

        // Formatting log data
        string logData = $"{time}, {velocityLeft}, {velocityRight}, {fixationLeft}, {fixationRight}, " +
                         $"{saccadeLeft}, {saccadeRight}, {pupilLeft}, {pupilRight}, " +
                         $"{gazeOriginLeft}, {gazeDirectionLeft}, {gazeOriginRight}, {gazeDirectionRight}\n";

        // Append data to file
        File.AppendAllText(filePath,logData);
    }
}
