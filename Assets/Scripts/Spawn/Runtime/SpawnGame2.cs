using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Events;
using VirtueSky.UIButton;
using Random = UnityEngine.Random;

public class SpawnGame2 : MonoBehaviour
{
    [Header("Spawn properties")]
    public Button prefab;
    public Color[] prefabColors;
    public EventNoParam eventNoParm;
    public IntegerEvent clickedIndex;
    
    private bool isSpawned = false;
    
    [Header("Reward properties")]
    public BooleanEvent roundEndEvent;
    int[] arr = new int[100];
    [SerializeField] Color[] rarityColors = new Color[0];

    public struct RewardData
    {
        public int rewardNumber;
        public int rarity;
    }
    
    private RewardData[] arr2 = new RewardData[20];
     
    
    
    
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
            QuickPlayGame();
        }
    }

    public void QuickPlayGame()
    {
        DisableAllButtons();
        foreach (var index in arr2)
        {
            var buttonGO = transform.GetChild(index.rewardNumber).gameObject;
            buttonGO.transform.GetChild(0).gameObject.SetActive(true);
            Image image = buttonGO.GetComponent<Image>(); 
            image.color = GetRarityColor(index.rarity);
        }

    }
    public void PlayGame()
    {
        DisableAllButtons();
        StartCoroutine(ActivateRewards());
    }

    IEnumerator ActivateRewards()
    {
        foreach (var item in arr2)
        {
            var buttonGO = transform.GetChild(item.rewardNumber).gameObject;
            buttonGO.transform.GetChild(0).gameObject.SetActive(true);
            Image image = buttonGO.GetComponent<Image>(); 
            image.color = GetRarityColor(item.rarity);
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
        
    private int GetRandomRarity()
    {
        float rand = Random.Range(0f, 100f);
        if (rand < 70f) return 0; //  (70%)
        if (rand < 90f) return 1; //  (20%)
        return 2; //  (10%)
    }

    private Color GetRarityColor(int rarity)
    {
        switch(rarity)
        {
            case 0: return rarityColors[0];//blue
            case 1: return rarityColors[1];//purple
            case 2: return rarityColors[2];//gold
            default: return Color.white;
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
        for (int i = 0; i < 20; i++)
        {
            arr2[i] = new RewardData 
            { 
                rewardNumber = arr[i], 
                rarity = GetRandomRarity() 
            };
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

    public void DisableAllButtons()
    {
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            var buttonUI = transform.GetChild(i).GetComponent<ButtonUI>();
            buttonUI.enabled = false;
        }
    }
}