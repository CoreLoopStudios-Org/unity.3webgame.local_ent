using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Animation;
using TMPro;
using VirtueSky.Misc;

public class BrushAction : MonoBehaviour
{
    [SerializeField]int usageCount = 3;
    [SerializeField] private TMP_Text usageCountText;
    
    int columns = 10;
    int rows = 10;
    private List<int> columsHoldingRewards = new List<int>();
    private List<int> uniqueColumsHoldingRewards = new HashSet<int>().ToList();
    
    void OnEnable()
    {
        int currentUsageCount = int.Parse(usageCountText.text);
        usageCount = currentUsageCount;
        usageCountText.text = usageCount.ToString();
    }
    
    [ContextMenu("Brush Action")]
    public void OnActionBrush1()
    {
        int currentUsageCount = int.Parse(usageCountText.text);
        usageCount = currentUsageCount;
        if (usageCount > 0)
        {
            BrushFundtion();
            usageCount--;
            usageCountText.text = usageCount.ToString();
        }
        
       
    }

    void BrushFundtion()
    {
        SpawnGame2.RewardData[] data = SpawnGame2.arr2;
        List<int> rewardNumbers = data.Select(d => d.rewardNumber).ToList();
        //Debug.Log("Rewards = [" + string.Join(", ", rewardNumbers) + "]");
        
        columsHoldingRewards.Clear();
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                int index = row * columns + col;
                if (rewardNumbers.Contains(index))
                {
                    columsHoldingRewards.Add(col);
                }
            }
        }

        uniqueColumsHoldingRewards = new HashSet<int>(columsHoldingRewards).ToList();
        uniqueColumsHoldingRewards.Shuffle();
        
        //Debug.Log("[" + string.Join(", ", columsHoldingRewards) + "]");
        //Debug.Log("[" + string.Join(", ", uniqueColumsHoldingRewards) + "]");
        
        
        var transformChildCount = transform.childCount;
        for (int i = transformChildCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).transform.GetChild(1);
            child.gameObject.SetActive(false);
        }
        
        int columnToHighlight = uniqueColumsHoldingRewards[0];
        for (int row = 0; row < rows; row++)
        {
            int index = row * columns + columnToHighlight;
            var child = transform.GetChild(index).transform.GetChild(1);
            child.gameObject.SetActive(true);

        }
    }
    

}
