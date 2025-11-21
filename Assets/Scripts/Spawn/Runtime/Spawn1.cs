using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using Random = UnityEngine.Random;

public class Spawn1 : MonoBehaviour
{
    public Button prefab;
    public Color[] prefabColors;
    public EventNoParam eventNoParm;
    public IntegerEvent clickedIndex;
    public BooleanEvent roundEndEvent;
    
    private bool isSpawned = false;
    int[] arr = new int[100];
    int[] arr2 = new int[20];

    

    private void OnEnable()
    {
        Spawn100();
        eventNoParm.AddListener(PlayGame);
        roundEndEvent.AddListener(ShowRewardsOnTimeup);
    }
    
    private void OnDisable()
    {
        eventNoParm.RemoveListener(PlayGame);
        roundEndEvent.RemoveListener(ShowRewardsOnTimeup);
    }

    private void ShowRewardsOnTimeup(bool show)
    {
        if (show)
        {
            PlayGame();
        }
    }

    public void QuickPlayGame()
    {
        foreach (var index in arr2)
        {
            var buttonGO = transform.GetChild(index).gameObject;
            buttonGO.transform.GetChild(0).gameObject.SetActive(true);
        }

    }
    public void PlayGame()
    {
        StartCoroutine(ActivateRewards());
    }

    IEnumerator ActivateRewards()
    {
        foreach (var item in arr2)
        {
            var buttonGO = transform.GetChild(item).gameObject;
            buttonGO.transform.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    public static void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n > 1) 
        {
            int k =  Random.Range(0, n--);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
        
    
    [ContextMenu("Spawn")]
    public void Spawn100()
    {
        for (int i = 0; i < 100; i++)
        {
            var button = Instantiate(prefab, transform.position, transform.rotation, transform);
            TMP_Text text = button.GetComponentInChildren<TMP_Text>();
            text.text = i.ToString(); 
            int index = i; 
            button.onClick.AddListener(() => OnButtonClick(index));
            var gameObjectName = button.gameObject.name;
            button.gameObject.name = String.Format(gameObjectName, i.ToString());
            
            Image buttonImage = button.GetComponent<Image>();
            if (prefabColors.Length > 0)
            {
                int colorIndex = (i / 10) % prefabColors.Length;
                buttonImage.color = prefabColors[colorIndex];
            }
        }
        
        for (int i = 0; i < arr.Length; i++) arr[i] = i;
        Shuffle(arr);
        for(int i=0; i<20; i++) arr2[i] = arr[i];
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