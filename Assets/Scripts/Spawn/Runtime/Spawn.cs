using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;

public class Spawn : MonoBehaviour
{
    public Button prefab;
    public EventNoParam eventNoParm;
    public EventNoParam resetEvent;
    public IntegerEvent clickedIndex;
    
    private bool isSpawned = false;

    

    private void OnEnable()
    {
        eventNoParm.AddListener(SpawnRequest);
        resetEvent.AddListener(Reset);
    }
    
    private void OnDisable()
    {
        eventNoParm.RemoveListener(SpawnRequest);
        resetEvent.RemoveListener(Reset);
    }

    private void SpawnRequest()
    {
        if (!isSpawned)
        {
            Spawn100();
        }
    }
        
    [ContextMenu("Spawn")]
    public void Spawn100()
    {
        for (int i = 0; i < 100; i++)
        {
            var button = Instantiate(prefab, transform.position, transform.rotation, transform);
            //TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            //text.text = i.ToString(); 
            int index = i; 
            button.onClick.AddListener(() => OnButtonClick(index));
            var gameObjectName = button.gameObject.name;
            button.gameObject.name = String.Format(gameObjectName, i.ToString());
        }
        isSpawned = true;
    }

    [ContextMenu("Reset")]
    public void Reset()
    {
        StartCoroutine(WaitTime(0.2f));
    }

    IEnumerator WaitTime(float i)
    {
        UnSpawn();
        yield return new WaitForSeconds(i);
        Spawn100();
    }
    
    

    public void OnButtonClick(int i)
    {
        clickedIndex.Raise(i);
    }


    [ContextMenu("UnSpawn")]
    public void UnSpawn()
    {
        // Remove all children
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
#if UNITY_EDITOR
            DestroyImmediate(transform.GetChild(i).gameObject);
#else
            Destroy(transform.GetChild(i).gameObject);
#endif
        }
        isSpawned = false;
        
    }
}