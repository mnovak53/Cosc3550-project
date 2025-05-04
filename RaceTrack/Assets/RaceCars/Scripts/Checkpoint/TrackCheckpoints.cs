using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerIncorrectCheckpoint;

    private List<CheckpointSingle> checkpointSinglesList;
    private int nextCheckpointSingleIndex;
    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");


        checkpointSinglesList = new List<CheckpointSingle>();
        foreach (Transform checkpointsSingleTransform in checkpointsTransform)
        {
            CheckpointSingle checkpointSingle = checkpointsSingleTransform.GetComponent<CheckpointSingle>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSinglesList.Add(checkpointSingle);
        }
        nextCheckpointSingleIndex = 0;
    }

    public void PlayerThroughCheckpont(CheckpointSingle checkpointSingle)
    {
        if (checkpointSinglesList.IndexOf(checkpointSingle) == nextCheckpointSingleIndex)
        {
            Debug.Log("Correct");
            CheckpointSingle correctChechkpointSingle = checkpointSinglesList[nextCheckpointSingleIndex];
            correctChechkpointSingle.Hide();
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSinglesList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Debug.Log("Wrong");
            OnPlayerIncorrectCheckpoint?.Invoke(this, EventArgs.Empty);

            CheckpointSingle correctChechkpointSingle = checkpointSinglesList[nextCheckpointSingleIndex];
            correctChechkpointSingle.Show();
        }
    }
}
