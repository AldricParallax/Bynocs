using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public bool TutorialRunning = false; 




    void Start()
    {
        UIHandler.instance.UpdateButton(-1);
        UIHandler.instance.UpdateScreen(0);
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
