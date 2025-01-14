using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimitScript : MonoBehaviour
{
   public int speedLimit = 60;
   [SerializeField] private int[] SpeedLimits= { 25, 40, 60, 80 ,90};
   [SerializeField] private GameObject[] SignBoards;
   [SerializeField] private GameObject canvas;
    private GameObject currentSignBoard;
    void Start()
    {
        speedLimit = SpeedLimits[2];
        
    }
    private void FixedUpdate()
    {
        if(currentSignBoard==null)
        {
            int randomIndex = Random.Range(0, SpeedLimits.Length);
            speedLimit = SpeedLimits[randomIndex];
            spawnSignBoard(randomIndex);
        }

        
    }
   // IEnumerator SetSpeedLimit()
   //{
   //     while (currentSignBoard==null)
   //     {
   //         int randomIndex = Random.Range(0, SpeedLimits.Length);
   //         speedLimit = SpeedLimits[randomIndex];
   //         spawnSignBoard(randomIndex);
   //         yield return new WaitForSeconds(15);
   //     }
      
   //}
   private void spawnSignBoard(int signindex)
   {
        //spawn signboard

        GameObject gameobject = Instantiate(SignBoards[signindex],transform);
        currentSignBoard = gameobject;
        
       
    }
}
