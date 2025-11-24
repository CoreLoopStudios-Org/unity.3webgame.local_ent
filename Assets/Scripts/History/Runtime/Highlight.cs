using System;
using System.Collections.Generic;
using System.Linq;
using LitMotion.Animation;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.UIButton;

public class Highlight : MonoBehaviour
{
    int columns = 10;
    int rows = 10;
    int totalPrizeCountPerColumn = 0;
    int totalPrizeCountPerRow = 0;
    int tier1PrizeCount = 0;
    int tier2PrizeCount = 0;
    int tier3PrizeCount = 0;
    int priority;
    private int columnToHighlight;
    
    
    public int[] columnPriorityArray = new int[10];
    public static int[] maxPrizePerColumnArray = new int[10];
    public int[] rowReportArray = new int[10]; 
    public int[,] columnReportMap = new int[10,10];
    public Color[,] spawnGrid = new Color[10, 10];

    [Header("Keep same as Spawn Game 2 script Rarity names")] [SerializeField]
    private static string[] _rarityNames = new string[] { "Common", "Rare", "Gold" };

    public Dictionary<string, int> prizeOccurances = new Dictionary<string, int>()
    {
        { _rarityNames[0], 0 },
        { _rarityNames[1], 0 },
        { _rarityNames[2], 0 }
    };
    
    

    #region  Highlights
    [ContextMenu("Highlight")]
    public void HighlightColumn()
    {

        int columns = spawnGrid.GetLength(0);
        int rows = spawnGrid.GetLength(1);

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
                    spawnGrid[col, row] = rarityObject.GetComponent<Image>().color;
                    //Debug.Log(spawnGrid[col, row]);
                    totalPrizeCountPerColumn++;
                    totalPrizeCountPerRow++;
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
                    
                    columnReportMap[col, row] = totalPrizeCountPerColumn;
                    rowReportArray[row] = rowReportArray[row] + totalPrizeCountPerRow;
                    totalPrizeCountPerRow = 0;
                }
                else
                {
                    spawnGrid[col, row] = Color.white;
                }
 
            }
            
            totalPrizeCountPerColumn = 0;
            columnPriorityArray[col] = highestPriorityInColumn;
        }

        prizeOccurances[_rarityNames[0]] = tier1PrizeCount;
        prizeOccurances[_rarityNames[1]] = tier2PrizeCount;
        prizeOccurances[_rarityNames[2]] = tier3PrizeCount;


        string mostCommonOccurance = prizeOccurances.OrderByDescending(x => x.Value).First().Key;
        int maxPrize = maxPrizePerColumnArray.Max();
        int columnToHighlight =  maxPrizePerColumnArray
            .Select((prize, col) => new { prize, col, priority = columnPriorityArray[col] })
            .Where(x => x.prize == maxPrize)
            .OrderByDescending(x => x.priority)
            .First()
            .col;

        for (int row = 0; row < rows; row++)
        {
            int index = row * columns + columnToHighlight;
            var child = transform.GetChild(index).transform.GetChild(1);
            child.gameObject.SetActive(true);

        }

        for (int col = 0; col < columnReportMap.GetLength(0); col++)
        {
            int maxValue = 0;
            for (int row = 0; row < columnReportMap.GetLongLength(1); row++)
            {
                int prizecount = columnReportMap[col, row];
                if (maxValue < prizecount)
                {
                    maxValue = prizecount;
                }
            }
            maxPrizePerColumnArray[col] = maxValue;
            maxValue = 0;
        }
        
        int maxRowValue = rowReportArray.Max();
        
        var maxRows = rowReportArray
            .Select((value, index) => new { value, index })
            .Where(x => x.value == maxRowValue)
            .ToList();
        
        List<int> rowNumbers = maxRows.Select(x => x.index).ToList();
        
        Color mostCommonColor = maxRows
            .Select(x => transform.GetChild(x.index).GetComponent<Image>().color)
            .GroupBy(color => color)
            .OrderByDescending(group => group.Count())
            .First()
            .Key;

        Debug.Log($"Row numbers with max value: {string.Join(", ", rowNumbers)}");
        Debug.Log($"Most common color: {mostCommonColor}");
        
        Debug.Log($"Column {columnToHighlight} highlighted - Prizes: {maxPrizePerColumnArray.Max()}, Priority: {columnPriorityArray.Max(col => col)}");
        Debug.Log("Most common prize this round: " + mostCommonOccurance);
    }
    #endregion

    #region PowerUp
    [ContextMenu("Brush Action")]
    public void OnActionBrush()
    {
        SpawnGame2.RewardData[] data = SpawnGame2.arr2;
        List<int> rewardNumbers = data.Select(d => d.rewardNumber).ToList();
        
        for (int col = 0; col < columns; col++)
        {

            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                var child = transform.GetChild(index).gameObject;
                child.GetComponent<LitMotionAnimation>().Play();
                if (rewardNumbers.Contains(index))
                {
                    var glowObject = child.transform.GetChild(3).gameObject;
                    glowObject.SetActive(true);
                    glowObject.GetComponent<LitMotionAnimation>().Play();
                
                }
            }
        }
    }
    

    #endregion
    
    #region Debugger
    [ContextMenu("Debug Color array")]
    public void DebugColorArray()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine($"SpawnGrid [{rows}x{columns}]:");
    
        for (int row = 0; row < rows; row++)
        {
            sb.Append($"Row {row}: ");
            for (int col = 0; col < columns; col++)
            {
                Color color = spawnGrid[col, row];
                sb.Append(GetColorName(color) + " | ");
            }
            sb.AppendLine();
        }
    
        Debug.Log(sb.ToString());
    }

    private string GetColorName(Color color)
    {
        if (color == Color.red) return "Red";
        if (color == Color.green) return "Green";
        if (color == Color.blue) return "Blue";
        if (color == Color.white) return "White";
        if (color == Color.black) return "Black";
        if (color == Color.yellow) return "Yellow";
        return $"({color.r:F2},{color.g:F2},{color.b:F2})";
    }
    
    [ContextMenu("Debug PrizeCount PerColumn")]
    public void DebugColumnReportMap()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Column Report Map:");
    
        // Column headers
        sb.Append("     ");
        for (int col = 0; col < 10; col++)
        {
            sb.Append($"C{col,2} ");
        }
        sb.AppendLine();
        sb.AppendLine(new string('-', 50));
    
        // Data rows
        for (int row = 0; row < 10; row++)
        {
            sb.Append($"R{row,2}: ");
            for (int col = 0; col < 10; col++)
            {
                sb.Append($"{columnReportMap[col, row],3} ");
            }
            sb.AppendLine();
        }
    
        Debug.Log(sb.ToString());
    }
    #endregion
}
    
