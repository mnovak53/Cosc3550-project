using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarLapCounter : MonoBehaviour
{
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;
    int numberofPassedCheckPoints = 0;
    int lapsCompleted = 0;
    const int lapsToComplete = 2;
    bool isRaceCompleted = false;
    int carPosition = 0;

    public event Action<CarLapCounter> OnPassCheckpoint;

    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public int GetNumberOfPassedCheckPoints()
    {
        return numberofPassedCheckPoints;
    }

    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            if (isRaceCompleted) return;

            Checkpoint checkPoint = collision.GetComponent<Checkpoint>();
            Debug.Log("Collison detected.");

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;
                numberofPassedCheckPoints++;
                timeAtLastPassedCheckPoint = Time.time;
                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }
                }
                OnPassCheckpoint?.Invoke(this);
            }
        }
    }


}
