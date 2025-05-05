using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceTimeUIHandler : MonoBehaviour
{
    Text timeText;

    float lastRaceTimeUpdate = 0;

    int finalMinutes = 0;
    int finalSeconds = 0;

    void Awake()
    {
        timeText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateTimeCO());
    }

    IEnumerator UpdateTimeCO()
    {
        while (true)
        {
            float raceTime = GameManager.instance.GetRaceTime();

            if (lastRaceTimeUpdate != raceTime)
            {
                int raceTimeMinutes = (int)Mathf.Floor(raceTime / 60);
                int raceTimeSeconds = (int)Mathf.Floor(raceTime % 60);

                timeText.text = $"Time: {raceTimeMinutes.ToString("00")}:{raceTimeSeconds.ToString("00")}";

                lastRaceTimeUpdate = raceTime;
            }

            else if (GameManager.instance.GetGameStates() == GameStates.raceOver)
            {
                finalMinutes = (int)Mathf.Floor(raceTime / 60);
                finalSeconds = (int)Mathf.Floor(raceTime % 60);
                break;

            }
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("Gets to this part");
        if (PlayerPrefs.HasKey("BestTimeMintues") && PlayerPrefs.HasKey("BestTimeSeconds"))
        {
            if (PlayerPrefs.GetInt("BestTimeMinutes") >= finalMinutes) PlayerPrefs.SetInt("BestTimeMinutes", finalMinutes);
            if (PlayerPrefs.GetInt("BestTimeSeconds") >= finalSeconds) PlayerPrefs.SetInt("BestTimeSeconds", finalSeconds);
            Debug.Log(PlayerPrefs.GetInt("BestTimeMinutes"));
            Debug.Log(PlayerPrefs.GetInt("BestTimeSeconds"));
            Debug.Log("Times gotten");
        }
        else
        {
            PlayerPrefs.SetInt("BestTimeMinutes", finalMinutes);
            PlayerPrefs.SetInt("BestTimeSeconds", finalSeconds);
            Debug.Log(PlayerPrefs.GetInt("BestTimeMinutes"));
            Debug.Log(PlayerPrefs.GetInt("BestTimeSeconds"));
            Debug.Log("Times set");
        }
    }


}
