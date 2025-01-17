using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSpeedHandler : MonoBehaviour
{
    public static VehicleSpeedHandler instance;
    public List<AnswerButtonOption> Buttons = new List<AnswerButtonOption>();
    public GameObject Canvas;
    public int SelectedSpeed = 0;
    public bool IsGivingAnswerAllowed = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }




    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public void SetButtonData(List<int> numbers)
    {
        SetButtonEnable(true);
        for (int i = 0; i < numbers.Count; i++)
        {
            
            Buttons[i].ResetValue(numbers[i]);
        }
    }

    public void SetButtonEnable(bool Enable)
    {
        IsGivingAnswerAllowed = !Enable;
        for (int i = 0; i < Buttons.Count; i++)
        {

            Buttons[i].GetComponent<Button>().interactable = Enable;
        }
    }
}
