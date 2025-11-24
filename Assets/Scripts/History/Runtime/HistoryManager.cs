using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

/// <summary>
/// Manages match history with object pooling and efficient rendering
/// Optimized for low-end devices
/// </summary>
public class HistoryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject historyItemPrefab;
    [SerializeField] private Transform historyContainer;
    [SerializeField] private ScrollRect scrollRect;
    
    [Header("Settings")]
    [SerializeField] private int maxHistoryCount = 50; // Total stored in memory
    [SerializeField] private int visibleHistoryCount = 15; // Visible at once
    
    // Data storage
    private List<MatchHistoryData> matchHistory = new List<MatchHistoryData>();
    private int currentMatchNumber = 0;
    
    // Object pool for UI elements
    private Queue<HistoryItemUI> itemPool = new Queue<HistoryItemUI>();
    private List<HistoryItemUI> activeItems = new List<HistoryItemUI>();
    
    private const string SAVE_KEY = "MatchHistorySave";
    
    private void Awake()
    {
        InitializePool();
        LoadHistory();
    }
    
    /// <summary>
    /// Pre-instantiate pool objects to avoid runtime allocation
    /// </summary>
    private void InitializePool()
    {
        for (int i = 0; i < visibleHistoryCount; i++)
        {
            CreatePooledItem();
        }
    }
    
    private HistoryItemUI CreatePooledItem()
    {
        GameObject obj = Instantiate(historyItemPrefab, historyContainer);
        HistoryItemUI item = obj.GetComponent<HistoryItemUI>();
        if (item == null)
        {
            item = obj.AddComponent<HistoryItemUI>();
        }
        obj.SetActive(false);
        itemPool.Enqueue(item);
        return item;
    }
    
    /// <summary>
    /// Add new match data to history
    /// Call this after each round ends
    /// </summary>
    public void AddMatchHistory(int column, int prizes, int priority, string commonRarity, Dictionary<string, int> distribution)
    {
        currentMatchNumber++;
        
        var newMatch = new MatchHistoryData(
            currentMatchNumber,
            column,
            prizes,
            priority,
            commonRarity,
            distribution
        );
        
        // Add to front of list (newest first)
        matchHistory.Insert(0, newMatch);
        
        // Enforce max history limit
        if (matchHistory.Count > maxHistoryCount)
        {
            matchHistory.RemoveAt(matchHistory.Count - 1);
        }
        
        SaveHistory();
        RefreshDisplay();
    }
    
    /// <summary>
    /// Refresh the visible history items
    /// Only creates/updates what's needed
    /// </summary>
    public void RefreshDisplay()
    {
        // Return all active items to pool
        ReturnAllToPool();
        
        // Display only the most recent matches up to visibleHistoryCount
        int itemsToShow = Mathf.Min(matchHistory.Count, visibleHistoryCount);
        
        for (int i = 0; i < itemsToShow; i++)
        {
            HistoryItemUI item = GetPooledItem();
            item.SetData(matchHistory[i]);
            item.transform.SetAsLastSibling(); // Maintain order
            activeItems.Add(item);
        }
        
        // If we have more history than visible, enable scrolling
        if (scrollRect != null)
        {
            scrollRect.vertical = matchHistory.Count > visibleHistoryCount;
        }
    }
    
    private HistoryItemUI GetPooledItem()
    {
        if (itemPool.Count == 0)
        {
            return CreatePooledItem();
        }
        
        HistoryItemUI item = itemPool.Dequeue();
        item.gameObject.SetActive(true);
        return item;
    }
    
    private void ReturnAllToPool()
    {
        foreach (var item in activeItems)
        {
            item.gameObject.SetActive(false);
            itemPool.Enqueue(item);
        }
        activeItems.Clear();
    }
    
    /// <summary>
    /// Save history to PlayerPrefs as JSON
    /// </summary>
    private void SaveHistory()
    {
        try
        {
            HistorySaveData saveData = new HistorySaveData
            {
                currentMatchNumber = this.currentMatchNumber,
                matches = matchHistory
            };
            
            string json = JsonUtility.ToJson(saveData);
            PlayerPrefs.SetString(SAVE_KEY, json);
            PlayerPrefs.Save();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to save history: {e.Message}");
        }
    }
    
    /// <summary>
    /// Load history from PlayerPrefs
    /// </summary>
    private void LoadHistory()
    {
        try
        {
            if (PlayerPrefs.HasKey(SAVE_KEY))
            {
                string json = PlayerPrefs.GetString(SAVE_KEY);
                HistorySaveData saveData = JsonUtility.FromJson<HistorySaveData>(json);
                
                if (saveData != null)
                {
                    currentMatchNumber = saveData.currentMatchNumber;
                    matchHistory = saveData.matches ?? new List<MatchHistoryData>();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to load history: {e.Message}");
            matchHistory = new List<MatchHistoryData>();
        }
    }
    
    /// <summary>
    /// Clear all history data
    /// </summary>
    public void ClearHistory()
    {
        matchHistory.Clear();
        currentMatchNumber = 0;
        PlayerPrefs.DeleteKey(SAVE_KEY);
        ReturnAllToPool();
    }
    
    /// <summary>
    /// Get total match count
    /// </summary>
    public int GetTotalMatches() => matchHistory.Count;
    
    /// <summary>
    /// Get specific match data
    /// </summary>
    public MatchHistoryData GetMatch(int index)
    {
        if (index >= 0 && index < matchHistory.Count)
            return matchHistory[index];
        return null;
    }
}

/// <summary>
/// Wrapper for JSON serialization
/// </summary>
[System.Serializable]
public class HistorySaveData
{
    public int currentMatchNumber;
    public List<MatchHistoryData> matches;
}