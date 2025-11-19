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
        switch (i)
        {
            case 0: 
                moneyCountText.text = (i+2).ToString();
                break;
            case 1: 
                moneyCountText.text = (i+5).ToString();
                break;
            case 2: 
                moneyCountText.text = (i+7).ToString();
                break;
            case 3:
                moneyCountText.text = (i+10).ToString();
                break;
        }
       
    }

}