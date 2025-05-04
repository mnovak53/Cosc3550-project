using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RaceManager : MonoBehaviour
{
    // Start is called before the first frame update
  static public RaceManager S;

    [Header("UI (assign these in your Canvas)")]
    public TextMeshProUGUI currentTimerText;    // shows time since last checkpoint
    public TextMeshProUGUI lastCheckpointText;  // shows the just‑completed lap
    public TextMeshProUGUI bestCheckpointText;  // shows your fastest lap so far
    public TextMeshProUGUI finishTimeText;      // total race time when you finish

    float raceTime = 0f;             // total time since StartRace()
    float lastCheckpointTime = 0f;   // raceTime at the last checkpoint
    bool  raceActive = false;
    List<float> lapTimes = new List<float>();

    void Awake()
    {
        if (S != null && S != this) Destroy(gameObject);
        else S = this;
    }

    void Start()
    {
        StartRace();
    }

    void Update()
    {
        if (!raceActive) return;

        // Advance the total timer
        raceTime += Time.deltaTime;

        // Compute current lap time (time since last checkpoint)
        float lapTime = raceTime - lastCheckpointTime;
        currentTimerText.text = FormatTime(lapTime);
    }

    /// <summary>
    /// Call when the race begins (or if you want to restart mid‐game).
    /// </summary>
    public void StartRace()
    {
        raceTime = 0f;
        lastCheckpointTime = 0f;
        lapTimes.Clear();
        raceActive = true;

        // Reset UI
        currentTimerText.text    = "00:00.00";
        lastCheckpointText.text  = "--:--.--";
        bestCheckpointText.text  = "--:--.--";
        finishTimeText.text      = "--:--.--";
    }

    /// <summary>
    /// Call from your checkpoint trigger (passing its index).
    /// Records the lap time, updates best, and resets the lap timer.
    /// </summary>
    public void RecordCheckpoint(int index)
    {
        if (!raceActive) return;

        // This lap’s duration
        float lapTime = raceTime - lastCheckpointTime;
        lapTimes.Add(lapTime);
        lastCheckpointTime = raceTime;

        // Update UI
        lastCheckpointText.text = $"Lap {index}: {FormatTime(lapTime)}";

        float best = Mathf.Min(lapTimes.ToArray());
        bestCheckpointText.text = $"Best Lap: {FormatTime(best)}";
    }

    /// <summary>
    /// Call from your finish‐line trigger.
    /// Stops the race and shows total time.
    /// </summary>
    public void FinishRace()
    {
        if (!raceActive) return;
        raceActive = false;

        finishTimeText.text = $"Finish: {FormatTime(raceTime)}";
    }

    // Utility to format seconds into MM:SS.ss
    string FormatTime(float t)
    {
        int minutes = Mathf.FloorToInt(t / 60f);
        float seconds = t % 60f;
        return $"{minutes:00}:{seconds:00.00}";
    }
}