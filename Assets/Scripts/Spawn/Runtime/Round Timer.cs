using System;
using TMPro;
using UnityEngine;
using VirtueSky.Events;

public class RoundTimer : MonoBehaviour
{
    [SerializeField] private float roundDuration = 120f;
    [SerializeField] private TMP_Text roundTimeText; 
    private float currentRoundTime;
    private bool isTimeRunning = false;

    public EventNoParam eventNoParm;
    
    private void OnEnable()
    {
        eventNoParm.AddListener(TimerSetReset);
    }


    private void OnDisable()
    {
        eventNoParm.RemoveListener(TimerSetReset);
    }

    private void TimerSetReset()
    {
        isTimeRunning = true;
    }
    
    private void Start()
    {
        currentRoundTime = roundDuration;
    }

    private void Update()
    {
        if (isTimeRunning)
        {
            if (currentRoundTime > 0)
            {
                currentRoundTime -= Time.deltaTime;  
                DisplayTime(currentRoundTime);
            }
            else
            {
                currentRoundTime = 0f;
                DisplayTime(currentRoundTime); 
            }
        }
       
    }

    private void DisplayTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        
        roundTimeText.text = $"{hours:00}:{minutes:00}:{seconds:00}";

        if (time <= 30f)
        {
            roundTimeText.color = Color.red;
        }
        else
        {
            roundTimeText.color = Color.white;
        }
    }
}