using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI component for individual history items
/// Displays match statistics in a compact format
/// </summary>
public class HistoryItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text matchNumberText;
    [SerializeField] private TMP_Text timestampText;
    [SerializeField] private TMP_Text columnText;
    [SerializeField] private TMP_Text prizesText;
    [SerializeField] private TMP_Text rarityText;
    [SerializeField] private Image rarityColorIndicator;
    [SerializeField] private Image backgroundImage;
    
    [Header("Color Settings")]
    [SerializeField] private Color[] rarityColors = new Color[] 
    {
        new Color(0.3f, 0.5f, 1f),    // Common - Blue
        new Color(0.7f, 0.3f, 1f),     // Rare - Purple
        new Color(1f, 0.84f, 0f)       // Gold - Gold
    };
    
    private MatchHistoryData currentData;
    
    /// <summary>
    /// Set data for this history item
    /// </summary>
    public void SetData(MatchHistoryData data)
    {
        currentData = data;
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        if (currentData == null) return;
        
        // Basic info
        if (matchNumberText != null)
            matchNumberText.text = $"Match #{currentData.matchNumber}";
        
        if (timestampText != null)
            timestampText.text = currentData.timestamp;
        
        if (columnText != null)
            columnText.text = $"Col {currentData.highlightedColumn}";
        
        if (prizesText != null)
            prizesText.text = $"{currentData.totalPrizesInColumn} Prizes";
        
        if (rarityText != null)
            rarityText.text = currentData.mostCommonRarity;
        
        // Color indicator based on rarity
        if (rarityColorIndicator != null)
        {
            rarityColorIndicator.color = GetRarityColor(currentData.mostCommonRarity);
        }
        
        // Optional: Alternate background colors for better readability
        if (backgroundImage != null)
        {
            bool isEven = currentData.matchNumber % 2 == 0;
            backgroundImage.color = isEven ? new Color(0.2f, 0.2f, 0.2f, 0.5f) : new Color(0.15f, 0.15f, 0.15f, 0.5f);
        }
    }
    
    private Color GetRarityColor(string rarity)
    {
        switch (rarity.ToLower())
        {
            case "common": return rarityColors[0];
            case "rare": return rarityColors[1];
            case "gold": return rarityColors[2];
            default: return Color.white;
        }
    }
    
    /// <summary>
    /// Optional: Button click to show detailed stats
    /// </summary>
    public void OnItemClicked()
    {
        if (currentData == null) return;
        
        // Show detailed popup or expand item
        Debug.Log($"Match {currentData.matchNumber} Details:\n" +
                  $"Column: {currentData.highlightedColumn}\n" +
                  $"Prizes: {currentData.totalPrizesInColumn}\n" +
                  $"Priority: {currentData.columnPriority}\n" +
                  $"Most Common: {currentData.mostCommonRarity}");
        
        // You can implement a detailed view popup here
    }
    
    /// <summary>
    /// Get formatted summary text
    /// </summary>
    public string GetSummary()
    {
        if (currentData == null) return "No Data";
        
        return $"Match #{currentData.matchNumber} - Col {currentData.highlightedColumn} " +
               $"({currentData.totalPrizesInColumn} {currentData.mostCommonRarity})";
    }
}