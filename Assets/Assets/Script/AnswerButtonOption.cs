using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswerButtonOption : MonoBehaviour
{
    public bool IsCorrect = false;
    int speedValue;

    public void OnClickfunc()
    {
        GetComponent<Button>().image.color = IsCorrect ? Color.green : Color.red;
        transform.GetChild(0).GetComponent<TMP_Text>().color = IsCorrect ? new Color(0f, 0.9803922f, 0.509804f) : new Color(1f, 0.6784314f, 0.6784314f);
        GameplayManager.instance.BuildingSpeed = speedValue;
    }

    // Start is called before the first frame update
    public void ResetValue(int Value)
    {
        speedValue = Value;
        GetComponent<Button>().image.color = Color.white;
        transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        transform.GetChild(0).GetComponent<TMP_Text>().text = Value.ToString();
        IsCorrect = Value == VehicleSpeedHandler.instance.SelectedSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
