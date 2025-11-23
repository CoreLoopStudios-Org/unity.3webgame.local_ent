using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.UIButton;

public class Highlight : MonoBehaviour
{
    int columns = 10;
    int rows = 10;
    int totalPrizeCountPerColumn = 0;
    int tier1PrizeCount = 0;
    int tier2PrizeCount = 0;
    int tier3PrizeCount = 0;
    int priority ;
    
    public Dictionary<int, int> columnReport = new Dictionary<int, int>();
    public Dictionary<int, int> columnPriority = new Dictionary<int, int>();
    
    [Header("Keep same as Spawn Game 2 script Rarity names")]
    [SerializeField] private static string[] _rarityNames = new string[] {"Common", "Rare", "Gold"};
    public Dictionary<string, int> prizeOccurances = new Dictionary<string, int>() 
    {
        {_rarityNames[0], 0}, 
        {_rarityNames[1], 0}, 
        {_rarityNames[2], 0}
    };
    
    
    
    [ContextMenu("Highlight")]
    public void HighlightColumn()
    {
        columnReport.Clear();
        columnPriority.Clear();
        
        for (int col = 0; col < columns; col++)
        {
            int highestPriorityInColumn = 0;
            
            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                var child = transform.GetChild(index);
                var rarityObject = child.transform.GetChild(0).gameObject;
                
                if (rarityObject.activeSelf)
                {
                    totalPrizeCountPerColumn++;
                    if (rarityObject.name == "Gold")
                    {
                        priority = 3;
                        tier3PrizeCount++;
                    }
                    else if (rarityObject.name == "Rare")
                    {
                        priority = 2;
                        tier2PrizeCount++;
                    }
                    else if (rarityObject.name == "Common")
                    {
                        priority = 1;
                        tier1PrizeCount++;
                    }
                    
                    
                    if (priority > highestPriorityInColumn)
                    {
                        highestPriorityInColumn = priority;
                    }
                    
                    
                }
            }
            columnReport.Add(col, totalPrizeCountPerColumn);
            columnPriority.Add(col, highestPriorityInColumn);
            totalPrizeCountPerColumn = 0; 
        }
        
        prizeOccurances[_rarityNames[0]] = tier1PrizeCount;
        prizeOccurances[_rarityNames[1]] = tier2PrizeCount;
        prizeOccurances[_rarityNames[2]] = tier3PrizeCount;
        
        
        int maxPrizeCount =  columnReport.Max(x => x.Value);
        var columnsWithMaxPrize = columnReport.Where(x => x.Value == maxPrizeCount).Select(x => x.Key).ToList();
        int columnToHighlight = columnsWithMaxPrize.OrderByDescending(col => columnPriority[col]).First();
        string mostCommonOccurance = prizeOccurances.OrderByDescending(x => x.Value).First().Key;
        
        for (int row = 0; row < rows; row++)
        {
            int index = row * columns + columnToHighlight;
            var child = transform.GetChild(index).transform.GetChild(1);
            child.gameObject.SetActive(true);
            
        }
        
        
        Debug.Log($"Column {columnToHighlight} highlighted - Prizes: {maxPrizeCount}, Priority: {columnPriority[columnToHighlight]}");  
        Debug.Log("Most common color this round: " + mostCommonOccurance);  
    }
}
