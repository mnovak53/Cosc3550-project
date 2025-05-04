using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint2D : MonoBehaviour
{
    [Tooltip("1-based index for this checkpoint")]
    public int checkpointIndex = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        RaceManager.S.RecordCheckpoint(checkpointIndex);
    }
}
