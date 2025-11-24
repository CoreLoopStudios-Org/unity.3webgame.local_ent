using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MatchHistoryData
{
    public int matchNumber;
    public string timestamp;
    public int highlightedColumn;
    public int totalPrizesInColumn;
    public int columnPriority;
    public string mostCommonRarity;
    
    // Prize distribution (rarity -> count)
    public SerializableDictionary<string, int> prizeDistribution;
    
    // Constructor
    public MatchHistoryData(int matchNum, int column, int prizes, int priority, string commonRarity, Dictionary<string, int> distribution)
    {
        matchNumber = matchNum;
        timestamp = DateTime.Now.ToString("HH:mm:ss");
        highlightedColumn = column;
        totalPrizesInColumn = prizes;
        columnPriority = priority;
        mostCommonRarity = commonRarity;
        
        prizeDistribution = new SerializableDictionary<string, int>();
        foreach(var kvp in distribution)
        {
            prizeDistribution.Add(kvp.Key, kvp.Value);
        }
    }
}

/// <summary>
/// Simple serializable dictionary wrapper
/// </summary>
[Serializable]
public class SerializableDictionary<TKey, TValue>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
    
    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);
    }
    
    public Dictionary<TKey, TValue> ToDictionary()
    {
        var dict = new Dictionary<TKey, TValue>();
        for (int i = 0; i < keys.Count; i++)
        {
            dict[keys[i]] = values[i];
        }
        return dict;
    }
    
    public int Count => keys.Count;
}