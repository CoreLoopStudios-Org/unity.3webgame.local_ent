using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;


public class SpawnMineBoard : MonoBehaviour
{
    public Button minesButtonPrefab;
    public TMP_Text mineCountText;
    public IntegerEvent minesButtonClickedIndex;
    
    private bool isSpawned = false;
    
    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        
    }

    public void SpawnMineSettingButtons()
    {
        if (!isSpawned)
        {
            for (int i = 2; i < 25; i++)
            {
                var button = Instantiate(minesButtonPrefab, transform.position, transform.rotation, transform);
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
        minesButtonClickedIndex.Raise(i);
        mineCountText.text = i.ToString();
    }

}
