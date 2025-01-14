using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    public bool TutorialRunning = false;

    public float BuildingSpeed = 10f;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }



    void Start()
    {
        UIHandler.instance.UpdateButton(-1);
        UIHandler.instance.UpdateScreen(0);
        EyeToggle.instance.UpdateEye(-1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginTutorial() {
        
        StartCoroutine(TutorialLoop());
    }

    IEnumerator TutorialLoop()
    {
        TutorialRunning = true;
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[0]);
        yield return new WaitForSeconds(2);
        UIHandler.instance.UpdateCenterScreen(UIHandler.instance.TutorialImages[1]);
        yield return new WaitForSeconds(2);
        UIHandler.instance.UpdateButton(1);




        while (TutorialRunning)
        {
            yield return null;
        }
    }

}
