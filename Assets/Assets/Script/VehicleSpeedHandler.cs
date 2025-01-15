using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpeedHandler : MonoBehaviour
{
    public static VehicleSpeedHandler instance;
    public List<AnswerButtonOption> Buttons = new List<AnswerButtonOption>();
    public GameObject Canvas;
    public int SelectedSpeed = 0;
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
        for (int i = 0; i < numbers.Count; i++)
        {
            
            Buttons[i].ResetValue(numbers[i]);
        }
    }
}
