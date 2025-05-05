using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestTimeUIHandler : MonoBehaviour
{
    private Text bestTimeText;
    public int bestRaceTimeMinutes = 0;
    public int bestRaceTimeSeconds = 0;

    void Awake()
    {
        bestTimeText = GetComponent<Text>();

        if (PlayerPrefs.HasKey("BestTimeMintues") && PlayerPrefs.HasKey("BestTimeSeconds"))
        {
            Debug.Log("Has Keys");
            bestRaceTimeMinutes = PlayerPrefs.GetInt("BestTimeMinutes");
            bestRaceTimeSeconds = PlayerPrefs.GetInt("BestTimeSeconds");

        }
        else Debug.Log("No Keys");
    }


    void Start()
    {
        SetBestTime();
    }

    void SetBestTime()
    {
        bestTimeText.text = $"Best Time: {bestRaceTimeMinutes.ToString("00")}:{bestRaceTimeSeconds.ToString("00")}";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
