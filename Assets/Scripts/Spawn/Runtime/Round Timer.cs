using System;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] private float roundDuration = 120f;
    [SerializeField] private TMP_Text roundTimeText; 

    public EventNoParam eventNoParm;
    
    private void OnEnable()
    {
        App.Delay(roundDuration, OnComplete, DisplayTime, false, true);
    }
    

    private void OnComplete()
    {
        Debug.Log("Time Up");
    }
    

    private void DisplayTime(float time)
    {
        var remainingTime = roundDuration - time;
        int hours = Mathf.FloorToInt(remainingTime / 3600);
        int minutes = Mathf.FloorToInt((remainingTime % 3600) / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        
        roundTimeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";

        if (remainingTime <= 30f)
        {
            roundTimeText.color = Color.red;
        }
        else
        {
            roundTimeText.color = Color.white;
        }
    }
}