using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownUIHandler : MonoBehaviour
{
    public Text countDownText;

    void Awake()
    {
        countDownText.text = "";
    }

    void Start()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(0.3f);

        int counter = 3;

        while (true)
        {
            if (counter != 0) countDownText.text = counter.ToString();
            else
            {
                countDownText.text = "Go";
                GameManager.instance.OnRaceStart();
                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
