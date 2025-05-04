using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarRecorder : MonoBehaviour
{
    public Transform carSpriteObject;
    public GameObject ghostCarPlaybackPrefab;

    GhostCarData ghostCarData = new GhostCarData();

    bool isRecording = true;

    Rigidbody2D carRigidbody2D;
    CarInput carInput;

    private void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carInput = GetComponent<CarInput>();
    }

    void Start()
    {
        GameObject ghostCar = Instantiate(ghostCarPlaybackPrefab);

        ghostCar.GetComponent<GhostCarPlayback>().LoadData(carInput.playerNumber);

        StartCoroutine(RecordCarPosition());
        StartCoroutine(SaveCarPosition());
    }

    IEnumerator RecordCarPosition()
    {
        while (isRecording)
        {
            if (carSpriteObject != null)
                ghostCarData.AddDataItem(new GhostCarDataListItem(carRigidbody2D.position, carRigidbody2D.rotation, carSpriteObject.localScale, Time.timeSinceLevelLoad));
            yield return new WaitForSeconds(0.15f);

        }
    }

    IEnumerator SaveCarPosition()
    {
        yield return new WaitForSeconds(5);

        SaveData();
    }

    void SaveData()
    {
        string jsonEncodeData = JsonUtility.ToJson(ghostCarData);

        Debug.Log($"Saved ghost data {jsonEncodeData}");

        if (carInput != null)
        {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}_{carInput.playerNumber}_ghost", jsonEncodeData);
            PlayerPrefs.Save();
        }

        isRecording = false;
    }


}
