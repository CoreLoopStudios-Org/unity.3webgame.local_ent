using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.UIButton;

public class GameController : MonoBehaviour
{
    
    public IntegerEvent integerEvent;
    [SerializeField] private float roundDuration = 120f;
    [SerializeField] private TMP_Text roundTimeText;

    private float currentRoundTime = 0f;

    private void OnEnable()
    {
        integerEvent.AddListener(Flipface);
        //TimeSetReset();
    }


    private void OnDisable()
    {
        integerEvent.RemoveListener(Flipface);
    }
    

    public void Flipface(int index)
    {
        var selected = transform.GetChild(index);
        selected.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        var buttonUI = selected.GetComponent<ButtonUI>();
        buttonUI.enabled = false;
    }

    public void TimeSetReset()
    {
        currentRoundTime = roundDuration;
        currentRoundTime -= Time.deltaTime;
        if (roundTimeText != null)
        {
            int minutes = Mathf.FloorToInt(currentRoundTime / 60);
            int seconds = Mathf.FloorToInt(currentRoundTime % 60);
            roundTimeText.text = $"{minutes:00}:{seconds:00}";

            // Change color when time is running out
            if (currentRoundTime <= 30f)
            {
                roundTimeText.color = Color.red;
            }
            else
            {
                roundTimeText.color = Color.white;
            }
        }
    }
    
}
