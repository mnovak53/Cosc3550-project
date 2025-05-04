using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class TrackCheckpointsUI : MonoBehaviour
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;

    private void Start()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnPlayerCorrectCheckpoint;
        trackCheckpoints.OnPlayerIncorrectCheckpoint += TrackCheckpoints_OnPlayerIncorrectCheckpoint;

        Hide();
    }

    private void TrackCheckpoints_OnPlayerCorrectCheckpoint(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void TrackCheckpoints_OnPlayerIncorrectCheckpoint(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}
