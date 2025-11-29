using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.Events;
using VirtueSky.UIButton;

public class RoundTimerAndWin : MonoBehaviour
{
    [Header("Round Timer Properties")]
    [SerializeField] private float roundDuration = 120f;
    [SerializeField] private float inBetweenRoundsDuration = 5f;
    [SerializeField] private TMP_Text roundTimeText;
    public BooleanEvent roundEndEvent;
    
    [Header("Win Logic Properties")]
    public IntegerEvent integerEvent;
    private List<int> selected = new List<int>();
    private List<int> won = new List<int>();
    int selectedCount = 0;
    private bool isEmpty;

    [Header("Enter Exit Properties")]
    public GameObject WinningPanel;
    public GameObject LossingPanel;
    public GameObject BackToHomeButton;
    public GameObject nextRoundStartsIn;
    public TMP_Text nextRoundCountDownText;
    
    private void OnEnable()
    {
        OnStartTimer();
    }

    void OnStartTimer()
    {
        nextRoundStartsIn.gameObject.SetActive(false);
        WinningPanel.SetActive(false);
        LossingPanel.SetActive(false);
        selected.Clear();
        won.Clear();
        App.Delay(roundDuration, OnComplete, DisplayTime, false, true);
        integerEvent.AddListener(OnSelected);
        roundEndEvent.AddListener(Reset);
        DisableButton();
    }

    void OnEndTimer()
    {
        roundEndEvent.RemoveListener(Reset);
        EnableButton();
    }

    private void OnDisable()
    {
        OnEndTimer();
    }

    void StartNextRound()
    {
        nextRoundStartsIn.gameObject.SetActive(true);

        App.Delay(inBetweenRoundsDuration, OnCountdownComplete, DisplayCountDownForNextRound, false, true);
    }

    private void OnComplete()
    {
        Debug.Log("Round Timer OnComplete");
        roundEndEvent.Raise(true);
        integerEvent.RemoveListener(OnSelected);
        WinningCondition();
    }
    
    void ShowResultPanels(bool showWin, bool showLoss)
    {
        WinningPanel.SetActive(showWin);
        LossingPanel.SetActive(showLoss);
    }

    private void OnCountdownComplete()
    {
        OnStartTimer();
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
    
    private void DisplayCountDownForNextRound(float time)
    {
        var remainingTime = inBetweenRoundsDuration - time;
        int seconds = Mathf.FloorToInt(remainingTime);
    
        nextRoundCountDownText.text = seconds.ToString();
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
        
        isEmpty = !won.Any();
        
        StartCoroutine(Wait());

        Debug.Log("[" + string.Join(", ", won) + "]");
    }

    #endregion
    
    [ContextMenu("Reset")]
    public void Reset(bool isReset)
    {
        if (isReset)
        {
            OnEndTimer();
            StartCoroutine(Wait());
        }
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        StartNextRound();
        ShowResultPanels(!isEmpty, isEmpty);
    }

    void EnableButton()
    {
        var buttonUI = BackToHomeButton.gameObject.GetComponent<ButtonUI>();
        buttonUI.enabled = true;
    }
    
    void DisableButton()
    {
        var buttonUI = BackToHomeButton.gameObject.GetComponent<ButtonUI>();
        buttonUI.enabled = false;
    }
}