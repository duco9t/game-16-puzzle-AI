using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float ellapsedTime;

    // Update is called once per frame
    void Update()
    {
        ellapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(ellapsedTime / 60);
        int seconds = Mathf.FloorToInt(ellapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
