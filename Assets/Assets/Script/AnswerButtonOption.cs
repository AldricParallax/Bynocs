
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ColorUtility = UnityEngine.ColorUtility;

public class AnswerButtonOption : MonoBehaviour
{
    public bool IsCorrect = false;
    int speedValue;

    public void OnClickfunc()
    {
        ColorUtility.TryParseHtmlString("#99999", out Color Text);
        ColorUtility.TryParseHtmlString("#33333", out Color Button);
        GetComponent<Button>().image.color = Button;
        transform.GetChild(0).GetComponent<TMP_Text>().color = Text;
        if(GameplayManager.instance.TutorialSemaphore==-1)
        {
            TimerManager.instance.responseTime = TimerManager.instance.GetCurrentTime();
            TimerManager.instance.userWasCorrect = IsCorrect;
            TimerManager.instance.openedeye = GameplayManager.instance.RightEyeBlock ? "L" : "R";
            TimerManager.instance.Answered=true;
        }
        GameplayManager.instance.BuildingSpeed = GameplayManager.instance.SpeedValues[speedValue];
        GameplayManager.instance.OnSignBannerEnd(IsCorrect);
        VehicleSpeedHandler.instance.SetButtonEnable(false);
    }

    // Start is called before the first frame update
    public void ResetValue(int Value)
    {
        speedValue = Value;
        GetComponent<Button>().image.color = Color.white;
        transform.GetChild(0).GetComponent<TMP_Text>().color = Color.white;
        transform.GetChild(0).GetComponent<TMP_Text>().text = Value.ToString();
        IsCorrect = GameplayManager.instance.SpeedValues[Value] == VehicleSpeedHandler.instance.SelectedSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
