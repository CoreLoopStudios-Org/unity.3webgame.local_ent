using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;


public class SpawnCurrencyBoard : MonoBehaviour
{
    public Button moneyButtonPrefab;
    public TMP_Text moneyCountText;
    public IntegerEvent moneyButtonClickedIndex;
    
    private bool isSpawned = false;
    
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    public void SpawnCurrencyBoardButtons()
    {
        if (!isSpawned)
        {
            for (int i = 0; i < 4; i++)
            {
                var button = Instantiate(moneyButtonPrefab, transform.position, transform.rotation, transform);
                TMP_Text text = button.GetComponentInChildren<TMP_Text>();
                text.text = i.ToString(); 
                int index = i; 
                button.onClick.AddListener(() => OnButtonClick(index));
                var gameObjectName = button.gameObject.name;
                button.gameObject.name = String.Format(gameObjectName, (i).ToString());
            }
            isSpawned = true;
        }
    }
    
    public void OnButtonClick(int i)
    {
        moneyButtonClickedIndex.Raise(i);
        int value;
        switch (i)
        {
            case 0:
                value = i + 2;
                moneyCountText.text = value.ToString();
                break;
            case 1: 
                value = i + 5;
                moneyCountText.text = value.ToString();
                break;
            case 2: 
                value = i + 7;
                moneyCountText.text = value.ToString();
                break;
            case 3:
                value = i + 10;
                moneyCountText.text = value.ToString();
                break;
        }
       
    }

}