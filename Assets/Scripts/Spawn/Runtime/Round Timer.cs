using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;

public class RoundTimerAndWin : MonoBehaviour
{
    [Header("Round Timer Properties")]
    [SerializeField] private float roundDuration = 120f;
    [SerializeField] private TMP_Text roundTimeText;
    public BooleanEvent roundEndEvent;
    
    [Header("Win Logic Properties")]
    public IntegerEvent integerEvent;
    private List<int> selected = new List<int>();
    private List<int> won = new List<int>();
    int selectedCount = 0;

    public GameObject WinningPanel;
    public GameObject LossingPanel;
    
    private void OnEnable()
    {
        selected.Clear();
        won.Clear();
        WinningPanel.SetActive(false);
        LossingPanel.SetActive(false);
        App.Delay(roundDuration, OnComplete, DisplayTime, false, true);
        integerEvent.AddListener(OnSelected);
    }
    

    private void OnComplete()
    {
        Debug.Log("Round Timer OnComplete");
        roundEndEvent.Raise(true);
        integerEvent.RemoveListener(OnSelected);
        WinningCondition();
    }

    #region Timer

    private void DisplayTime(float time)
    {
        var remainingTime = roundDuration - time;
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        int centiseconds = Mathf.FloorToInt((remainingTime * 100) % 100);
        
        roundTimeText.text = $"{minutes:00}  {seconds:00}  {centiseconds:00}";

        if (remainingTime <= 30f)
        {
            roundTimeText.color = Color.red;
        }
        else
        {
            roundTimeText.color = Color.white;
        }
    }
    #endregion
    
    #region WinLogic
    
    void OnSelected(int index)
    {
        selected.Add(index);
        selectedCount++;
    }

    public void WinningCondition()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            if (selected.Contains(i))
            {
                var child = transform.GetChild(i).transform.GetChild(0);
                string childName = child.name;
                if (child.gameObject.activeSelf)
                {
                    won.Add(i);
                }
            }
        }
        bool isEmpty = !won.Any();
        if(isEmpty)
        {
            WinningPanel.SetActive(false);
            LossingPanel.SetActive(true);
        }
        else
        {
            WinningPanel.SetActive(true);
            LossingPanel.SetActive(false);
        }


        Debug.Log("[" + string.Join(", ", won) + "]");
        
    }

    #endregion
   
}